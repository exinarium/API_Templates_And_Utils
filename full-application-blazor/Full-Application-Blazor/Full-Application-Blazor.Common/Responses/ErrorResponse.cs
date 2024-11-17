using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;

namespace Full_Application_Blazor.Common.Responses
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse : IBaseAPIModel
    {
        /// <summary>
        /// The error message when an error occurred
        /// </summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }

        /// <summary>
        /// Additional error info, if any
        /// </summary>
		[JsonPropertyName("internalExceptionMessage")]
        public string? InternalExceptionMessage { get; set; }
	}
}

