namespace BusinessLogic.Results
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public TValue? Value => _value;

        protected Result(TValue? value, bool isSuccess, IEnumerable<Error> errors)
            : base(isSuccess, errors)
        {
            _value = value;
        }

        public static Result<TValue> Success(TValue value) => new(value, true, Enumerable.Empty<Error>());

        public new static Result<TValue> Failure(Error error) => new(default, false, new List<Error> { error });

        public new static Result<TValue> Failure(IEnumerable<Error> errors) => new(default, false, errors);
    }
}
