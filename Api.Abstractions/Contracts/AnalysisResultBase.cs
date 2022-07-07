using System.Collections.Generic;

namespace Api.Abstractions.Contracts
{
    public class AnalysisResultBase
    {
        //public AnalysisResultBase(string message)
        //{
        //    IsSuccess = !message.IsNullOrWhiteSpace();
        //    Message = message;
        //}

        //public AnalysisResultBase(string message, string failureReason)
        //{
        //    IsSuccess = !message.IsNullOrWhiteSpace();
        //    Message = message;
        //    FailureReasons.Add(failureReason);
        //}

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> FailureReasons { get; } = new List<string>();
    }
}
