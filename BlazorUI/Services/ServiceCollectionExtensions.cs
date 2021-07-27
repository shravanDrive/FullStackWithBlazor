// <copyright file="ServiceCollectionExtensions.cs" company="SELF">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace BlazorUI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BlazorUI.Models;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
        {
            return services.AddScoped(typeof(DragDropService<>));
        }
    }
}
