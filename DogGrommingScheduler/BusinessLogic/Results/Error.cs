namespace BusinessLogic.Results
{
    public record Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static readonly Error NotFound = new("Error.NotFound", "The requested resource was not found.");
        public static readonly Error Validation = new("Error.Validation", "One or more validations failed.");
        public static readonly Error Forbidden = new("Error.Forbidden", "You do not have the necessary permissions for this action.");
        public static readonly Error Unauthorized = new("Error.Unauthorized", "You are not authorized to perform this operation.");
        public static readonly Error Unexpected = new("Error.Unexpected", "An unexpected error occurred.");
        public static readonly Error Conflict = new("Error.Conflict", "The operation conflicts with the current state of the resource.");
    }
}
