using Braintree;
using BraintreeSample.WebApi.Config;
using Microsoft.AspNetCore.Mvc;
using RandomNameGeneratorLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraintreeSample.WebApi.Controllers
{
  [Produces("application/json")]
  [Route("api/Checkouts")]
  public class CheckoutsController: ControllerBase
  {

    private readonly IBraintreeConfiguration _braintreeConfiguration;

    public CheckoutsController(IBraintreeConfiguration braintreeConfiguration)
    {
      _braintreeConfiguration = braintreeConfiguration;
    }

    public static readonly TransactionStatus[] transactionSuccessStatuses = {       TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };
    [HttpGet("ClientToken")]
    public async Task<IActionResult> ClientToken(string customerId = "")
    {
      var gateway = _braintreeConfiguration.CreateGateway();

      if (string.IsNullOrEmpty(customerId))
      {
        var personGenerator = new PersonNameGenerator();
        var firstname = personGenerator.GenerateRandomFirstName();
        var lastname = personGenerator.GenerateRandomLastName();

        var request = new CustomerRequest
        {
          FirstName = firstname,
          LastName = lastname,
          Company = $"{firstname} {lastname} Co.",
          Email = $"{firstname}.{lastname}@example.com",
          Fax = "419-555-1234",
          Phone = "614-555-1234",
          Website = $"http://{firstname}{lastname}.com",
        };
        Result<Customer> result = gateway.Customer.Create(request);

        bool success = result.IsSuccess();
        // true
        customerId = result.Target.Id;
      }

      var clientTokenRequest = new ClientTokenRequest();
      clientTokenRequest.CustomerId = customerId;

      var clientToken = await gateway.ClientToken.GenerateAsync(clientTokenRequest);

      var response = new Dictionary<string, string>
      {
        { "client_token", clientToken },
        { "customer_id", customerId },
      };

      return Ok(response);
    }


    [HttpGet("MakeTransction")]
    public IActionResult MakeTransction(decimal amount, string nonce, string customerId, string merchantAccountId = null, bool? threeDSecureRequired = null)
    {
      var request = new TransactionRequest
      {
        Amount = amount,
        PaymentMethodNonce = nonce,
        Options = new TransactionOptionsRequest
        {
          SubmitForSettlement = true,
        },
        //MerchantAccountId = merchantAccountId,
        CustomerId = customerId,
      };

      var gateway = _braintreeConfiguration.CreateGateway();

      Result<Transaction> result = gateway.Transaction.Sale(request);
      if (result.IsSuccess())
      {
        Transaction transaction = result.Target;
        return Ok(true);
      }
      else if (result.Transaction != null)
      {
        return Ok(true);
      }
      else
      {
        string errorMessages = "";
        foreach (ValidationError error in result.Errors.DeepAll())
        {
          errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
        }

        return Ok(false);
      }
    }
  }
}
