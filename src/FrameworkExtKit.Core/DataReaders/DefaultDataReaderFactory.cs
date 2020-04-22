using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Core.DataReaders {
    public class DefaultDataReaderFactory : IDataReaderFactory {
        public IDataReader<T> GetDataReader<T>(string source, DataReaderType type) {
            IDataReader<T> reader;

            switch (type) {
                case DataReaderType.Excel: reader = new ExcelDataReader<T>(); break;
                case DataReaderType.Csv:
                case DataReaderType.Database:
                case DataReaderType.Text:
                default:
                    throw new NotImplementedException($"data reader for {source} is not implemented");
            }

            if(reader != null) {
                reader.Source = source;
            }

            return reader;
        }
    }
}
