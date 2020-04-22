using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {
    public class DirectoryPropertyAttribute : Attribute {
        public string SchemaAttributeName { get; protected set; }
        public DirectoryPropertyAttribute(string propertyName) {
            this.SchemaAttributeName = propertyName;
        }
    }
}
