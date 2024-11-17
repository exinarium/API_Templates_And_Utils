using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;
using Full_Application_Blazor.Utils.Helpers.Constants;

namespace Full_Application_Blazor.Common.Requests
{
    [ExcludeFromCodeCoverage]
    public class SignupRequest : IBaseAPIModel
    {
        /// <summary>
        /// The email address of the user
        /// </summary>
        [JsonPropertyName("email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.EMAIL_REQUIRED_ERROR)]
        [EmailAddress(ErrorMessage = RequestsConstants.VALID_EMAIL_ERROR)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The telephone number of the user
        /// </summary>
        [JsonPropertyName("phoneNumber")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.PHONE_NUMBER_REQUIRED_ERROR)]
        [Phone(ErrorMessage = RequestsConstants.PHONE_NUMBER_INVALID_ERROR)]
        [RegularExpression("^\\+[\\d]{11,11}$", ErrorMessage = RequestsConstants.PHONE_NUMBER_REGEX_ERROR)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        [JsonPropertyName("firstName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.FIRSTNAME_REQUIRED_ERROR)]
        [MinLength(3, ErrorMessage = RequestsConstants.FIRSTNAME_LENGTH_ERROR)]
        [RegularExpression("^[A-Z]{1,1}[a-z]{2,}$", ErrorMessage = RequestsConstants.FIRSTNAME_REGEX_ERROR)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        [JsonPropertyName("lastName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.LASTNAME_REQUIRED_ERROR)]
        [MinLength(3, ErrorMessage = RequestsConstants.LASTNAME_LENGTH_ERROR)]
        [RegularExpression("^[A-Z]{1,1}[a-z]{2,}$", ErrorMessage = RequestsConstants.LASTNAME_REGEX_ERROR)]
        public string LastName { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        [JsonPropertyName("password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = RequestsConstants.PASSWORD_REQUIRED_ERROR)]
        [MinLength(8, ErrorMessage = RequestsConstants.PASSWORD_LENGTH_ERROR)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\W]{8,}$", ErrorMessage = RequestsConstants.PASSWORD_REGEX_ERROR)]
        public string Password { get; set; }
    }
}