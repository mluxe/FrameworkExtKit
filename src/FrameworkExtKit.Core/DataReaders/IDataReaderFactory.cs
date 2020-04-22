using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Core.DataReaders {

    public enum DataReaderType {
        Excel, Csv, SqlServer, Text, MySQL, Oracle, Database
    }

    public interface IDataReaderFactory {
        IDataReader<T> GetDataReader<T>(string source, DataReaderType type);
    }
}
