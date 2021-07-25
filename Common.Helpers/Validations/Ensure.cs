// <copyright file="Ensure.cs" company="SELF">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Common.Helpers.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Ensure Validation Helper
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// ArgumentNotNull Validations
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="parameter"></param>
        public static void ArgumentNotNull([ValidatedNotNull] object argument, string parameter)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameter);
            }
        }

        /// <summary>
        /// NotNullOrEmpty Validations
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="parameter"></param>
        public static void NotNullOrEmpty([ValidatedNotNull] string argument, string parameter)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException($"'{parameter}' cannot be null or empty", parameter);
            }
        }

        /// <summary>
        /// ArgumentNotNull Validations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="parameter"></param>
        public static void ArgumentNotNull<T>([ValidatedNotNull] object argument, string parameter)
            where T : Exception
        {
            if ((argument is string @string && string.IsNullOrWhiteSpace(@string)) || argument == null)
            {
                throw (T)Activator.CreateInstance(typeof(T), parameter);
            }
        }
    }
}
