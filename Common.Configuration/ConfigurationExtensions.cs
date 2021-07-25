// <copyright file="ConfigurationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Common.Configuration
{
    using System;
    using System.Globalization;
    using Common.Helpers.Validations;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// ConfigurationExtensions
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static T GetConfigValue<T>(this IConfiguration configuration, string key)
        {
            Ensure.ArgumentNotNull(configuration, nameof(configuration));
            bool sectionExist = configuration.GetSection(key).Exists();

            if (!sectionExist)
            {
                throw new ConfigurationErrorException($"Configuration key {key} not found");
            }

            T value = configuration.GetValue<T>(key);

            return value;
        }

        /// <summary>
        /// GetConfigValueOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetConfigValueOrDefault<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            Ensure.ArgumentNotNull(configuration, nameof(configuration));
            bool sectionExist = configuration.GetSection(key).Exists();

            T value = configuration.GetValue<T>(key);

            if (!sectionExist)
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// GetEnvironmentType
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetEnvironmentType(this IConfiguration configuration)
        {
            Ensure.ArgumentNotNull(configuration, nameof(configuration));
            string env = Environment.GetEnvironmentVariable("ASPNET_ENVIRONMENT");

            if (string.IsNullOrEmpty(env))
            {
                env = "local";
            }

            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            env = textinfo.ToTitleCase(env);
            return env;
        }
    }
}
