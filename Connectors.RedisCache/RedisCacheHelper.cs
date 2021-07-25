// <copyright file="RedisCacheHelper.cs" company="PlaceholderCompany">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Connectors.RedisCache
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using StackExchange.Redis;

    /// <summary>
    /// RedisCacheHelper
    /// </summary>
    public class RedisCacheHelper : ICacheHelper
    {

        /// <summary>
        /// Gets the lazy Connection. using Lazy initialization for the cache connection,
        /// being thread safe and created only the first time on access.
        /// </summary>
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(connectionString);
        });

        /// <summary>
        /// Gets the connectionString
        /// </summary>
        private static string connectionString;

        /// <summary>
        /// RedisCacheHelper
        /// </summary>
        /// <param name="config"></param>
        public RedisCacheHelper(IConfiguration config)
        {
            connectionString = config.GetConfigValue<string>("RedisCache");
        }

        public Task<T> ReadValueAsync<T>(CacheScheme schema, string key)
        {
            throw new NotImplementedException();
        }

        public Task SetValueAsync<T>(CacheScheme schema, string key, T value, int expiryTimeoutMinutes)
        {
            throw new NotImplementedException();
        }
    }
}
