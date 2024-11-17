using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;

namespace Full_Application_Blazor.Common.Responses
{
    [ExcludeFromCodeCoverage]
    public class LoginResponse : IBaseAPIModel
    {
        /// <summary>
        /// The authentication token for the user
        /// </summary>
        [JsonPropertyName("token")]
		public string Token { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Whether or not the user has a verified email address
        /// </summary>
        [JsonPropertyName("isEmailVerified")]
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Whether or not the user has a verified telephone number
        /// </summary>
        [JsonPropertyName("isTelephoneVerified")]
        public bool IsTelephoneVerified { get; set; }
    }
}

