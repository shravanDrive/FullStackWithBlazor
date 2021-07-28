// <copyright file="CustomAuthorizationMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BlazorUI.MiddleWare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// CustomAuthorizationMessageHandler
    /// </summary>
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// THROTTLING_HTTP_STATUS_CODE
        /// </summary>
        private const int THROTTLINGHTTPSTATUSCODE = 429;

        /// <summary>
        /// httpContextHelper
        /// </summary>
        // private readonly IHttpContextHelper httpContextHelper;

    }
}
