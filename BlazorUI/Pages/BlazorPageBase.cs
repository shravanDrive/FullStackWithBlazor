// <copyright file="Class.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BlazorUI.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Blazored.Toast.Services;
    using BlazorUI.Services;
    using Connectors.RedisCache;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    /// <summary>
    /// Class
    /// </summary>
    public class BlazorPageBase : ComponentBase
    {
        /// <summary>
        /// MacoRef
        /// </summary>
        [Parameter]
        public Blazorise.Modal MacoRef { get; set; }

        /// <summary>
        /// NavigationManager
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// ToastService
        /// </summary>
        [Inject]
        public IToastService ToastService { get; set; }

        /// <summary>
        /// BlazorService
        /// </summary>
        [Inject]
        public IBlazorService BlazorService { get; set; }

        /// <summary>
        /// JSRuntime
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public ICacheHelper Cache { get; set; }

        /// <summary>
        /// ButtonClick
        /// </summary>
        /// <returns>Task</returns>
        public async Task ButtonClick()
        {
            await this.BlazorService.TrialMethod().ConfigureAwait(false);
        }

    }
}
