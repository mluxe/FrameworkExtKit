using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FrameworkExtKit.Core.DocumentBuilder {

    internal class PropertyNotFoundException : Exception {
        public PropertyNotFoundException(string message) : base(message) {

        }
    }

    public partial class SimpleSpreadsheetDocumentBuilder
    {
        private enum CustomCellFormats {
            ISODateTimeFormat = 1,
            ISODateFormat
        };

        SpreadsheetDocument document;

        public string DateTimeFormat = "yyyy-mm-dd hh:mm:ss";
        public string DateFormat = "yyyy-mm-dd";

        public SimpleSpreadsheetDocumentBuilder(Stream stream) {
            document = SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            this.InitSpreadsheetDocument();
        }

        public SimpleSpreadsheetDocumentBuilder(string filePath) {
            document = SpreadsheetDocument.Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            this.InitSpreadsheetDocument();
        }

        private void InitSpreadsheetDocument() {
            WorkbookPart workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            workbookpart.Workbook.AppendChild<Sheets>(new Sheets());

            WorkbookStylesPart sp = document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            sp.Stylesheet = createStyleSheet();
            sp.Stylesheet.Save();
        }

        public void AddDataToSheet<T>(string sheetName, IEnumerable<T> data) {
            this.AddDataToSheet(sheetName, null, data);
        }

        public void AddDataToSheet(string sheetName, IEnumerable<string> columnNames, IEnumerable<Array> data) {

            SheetData sheetData = new SheetData();
            Worksheet worksheet = new Worksheet(sheetData);
            WorksheetPart worksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = worksheet;

            Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();

            // Append the new worksheet and associate it with the workbook.
            uint sheetId = (uint)sheets.Count() + 1;
            sheetName = String.IsNullOrEmpty(sheetName) ? "Sheet " + sheetId : sheetName;

            Sheet sheet = new Sheet() {
                Id = document.WorkbookPart
                    .GetIdOfPart(worksheetPart), SheetId = sheetId, Name = sheetName
            };

            Columns columns = new Columns();
            Row headerRow = new Row { RowIndex = 1 };
            List<string> keys = new List<string>();

            for (int i = 0; i < columnNames.Count(); i++) {
                var columnName = columnNames.ElementAt(i);
                Column column = new Column() {
                    Min = (uint)i, Max = (uint)i, CustomWidth = false
                };

                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(columnName);

                /*** set the columnWith does not work **/
                /*
                double columnWith = 8.43;

                columnWith = Math.Max(columnWith, 0.94 * (columnName.Length+1));
                column.Width = columnWith;
                */
                headerRow.Append(cell);
                columns.Append(column);
            }

            sheetData.Append(headerRow);

            for (int counter = 0; counter < data.Count(); counter++) {
                var rowData = data.ElementAt(counter);
                Row row = new Row { RowIndex = (uint)counter + 2 };

                for (int i = 0; i < rowData.Length; i++) {
                    var value = rowData.GetValue(i);
                    Cell cell = new Cell();
                    cell.DataType = GetCellDataType(value);
                    string cellValue = (value == null ? "" : value.ToString());
                    cell.CellValue = new CellValue(cellValue);

                    row.Append(cell);
                }
                sheetData.Append(row);
            }

            sheets.Append(sheet);
        }

        protected Dictionary<string, IEnumerable<PropertyInfo>> propertyInfos = new Dictionary<string, IEnumerable<PropertyInfo>>();

        protected IEnumerable<PropertyInfo> getPropertyInfo(Type type) {
            if (this.propertyInfos.ContainsKey(type.FullName)) {
                return this.propertyInfos[type.FullName];
            }
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            this.propertyInfos.Add(type.FullName, properties);
            return properties;

        }
        protected Dictionary<string, string> getPropertyFieldMappings(Type type) {
            var propertyInfos = this.getPropertyInfo(type);
            Dictionary<string, string> mappings = new Dictionary<string, string>();
            foreach (var info in propertyInfos) {
                if (info.PropertyType.IsClass
                    && !info.PropertyType.FullName.StartsWith("System.")) {
                    var inner_property_mappings = this.getPropertyFieldMappings(info.PropertyType);
                    foreach( var pair in inner_property_mappings) {
                        mappings.Add($"{info.Name}.{pair.Key}", $"{info.Name}_{pair.Key}");
                    }
                } else {
                    mappings.Add(info.Name, info.Name);
                }
            }
            return mappings;
        }

        protected Object getPropertyValue(Object obj, string[] propertyChain) {

            Object result = null;
            Object value = obj;

            for(var i=0; i< propertyChain.Length; i++) {
                var property_name = propertyChain[i];
                var type = value.GetType();
                var propertyInfos = this.getPropertyInfo(type);
                PropertyInfo info = propertyInfos.Where(x => x.Name == property_name).FirstOrDefault();

                if(info == null) {
                    throw new PropertyNotFoundException($"<{String.Join(".", propertyChain)} does not exist, break at {property_name}>");
                }
                value = info.GetValue(value, BindingFlags.Instance, null, null, null);
                if(value == null) {
                    break;
                }
            }

            result = value;
            return result;
        }

        public void AddDataToSheet<T>(string sheetName, Dictionary<string, string> fieldMapping, IEnumerable<T> data) {

            SheetData sheetData = new SheetData();
            Worksheet worksheet = new Worksheet(sheetData); 
            WorksheetPart worksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = worksheet;

            Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();

            // Append the new worksheet and associate it with the workbook.
            uint sheetId = (uint)sheets.Count() + 1;
            sheetName = String.IsNullOrEmpty(sheetName) ? "Sheet " + sheetId : sheetName;

            if (sheetName.Length > 30) {
                sheetName = sheetName.Substring(0, 30);
            }

            Sheet sheet = new Sheet() {
                Id = document.WorkbookPart
                    .GetIdOfPart(worksheetPart), SheetId = sheetId, Name = sheetName
            };

            Type type = null;
            IEnumerable<PropertyInfo> propertyInfos = new List<PropertyInfo>();

            if (data.Count() > 0) {
                var obj = data.First();
                type = obj.GetType();
                propertyInfos = this.getPropertyInfo(type);

                // if field mapping is null
                // then we want to export all properties
                if (fieldMapping == null) {
                    fieldMapping = this.getPropertyFieldMappings(type);
                }
            }

            Columns columns = new Columns();
            Row headerRow = new Row { RowIndex = 1 };
            List<string> keys = new List<string>();

            if (fieldMapping != null) {
                keys = fieldMapping.Keys.ToList();
                for (uint i = 0; i < keys.Count; i++) {
                    Column column = new Column() {
                        Min = i, Max = i, CustomWidth = false
                    };
                    columns.Append(column);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(fieldMapping[keys[(int)i]]);
                    headerRow.Append(cell);
                }
            }

            //worksheet.InsertAt(columns, 0);
            sheetData.Append(headerRow);

            for (uint counter = 0; counter < data.Count(); counter++) {
                var obj = data.ElementAt((int)counter);
                Row row = new Row { RowIndex = counter+2 };

                for (int i = 0; i < keys.Count; i++) {
                    string key = keys[i];
                    Cell cell = new Cell();
                    try {
                        var value = getPropertyValue(obj, key.Split('.'));
                        CellValues cellDataType = GetCellDataType(value);
                        cell.DataType = cellDataType;

                        if (cellDataType == CellValues.Date) {
                            var d = (DateTime)value;
                            cell.DataType = CellValues.Number;
                            if (d.TimeOfDay.Seconds == 0) {
                                cell.StyleIndex = UInt32Value.FromUInt32((int)CustomCellFormats.ISODateFormat);
                            } else {
                                cell.StyleIndex = UInt32Value.FromUInt32((int)CustomCellFormats.ISODateTimeFormat);
                            }
                            cell.CellValue = new CellValue(Convert.ToString(d.ToOADate()));
                        } else if (cellDataType == CellValues.Boolean) {
                            cell.DataType = CellValues.Number;
                            cell.CellValue = new CellValue(Convert.ToString(value));
                        } else {
                            cell.CellValue = new CellValue(Convert.ToString(value));
                        }
                    } catch(PropertyNotFoundException ex) {
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(ex.Message);
                    }

                    row.Append(cell);
                }
                sheetData.Append(row);
            }

            sheets.Append(sheet);
        }


        public void Close() {
            document.Close();
        }

        public SpreadsheetDocument SpreadsheetDocument { get { return document; } }

        private CellValues GetCellDataType(Object obj) {

            CellValues dataType = CellValues.String;

            if (obj != null) {
                string name = obj.GetType().Name;
                switch (name) {
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Single":
                    case "Double":
                    case "Decimal":
                        dataType = CellValues.Number;
                        break;
                    // excel will pop up "excel found unreadable content" error
                    // if we use CellValues.Boolean data type for Boolean types
                    // not sure how to avoid this error so we use String type at the moment
                    //dataType = CellValues.Boolean;
                    //break;
                    case "DateTime":
                    case "Date":
                    case "Time":
                    // excel will pop up "excel found unreadable content" error
                    // if we use CellValues.Date dat type for Date/DateTime/Time types
                    // not sure how to avoid this error so we use String type at the moment
                        dataType = CellValues.Date;
                        break;
                    case "Boolean":
                        dataType = CellValues.Boolean;
                        break;
                    case "String":
                    default:
                        dataType = CellValues.String;
                        break;
                }
            }
            
            return dataType;
        }

        private Stylesheet createStyleSheet() {
            Stylesheet stylesheet = new Stylesheet();

            Fonts fts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = StringValue.FromString("Calibri");
            FontSize ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(11);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            ftn = new FontName();
            ftn.Val = StringValue.FromString("Palatino Linotype");
            ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(18);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            fts.Count = UInt32Value.FromUInt32((uint)fts.ChildElements.Count);

            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Solid;
            patternFill.ForegroundColor = new ForegroundColor();
            patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00ff9728");
            patternFill.BackgroundColor = new BackgroundColor();
            patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);

            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);

            border = new Border();
            border.LeftBorder = new LeftBorder();
            border.LeftBorder.Style = BorderStyleValues.Thin;


            CellStyleFormats cellStyleFormats = new CellStyleFormats();

            CellFormat cellFormat = new CellFormat();
            cellFormat.NumberFormatId = 0;
            cellFormat.FontId = 0;
            cellFormat.FillId = 0;
            cellFormat.BorderId = 0;
            cellStyleFormats.Append(cellFormat);
            cellStyleFormats.Count = UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);

            uint iExcelIndex = 164;
            NumberingFormats numberFormats = new NumberingFormats();
            CellFormats cellFormats = new CellFormats();

            cellFormat = new CellFormat();
            cellFormat.NumberFormatId = 0;
            cellFormat.FontId = 0;
            cellFormat.FillId = 0;
            cellFormat.BorderId = 0;
            cellFormat.FormatId = 0;
            cellFormats.Append(cellFormat);

            NumberingFormat nfDateTime = new NumberingFormat();
            nfDateTime.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDateTime.FormatCode = StringValue.FromString(this.DateTimeFormat);
            numberFormats.Append(nfDateTime);

            NumberingFormat nfDate = new NumberingFormat();
            nfDate.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDate.FormatCode = StringValue.FromString(this.DateFormat);
            numberFormats.Append(nfDate);

            // index 1
            cellFormat = new CellFormat();
            cellFormat.NumberFormatId = nfDateTime.NumberFormatId;
            cellFormat.FontId = 0;
            cellFormat.FillId = 0;
            cellFormat.BorderId = 0;
            cellFormat.FormatId = 0;
            cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cellFormats.Append(cellFormat);

            cellFormat = new CellFormat();
            cellFormat.NumberFormatId = nfDate.NumberFormatId;
            cellFormat.FontId = 0;
            cellFormat.FillId = 0;
            cellFormat.BorderId = 0;
            cellFormat.FormatId = 0;
            cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cellFormats.Append(cellFormat);

            numberFormats.Count = UInt32Value.FromUInt32((uint)numberFormats.ChildElements.Count);
            cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);

            stylesheet.Append(numberFormats);
            stylesheet.Append(fts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);

            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle();
            cs.Name = StringValue.FromString("Normal");
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
            stylesheet.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            stylesheet.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9");
            tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16");
            stylesheet.Append(tss);

            return stylesheet;
        }
    }

    
}
