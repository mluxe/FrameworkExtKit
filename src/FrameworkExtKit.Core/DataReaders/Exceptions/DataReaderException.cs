using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Core.DataReaders.Exceptions {
    public class DataReaderException : Exception {

        public int Row { get; set; }
        public string Value { get; set; }
        public string Step { get; set; }

        public DataReaderException(string step, int row, string value, string msg) : base(msg) {
            this.Row = row;
            this.Step = step;
            this.Value = value;
        }

        public DataReaderException(string step, int row, int value, string msg) : base(msg) {
            this.Row = row;
            this.Step = step;
            this.Value = value.ToString();
        }

        public DataReaderException(string step, int row, decimal value, string msg) : base(msg) {
            this.Row = row;
            this.Step = step;
            this.Value = value.ToString();
        }

        public DataReaderException(string step, int row, double value, string msg) : base(msg) {
            this.Row = row;
            this.Step = step;
            this.Value = value.ToString();
        }

        public DataReaderException(string step, int row, string msg) : base(msg) {
            this.Row = row;
            this.Step = step;
        }

        public DataReaderException(string step, string msg)
            : base(msg) {
            this.Step = step;
        }

        public override string Message {
            get {
                if (Row > 0) {
                    return String.Format("Step: {0}, Row: {1}, Error: {2}", Step, Row, base.Message);
                }
                return String.Format("Step: {0}, Error: {1}", Step, base.Message);
            }
        }
    }

}
