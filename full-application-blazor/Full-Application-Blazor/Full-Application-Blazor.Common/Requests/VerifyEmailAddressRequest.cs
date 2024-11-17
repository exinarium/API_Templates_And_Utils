using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;
using Full_Application_Blazor.Utils.Helpers.Constants;

namespace Full_Application_Blazor.Common.Requests
{
    [ExcludeFromCodeCoverage]
    public class VerifyEmailAddressRequest : IBaseAPIModel
	{
        /// <summary>
        /// The email address of the user
        /// </summary>
        [JsonPropertyName("email")]
		[Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.EMAIL_REQUIRED_ERROR)]
		[EmailAddress(ErrorMessage = RequestsConstants.VALID_EMAIL_ERROR)]
		public string EmailAddress { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        [JsonPropertyName("token")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.TOKEN_REQUIRED_ERROR)]
		public string Token { get; set; }
    }
}