using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FrameworkExtKit.Core.DataReaders {

    
    public class DataSourceErrorItem {
        public string Category { get; set; }
        public string Message { get; set; }

        public DataSourceErrorItem(string category, string message) {
            this.Category = category;
            this.Message = message;
        }
    }
   

    public class DataSourceError {
        public long RowId { get; set; }
        public ICollection<DataSourceErrorItem> Errors { get; set; }

        public bool HasErrors {
            get { return Errors.Count > 0; }
        }

        public int Count {
            get { return Errors.Count; }
        }

        public DataSourceError(int rowId) {
            RowId = rowId;
            Errors = new List<DataSourceErrorItem>();
        }

        public void Add(string category, string message) {
            this.Errors.Add(new DataSourceErrorItem(category, message));
        }

        public DataSourceErrorItem GetError(string category) {
            return this.Errors.Where(e => e.Category == category).FirstOrDefault();
        }

        public bool HasErrorCategory(string category) {
            return this.Errors.Any(e => e.Category == category);
        }

    }

    public class DataSourceErrorCollection : Collection<DataSourceError> {
   
        public bool HasErrors { get { return this.Count > 0; } }

    }
}
