// See https://aka.ms/new-console-template for more information

using ROP;

Console.WriteLine("Hello, ROP!");

Customer customer = new Customer();
customer.Name = "";
customer.Age = 10;

var result = CustomerValidation.ValidateName(customer).Then(CustomerValidation.ValidateAge);
Console.WriteLine(result.Success  is null ? result.Failure : result.Success);