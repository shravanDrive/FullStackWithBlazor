// <copyright file="CacheScheme.cs" company="PlaceholderCompany">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Connectors.RedisCache
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// CacheScheme Enum.
    /// </summary>
    public enum CacheScheme
    {
        /// <summary>
        /// None value
        /// </summary>
        None = 0,

        /// <summary>
        /// Authorization Value
        /// </summary>
        Authorization = 1,

        /// <summary>
        /// MacroKey Value
        /// </summary>
        MacroKey = 2,

        /// <summary>
        /// Keys Value
        /// </summary>
        Keys = 3,

        /// <summary>
        /// FileName Value
        /// </summary>
        FileName = 4,

        /// <summary>
        /// Order Value
        /// </summary>
        Order = 5,

        /// <summary>
        /// SearchItem Value
        /// </summary>
        SearchItem = 6,
    }
}
