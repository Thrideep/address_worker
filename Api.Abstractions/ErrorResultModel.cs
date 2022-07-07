using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Abstractions
{
    public class ErrorResultModel
    {
        public ErrorResultModel(string code, string message, IEnumerable<Error> errors)
        {
            Code = code;
            Message = message;
            Errors = errors;
        }

        public ErrorResultModel(string message, IEnumerable<Error> errors) : this(null, message, errors)
        {
        }

        public ErrorResultModel(string code, string message, Error error) : this(code, message, new[] { error })
        {
        }

        public ErrorResultModel(string code, string message, string errorFieldId, string errorMessage) : this(code, message, new[] { new Error(errorFieldId, errorMessage) })
        {
        }

        public ErrorResultModel(string code, string message, string errorMessage) : this(code, message, new[] { new Error(errorMessage) })
        {
        }

        public ErrorResultModel(string code, string message) : this(code, message, Enumerable.Empty<Error>())
        {
        }

        public string Code { get; }
        public string Message { get; }
        public IEnumerable<Error> Errors { get; }
    }

    public class Error
    {
        public Error(string message) : this(null, message)
        {

        }

        public Error(string field, string message)
        {
            Message = message;
            Field = field != string.Empty ? field : null;
        }

        public string Message { get; }
        public string Field { get; }
    }
}
