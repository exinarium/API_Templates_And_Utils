using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;

namespace Full_Application_Blazor.Domain.Aggregates
{
	public interface IVerifyAggregate
	{
		Task SendVerificationEmail(ClaimsPrincipal user);
		Task<VerificationResponse> VerifyEmailAddress(VerifyEmailAddressRequest request, ClaimsPrincipal user);
        Task SendVerificationPhoneNumber(ClaimsPrincipal user);
        Task<VerificationResponse> VerifyPhoneNumber(VerifyPhoneNumberRequest request, ClaimsPrincipal user);
    }
}

