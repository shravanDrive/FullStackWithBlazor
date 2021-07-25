// <copyright file="ConfigurationErrorException.cs" company="SELF">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// ConfigurationErrorException
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ConfigurationErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of ConfigurationErrorException
        /// </summary>
        public ConfigurationErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ConfigurationErrorException
        /// </summary>
        /// <param name="message"></param>
        public ConfigurationErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of ConfigurationErrorException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ConfigurationErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
