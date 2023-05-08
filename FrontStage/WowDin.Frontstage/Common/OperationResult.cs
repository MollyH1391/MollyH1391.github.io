using Microsoft.Extensions.Logging;
using System;

namespace WowDin.Frontstage.Common
{
    public class OperationResult
    {

        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public OperationResult() : this(false, "")
        {
        }

        public OperationResult(bool isSuccess, string message)
        {
            IsSuccessful = isSuccess;
            Message = message;
        }
    }

    
}
