using Full_Application_Blazor.Utils.Helpers.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Full_Application_Blazor.Common.Requests
{
    [ExcludeFromCodeCoverage]
    public class ResetPasswordRequest
	{
        /// <summary>
        /// The email address of the user
        /// </summary>
        [JsonPropertyName("email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.EMAIL_REQUIRED_ERROR)]
        [EmailAddress(ErrorMessage = RequestsConstants.VALID_EMAIL_ERROR)]
        public string EmailAddress { get; set; }
    }
}

