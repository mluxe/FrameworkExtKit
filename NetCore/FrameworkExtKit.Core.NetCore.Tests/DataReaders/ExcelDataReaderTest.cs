using FrameworkExtKit.Core.DataReaders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrameworkExtKit.Core.NetCore.Tests.DataReaders {
    public class ExcelDataReaderTest {


        [Test]
        public void test_read_excel_should_success() {

            ExcelDataReader<InvoiceData> processor = new ExcelDataReader<InvoiceData>();
            processor.Source = this.getFilePath("invoice.xlsx");
            var process_result = processor.ReadData();

            Assert.IsTrue(process_result);
            Assert.AreEqual(5, processor.Data.Count());
            Assert.AreEqual(5, processor.TotalRecords);

            var first_row = processor.Data.First();

            Assert.AreEqual("Circuit #1", first_row.CircuitName);
            Assert.AreEqual("CNY", first_row.CurrencyCode);
            Assert.AreEqual("2019-08-01 00:00:00", first_row.BillingPeriod.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(360.33, first_row.TerminationCharge);
            Assert.AreEqual(4000.56m, first_row.AccessInstallationCharge);
            Assert.AreEqual(872.18m, Math.Round(first_row.VAT, 2));
        }

        [Test]
        public void test_read_excel_should_report_missing_column_error() {
            ExcelDataReader<InvoiceData> processor = new ExcelDataReader<InvoiceData>();
            processor.Source = this.getFilePath("invoice_missing_column.xlsx");
            var process_result = processor.ReadData();

            Assert.IsFalse(process_result);
            Assert.AreEqual(0, processor.Data.Count());
            Assert.AreEqual(1, processor.Errors.Count);

            Assert.AreEqual("column Circuit Name is not found", processor.Errors[0].GetError("Circuit Name").Message);
        }

        [Test]
        public void test_read_excel_should_report_data_errors() {
            ExcelDataReader<InvoiceData> processor = new ExcelDataReader<InvoiceData>();
            processor.Source = this.getFilePath("invoice_invalid_data.xlsx");
            var process_result = processor.ReadData();

            Assert.IsFalse(process_result);
            Assert.AreEqual(4, processor.Data.Count());
            Assert.AreEqual(1, processor.Errors.Count);

            var error = processor.Errors[0];

            Assert.AreEqual(3, error.Count);
            Assert.AreEqual("unable to convert 'today' to datetime value", error.GetError("Invoice From").Message);
            Assert.AreEqual("unable to convert 'tomorrow' to datetime value", error.GetError("Invoice To").Message);
            Assert.AreEqual("unable to convert '300 Million' to number value", error.GetError("Termination Charge").Message);
        }

        private string getFilePath(string fileName) {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Files", "ExcelDataReaderTest", fileName);
        }
    }
}
