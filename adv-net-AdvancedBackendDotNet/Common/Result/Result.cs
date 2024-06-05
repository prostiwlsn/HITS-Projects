using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    public class ResultBase
    {
        public string Message { get; set; }
    }

    public class ResultErrors : ResultBase
    {
        public IEnumerable<object> Errors { get; set; }
    }

    public class Result : ResultErrors
    {
        public ResultMessage ResultMessage { get; set; }
        public bool Success { get; set; } = true;
    }

    public class Result<TLoad> : Result
    {
        public TLoad? Load { get; set; }
    }

    public static class ResultExtensions
    {
        public static ResultBase CastToBase(this ResultBase result) => new ResultBase { Message = result.Message };

        public static ResultErrors CastToErrors(this ResultErrors result) => new ResultErrors { Errors = result.Errors, Message = result.Message };

        public static Result CastToResult(this Result result) => new Result { Errors = result.Errors, Message = result.Message, ResultMessage = result.ResultMessage, Success = result.Success };
    }
}
