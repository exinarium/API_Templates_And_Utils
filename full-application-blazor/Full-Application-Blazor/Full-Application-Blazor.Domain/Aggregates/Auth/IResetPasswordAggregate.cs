using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;

namespace Full_Application_Blazor.Domain.Aggregates
{
	public interface IResetPasswordAggregate
	{
		Task SendResetPasswordEmail(ResetPasswordRequest request);
		Task<ResetPasswordResponse> VerifyResetPasswordToken(VerifyResetPasswordRequest request);
    }
}

