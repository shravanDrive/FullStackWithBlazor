// <copyright file="RedisCacheHelper.cs" company="PlaceholderCompany">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Connectors.RedisCache
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Configuration;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    /// <summary>
    /// RedisCacheHelper
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
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
        /// RedisCacheHelper Pushing Code
        /// </summary>
        /// <param name="config"></param>
        public RedisCacheHelper(IConfiguration config)
        {
            connectionString = config.GetConfigValue<string>("RedisCache");
        }

        /// <summary>
        /// Gets the Connection
        /// </summary>
        private static ConnectionMultiplexer Connection => LazyConnection.Value;

        /// <summary>
        /// ReadValueAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ReadValueAsync<T>(CacheScheme schema, string key)
        {
            T result = default(T);
            string json = await ReadValueAsync(schema, key).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(json))
            {
                result = Deserialize<T>(json);
            }

            return result;
        }

        /// <summary>
        /// SetValueAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiryTimeoutMinutes"></param>
        /// <returns></returns>
        public async Task SetValueAsync<T>(CacheScheme schema, string key, T value, int expiryTimeoutMinutes)
        {
            string finalKey = BuildKey(schema, key);
            IDatabase cache = Connection.GetDatabase();
            await cache.StringSetAsync(finalKey, Serialize(value), new TimeSpan(0, 0, expiryTimeoutMinutes, 0)).ConfigureAwait(false);
        }

        /// <summary>
        /// ReadValueAync
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static async Task<string> ReadValueAsync(CacheScheme schema, string key)
        {
            // builds up a proper key and gets a reference to the correct Redis database.
            IDatabase cache = Connection.GetDatabase();

            key = schema + ":" + key;

            RedisValue value = await cache.StringGetAsync(key).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value;
        }

        /// <summary>
        /// Deserialize Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueString"></param>
        /// <returns></returns>
        private static T Deserialize<T>(string valueString)
        {
            return JsonConvert.DeserializeObject<T>(valueString);
        }

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// BuildKey
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string BuildKey(CacheScheme schema, string key)
        {
            return schema + ":" + key;
        }
    }
}
