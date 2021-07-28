// <copyright file="BlazorService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BlazorUI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using BlazorUI.Constants;
    using Common.Models.DataTransferModels;
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// BlazorService
    /// </summary>
    public class BlazorService : IBlazorService
    {
        /// <summary>
        /// httpClient
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// BlazorService
        /// </summary>
        /// <param name="httpClient"></param>
        public BlazorService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<bool> TrialMethod()
        {
            int orderId = 23;
            string appendUrl = string.Format(StaticConstants.ApiCall, orderId); // string.Format(StaticConstants.ApiCall, parameter);
            var result = await this.httpClient.GetStringAsync(appendUrl).ConfigureAwait(false);
            await Task.Delay(1).ConfigureAwait(false);
            return true;
        }
    }
}
