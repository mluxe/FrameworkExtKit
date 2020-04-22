using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.DirectoryAccountService.NetCore.Tests.Services.DirectoryServices {
    public class DirectoryPropertyAttribute : Attribute {
        public string SchemaAttributeName { get; protected set; }
        public DirectoryPropertyAttribute(string propertyName) {
            this.SchemaAttributeName = propertyName;
        }
    }
}
