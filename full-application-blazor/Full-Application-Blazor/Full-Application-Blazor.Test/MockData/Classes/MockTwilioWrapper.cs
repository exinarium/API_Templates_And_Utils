using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Twilio.Clients;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    public class MockTwilioWrapper : ITwilioWrapper
    {
        public bool isCreated;
        public MockTwilioWrapper() { }

        public Task Init(string username, string password)
        {
            return Task.CompletedTask;
        }

        public Task Create(PhoneNumber from, string body, PhoneNumber to) 
        {
            return Task.CompletedTask;
        }
    }
}
