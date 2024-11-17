using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;
using Full_Application_Blazor.Utils.Helpers.Constants;

namespace Full_Application_Blazor.Common.Requests
{
    [ExcludeFromCodeCoverage]
    public class VerifyPhoneNumberRequest : IBaseAPIModel
	{
        /// <summary>
        /// The phone number of the user
        /// </summary>
        [JsonPropertyName("phoneNumber")]
		[Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.PHONE_NUMBER_REQUIRED_ERROR)]
        [Phone(ErrorMessage = RequestsConstants.PHONE_NUMBER_INVALID_ERROR)]
        [RegularExpression("^\\+[\\d]{11,11}$", ErrorMessage = RequestsConstants.PHONE_NUMBER_REGEX_ERROR)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The phone number verification token
        /// </summary>
        [JsonPropertyName("token")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.TOKEN_REQUIRED_ERROR)]
		public string Token { get; set; }
    }
}