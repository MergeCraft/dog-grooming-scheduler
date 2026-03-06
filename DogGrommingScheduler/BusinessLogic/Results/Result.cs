using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, IEnumerable<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = (errors ?? Enumerable.Empty<Error>()).ToList().AsReadOnly();
        }

        public static Result Success() => new(true, Enumerable.Empty<Error>());

        public static Result Failure(Error error) => new(false, new List<Error> { error });

        public static Result Failure(IEnumerable<Error> errors) => new(false, errors);
    }
}
