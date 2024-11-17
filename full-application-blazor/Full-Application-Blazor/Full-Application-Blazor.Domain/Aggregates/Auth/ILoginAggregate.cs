using System;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;

namespace Full_Application_Blazor.Domain.Aggregates
{
	public interface ILoginAggregate
	{
		Task<LoginResponse> Login(LoginRequest request);
	}
}

