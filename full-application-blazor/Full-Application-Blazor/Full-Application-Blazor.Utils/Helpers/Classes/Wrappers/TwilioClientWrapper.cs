using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using System.Diagnostics.CodeAnalysis;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Twilio.Rest.Api.V2010.Account;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    [ExcludeFromCodeCoverage]
    public class TwilioClientWrapper : ITwilioWrapper
    {
        public TwilioClientWrapper()
        {

        }

        public async Task Init(string username, string password)
        {
            TwilioClient.Init(username, password);
            return;
        }

        public async Task Create(PhoneNumber from, string body, PhoneNumber to)
        {
            MessageResource.Create(
                from: from,
                body: body,
                to: to
                );
        }
    }
}
