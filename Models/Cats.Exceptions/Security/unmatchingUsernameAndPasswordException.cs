﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace Cats.Models.Exceptions
{
    public class UnmatchingUsernameAndPasswordException : Exception, ISerializable
    {
        private const int exceptionCode = 1;
        private const string exceptionDetail = "User password and username do not match.";
        private const string exceptionTip = "Check the spelling, notice capitalization.";

        public UnmatchingUsernameAndPasswordException() { }
        public UnmatchingUsernameAndPasswordException(string message) { }
        public UnmatchingUsernameAndPasswordException(Exception innerExeption) { }
        public UnmatchingUsernameAndPasswordException(string message, Exception innerException) { }

        public string detail {get{return exceptionDetail;}}
        public string tip { get { return exceptionTip; } }
        
        public override string ToString()
        {
            StringBuilder message = new StringBuilder();
            message.Append(detail);
            message.Append(Environment.NewLine);
            message.Append(tip);
            return message.ToString();
        }
    }
}