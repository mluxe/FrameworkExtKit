using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace FrameworkExtKit.Core.DataReaders {

    class ExcelColumnInfo {
        public string ColumnReferenceKey { get; set; }
        public string ColumnName { get; set; }
        // public string ExpectedPropertyName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }

    public class ExcelDataReader<T> : IDataReader<T> {

        private const string ROW_NUMBER_COLUMN = "RowNumber";
        
        protected IDictionary<String, string> ColumnPropertyMappings { get; set; }

        protected DataSourceErrorCollection errors = new DataSourceErrorCollection();
        protected DataSourceErrorCollection warnings = new DataSourceErrorCollection();
        protected ICollection<T> results = new List<T>();

        public DataSourceErrorCollection Errors { get { return this.errors; } }
        public DataSourceErrorCollection Warnings { get { return this.warnings; } }

        public IEnumerable<T> Data { get { return this.results; } }
        public String Source { get; set; }
        public long TotalRecords { private set; get; }

        public bool StrictDataType { get; set; } = false;

        public ExcelDataReader() {
            this.ColumnPropertyMappings = new Dictionary<String, string>();
            this.ConfigureColumnPropertyMappings();
        }


        protected virtual void ConfigureColumnPropertyMappings() {
            var propertyInfos = typeof(T).GetProperties();

            foreach(var info in propertyInfos) {
                var name = info.Name;

                if(!name.Equals(ROW_NUMBER_COLUMN, StringComparison.InvariantCultureIgnoreCase)) {
                    var expected_column_name = string.Concat(name.Select((x, i) => i > 0 & i < name.Length - 1 && char.IsLower(name[i + 1]) && char.IsUpper(x) ? " " + x.ToString() : x.ToString()));
                    this.ColumnPropertyMappings.Add(name, expected_column_name);
                }
                
            }
        }

        private string getCellTextValue(Cell cell, SharedStringTable sharedStringTable)  {
            string value_text = String.Empty;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString) {
                int ssid = int.Parse(cell.CellValue.Text);
                value_text = sharedStringTable.ChildElements[ssid].InnerText;
            } else if (cell.CellValue != null) {
                // var k = cell.CellReference.Value;
                value_text = cell.CellValue.Text;
            }
            return value_text.Trim();
        }

        protected void AddResult(string[] columns, string[] values) {

        }

        public bool ReadData() {

            if (String.IsNullOrEmpty(this.Source)) {
                throw new ArgumentNullException("data source has not been set");
            }

            if (!File.Exists(this.Source)) {
                throw new FileNotFoundException(this.Source);
            }

            var fileName = Path.GetFileName(this.Source);
            var extension = Path.GetExtension(this.Source);

            if(extension.ToLower() != ".xlsx") {
                throw new FileLoadException("only .xslx document is accepted", fileName);
            }

            using(SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(this.Source, false)) {

                results.Clear();

                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                SharedStringTablePart sharedStringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                SharedStringTable sharedStringTable = sharedStringTablePart.SharedStringTable;

                var rows = sheetData.Elements<Row>();

                this.TotalRecords = Math.Max(rows.LongCount() - 1, 0);

                if (rows.LongCount() > 0) {

                    // get columns from the first row
                    var column_row = rows.First();
                    var column_cells = column_row.Elements<Cell>();
                    List<ExcelColumnInfo> columns = new List<ExcelColumnInfo>();
                    //Dictionary<string, string> columns = new Dictionary<string, string>();
                    // string[] columns = new string[column_cells.Count()];

                    for (var i=0; i<column_cells.Count(); i++) {
                        var cell = column_cells.ElementAt(i);
                        var cell_ref = cell.CellReference;
                        Regex regex = new Regex("[A-Za-z]+");
                        Match match = regex.Match(cell_ref);
                        var column_title = match.Value;
                        columns.Add(new ExcelColumnInfo {
                            ColumnReferenceKey = column_title, ColumnName = getCellTextValue(column_cells.ElementAt(i), sharedStringTable)
                        });
                        // columns.Add(cell.CellReference, getCellTextValue(column_cells.ElementAt(i), sharedStringTable));
                    }


                    // check if all required columns exist
                    var column_errors = new DataSourceError(0);

                    foreach(var required_column_name in this.ColumnPropertyMappings.Values) {
                        if (!columns.Any(c => c.ColumnName.Equals(required_column_name, StringComparison.InvariantCultureIgnoreCase))) {
                            column_errors.Add(required_column_name, $"column {required_column_name} is not found");
                        }
                    }

                    if(column_errors.Count > 0) {
                        this.errors.Add(column_errors);
                    }


                    // continue load data if all columns are found
                    if(this.Errors.Count() == 0) {


                        // read property info from class
                        var propertyInfos = typeof(T).GetProperties();
                        var row_id_property = propertyInfos.FirstOrDefault(p => p.Name.Equals(ROW_NUMBER_COLUMN, StringComparison.InvariantCultureIgnoreCase));

                        for (var i=0; i< columns.Count; i++) {
                            var column = columns[i];

                            var has_pair = this.ColumnPropertyMappings.Any(k => k.Value.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase));

                            if (has_pair) {
                                var pair = this.ColumnPropertyMappings.First(k => k.Value.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase));
                                var property_name = pair.Key;
                                var info = propertyInfos.First(p => p.Name == property_name);
                                column.PropertyInfo = info;
                            }
                        }

                        // start converting row data to class object
                        for(var row_num=1;row_num < rows.Count();row_num++) {
                            var row = rows.ElementAt(row_num);

                            T obj_instance = (T)Activator.CreateInstance(typeof(T));
                            var cells = row.Elements<Cell>();

                            string[] row_value_texts = new string[cells.Count()];
                            DataSourceError error = new DataSourceError(row_num);
                            DataSourceError warning = new DataSourceError(row_num);

                            if (row_id_property != null) {
                                row_id_property.SetValue(obj_instance, row_num + 1);
                            }

                            for (var i = 0; i < columns.Count; i++) {
                                var column = columns[i];
                                
                                var property_info = column.PropertyInfo;

                                if (property_info != null) {

                                    var cell_ref = $"{column.ColumnReferenceKey}{row_num + 1}";
                                    var cell = cells.FirstOrDefault(c => c.CellReference.Value == cell_ref);
                                    
                                    if (cell != null) {
                                        var column_text_value = getCellTextValue(cell, sharedStringTable);
                                        object value = null;
                                        bool value_parsed = false;
                                        if (property_info.PropertyType == typeof(DateTime) || property_info.PropertyType == typeof(DateTime?)) {
                                            double datetime_double;
                                            DateTime datetime;
                                            if (Double.TryParse(column_text_value, out datetime_double)) {
                                                value = DateTime.FromOADate(datetime_double);
                                                value_parsed = true;
                                            } else if (DateTime.TryParse(column_text_value, out datetime)) {
                                                value = datetime;
                                                value_parsed = true;
                                            } else {
                                                error.Add(column.ColumnName, $"unable to convert '{column_text_value}' to datetime value");
                                            }
                                        } else if (property_info.PropertyType == typeof(int) || property_info.PropertyType == typeof(int?)) {
                                            int int_value = 0;
                                            if (int.TryParse(column_text_value, out int_value)) {
                                                value = int_value;
                                                value_parsed = true;
                                            } else {
                                                if (StrictDataType || column_text_value != String.Empty) {
                                                    error.Add(column.ColumnName, $"unable to convert '{column_text_value}' to number value");
                                                } else {
                                                    value_parsed = true;
                                                    warning.Add(column.ColumnName, $"'{column_text_value}' is not number value, set to default value {int_value}");
                                                }
                                            }
                                        } else if (property_info.PropertyType == typeof(long) || property_info.PropertyType == typeof(long?)) {
                                            long lng_value = 0;
                                            if (long.TryParse(column_text_value, out lng_value)) {
                                                value_parsed = true;
                                            } else {
                                                if (StrictDataType || column_text_value != String.Empty) {
                                                    error.Add(column.ColumnName, $"unable to convert '{column_text_value}' to number value");
                                                } else {
                                                    value_parsed = true;
                                                    warning.Add(column.ColumnName, $"'{column_text_value}' is not number value, set to default value {lng_value}");
                                                }
                                            }
                                            value = lng_value;
                                        } else if (property_info.PropertyType == typeof(decimal) || property_info.PropertyType == typeof(decimal?)) {
                                            decimal decimal_value = 0;
                                            if (decimal.TryParse(column_text_value, out decimal_value)) {
                                                value = decimal_value;
                                                value_parsed = true;
                                            } else {
                                                if (StrictDataType || column_text_value != String.Empty) {
                                                    error.Add(column.ColumnName, $"unable to convert '{column_text_value}' to number value");
                                                } else {
                                                    value_parsed = true;
                                                    warning.Add(column.ColumnName, $"'{column_text_value}' is not number value, set to default value {decimal_value}");
                                                }
                                            }
                                        } else if (property_info.PropertyType == typeof(double) || property_info.PropertyType == typeof(double?)) {
                                            double double_value = 0;
                                            if (double.TryParse(column_text_value, out double_value)) {
                                                value = double_value;
                                                value_parsed = true;
                                            } else {
                                                if (StrictDataType || column_text_value != String.Empty) {
                                                    error.Add(column.ColumnName, $"unable to convert '{column_text_value}' to number value");
                                                } else {
                                                    value_parsed = true;
                                                    warning.Add(column.ColumnName, $"'{column_text_value}' is not number value, set to default value {double_value}");
                                                }
                                            }
                                        } else {
                                            value = column_text_value;
                                            value_parsed = true;
                                        }

                                        if (value_parsed) {
                                            property_info.SetValue(obj_instance, value);
                                        }
                                    }
                                }
                            }
                            if (warning.HasErrors) {
                                this.warnings.Add(warning);
                            }
                            if (!error.HasErrors) {
                                results.Add(obj_instance);
                            } else {
                                this.errors.Add(error);
                            }
                        }
                    }
                }
            }

            return this.Errors.Count() == 0;
        }

        private bool ValidateDataSource() {
            return true;
        }
    }
}
