namespace ROP
{
    public interface IResult<TSuccess, TFailure>
    {
    }

    public class Result<TSuccess, TFailure> : IResult<TSuccess, TFailure>
    {
        public TSuccess Success { get; set; }

        public TFailure Failure { get; set; }

        public Result(TSuccess success)
        {
            Success = success;
            Failure = default;
        }

        public Result(TFailure failure)
        {
            Failure = failure;
            Success = default;
        }

        public void Deconstruct(out TSuccess? success, out TFailure? failure) { success = Success; failure = Failure; }
    }

    public static class ResultExtensions
    {
        public static Func<Result<TValue, TFailure>, Result<TSuccess, TFailure>> Bind<TValue, TSuccess, TFailure>(Func<TValue, Result<TSuccess, TFailure>> map)
        {
            return input =>
                {
                    var (success, failure) = input;
                    return (success, failure) switch
                    {
                        (_, null) => map(success),
                        (null, _) => new Result<TSuccess,TFailure>(failure),
                        _ => throw new NotImplementedException(),
                    };
                };
        }

        public static Result<TSuccess, TFailure> Then<TValue, TSuccess, TFailure>(this Result<TValue, TFailure> instance, Func<TValue, Result<TSuccess, TFailure>> map) => Bind(map)(instance);
    }

    public class Customer
    {
        public string? Name { get; set; }

        public int Age { get; set; }
    }

    public class CustomerResult : Result<Customer, Exception>
    {
        public CustomerResult(Customer success) : base(success) { }

        public CustomerResult(Exception error) : base(error) { }
    }

    public static class CustomerValidation
    {
        public static CustomerResult ValidateName(Customer customer) => !string.IsNullOrWhiteSpace(customer.Name) ? new CustomerResult(customer) : new CustomerResult(new InvalidDataException("Name cannot be empty"));

        public static CustomerResult ValidateAge(Customer customer) => customer.Age is > 0 and < 100 ? new CustomerResult(customer) : new CustomerResult(new InvalidDataException("Age Invalid"));
    }
}
