using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices.Exceptions {
    public class DirectoryServiceException : Exception{
        public DirectoryServiceException(string message) : base(message) {

        }
    }
}
