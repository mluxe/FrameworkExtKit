using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Core.DataReaders {


    public interface IDataReader<T> {
        DataSourceErrorCollection Errors { get; }
        DataSourceErrorCollection Warnings { get; }

        bool StrictDataType { get; set; }
        IEnumerable<T> Data { get; }
        long TotalRecords { get; }
        string Source { get; set; }
        bool ReadData();
    }
}
