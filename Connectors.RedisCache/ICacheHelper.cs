// <copyright file="ICacheHelper.cs" company="PlaceholderCompany">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Connectors.RedisCache
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ICacheHelper
    /// </summary>
    public interface ICacheHelper
    {
        /// <summary>
        /// ReadValueAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> ReadValueAsync<T>(CacheScheme schema, string key);

        /// <summary>
        /// SetValueAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiryTimeoutMinutes"></param>
        /// <returns></returns>
        Task SetValueAsync<T>(CacheScheme schema, string key, T value, int expiryTimeoutMinutes);
    }
}
