﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Full_Application_Blazor.Common.Objects.Base;

namespace Full_Application_Blazor.Common.Responses
{
    [ExcludeFromCodeCoverage]
    public class VerificationResponse : IBaseAPIModel
	{
        /// <summary>
        /// Indication if verification was successful
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
