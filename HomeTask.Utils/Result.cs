using System.Diagnostics;

namespace HomeTask.Utils
{
    public class Result<T>
    {
        public int HttpStatusCode { get; set; }

        public string Message { get; set; }

        public T? Data { get; set; }

        /// <summary>
        ///     Creates a result instance
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="message"></param>
        public Result(int StatusCode, string message) 
        { 
            HttpStatusCode = StatusCode;
            Message = message;
        }
    }
}
