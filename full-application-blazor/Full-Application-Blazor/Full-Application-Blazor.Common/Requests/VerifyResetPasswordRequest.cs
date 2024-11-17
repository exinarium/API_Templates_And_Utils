using Full_Application_Blazor.Utils.Helpers.Constants;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Full_Application_Blazor.Common.Requests
{
    [ExcludeFromCodeCoverage]
	public class VerifyResetPasswordRequest
	{
        /// <summary>
        /// The email address of the user
        /// </summary>
        [JsonPropertyName("email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.EMAIL_REQUIRED_ERROR)]
        [EmailAddress(ErrorMessage = RequestsConstants.VALID_EMAIL_ERROR)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The current password of the user
        /// </summary>
        [JsonPropertyName("currentPassword")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.CURRENT_PASSWORD_REQUIRED_ERROR)]
        [MinLength(8, ErrorMessage = RequestsConstants.CURRENT_PASSWORD_LENGTH_ERROR)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\W]{8,}$", ErrorMessage = RequestsConstants.CURRENT_PASSWORD_REGEX_ERROR)]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The new password of the user
        /// </summary>
        [JsonPropertyName("newPassword")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.NEW_PASSWORD_REQUIRED_ERROR)]
        [MinLength(8, ErrorMessage = RequestsConstants.NEW_PASSWORD_LENGTH_ERROR)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\W]{8,}$", ErrorMessage = RequestsConstants.NEW_PASSWORD_REGEX_ERROR)]
        public string NewPassword { get; set; }

        /// <summary>
        /// The reset password verification token
        /// </summary>
        [JsonPropertyName("token")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.TOKEN_REQUIRED_ERROR)]
        public string Token { get; set; }
    }
}

