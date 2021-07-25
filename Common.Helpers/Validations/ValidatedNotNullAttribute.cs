// <copyright file="ValidatedNotNullAttribute.cs" company="SELF">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace Common.Helpers.Validations
{
    using System;

    /// <summary>
    /// ValidatedNotNullAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
