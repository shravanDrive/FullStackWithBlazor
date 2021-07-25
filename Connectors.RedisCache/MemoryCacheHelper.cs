// <copyright file="MemoryCacheHelper.cs" company="SELF">
// Copyrght (c) SELF. All rights reserved.
// </copyright>

namespace Connectors.RedisCache
{
    using System;
    using System.Runtime.Caching;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// MemoryCacheHelper
    /// </summary>
    public class MemoryCacheHelper : ICacheHelper
    {
        /// <summary>
        /// ReadValueAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ReadValueAsync<T>(CacheScheme schema, string key)
        {
            MemoryCache memorycache = MemoryCache.Default;

            T result = default(T);
            string json = memorycache.Get(key)?.ToString();
            if (!string.IsNullOrWhiteSpace(json))
            {
                result = Deserialize<T>(json);
            }

            return await Task.FromResult(result).ConfigureAwait(false);
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
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(key, Serialize(value), DateTimeOffset.UtcNow.AddMinutes(expiryTimeoutMinutes));
            await Task.CompletedTask.ConfigureAwait(false);
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
        /// Deserialize json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueString"></param>
        /// <returns></returns>
        private static T Deserialize<T>(string valueString)
        {
            return JsonConvert.DeserializeObject<T>(valueString);
        }
    }
}
