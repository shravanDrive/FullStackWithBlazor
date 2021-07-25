// <copyright file="HttpGlobalExceptionFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services.Common.ExceptionFilter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// HttpGlobalExceptionFilter class for handling exceptions.
    /// </summary>
    public sealed class HttpGlobalExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGlobalExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.logger = logger;
            this.LoadExceptionTypeMappings();
        }

        public IDictionary<Type, HttpStatusCode> ExceptionTypeMappings
        {
            get;
            private set;
        }

        /// <summary>
        /// Overrides on exception method when an exception is returned.
        /// </summary>
        /// <param name="context">context.</param>
        public override void OnException(ExceptionContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ProblemDetails problemDetails;
            if (this.ExceptionTypeMappings.ContainsKey(context.Exception.GetType()))
            {
                var httpStatusCode = this.ExceptionTypeMappings[context.Exception.GetType()];
                problemDetails = this.CreateProblemDetails(context.Exception.Message, httpStatusCode);
            }
            else
            {
                problemDetails = this.CreateProblemDetails(context.Exception.Message, HttpStatusCode.InternalServerError);
            }

            context.HttpContext.Response.ContentType = "application/problem+json";
            context.HttpContext.Response.StatusCode = problemDetails.Status.Value;
            context.ExceptionHandled = true;
            context.Result = new JsonResult(problemDetails);

            this.logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
        }

        /// <summary>
        /// Creation of a problem detail.
        /// </summary>
        /// <param name="title">title.</param>
        /// <param name="status">status.</param>
        /// <param name="details">details.</param>
        /// <returns>ProblemDetails.</returns>
        private ProblemDetails CreateProblemDetails(string title, HttpStatusCode status, string details = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
            }

            return new ProblemDetails
            {
                Title = title,
                Status = (int)status,
                Detail = details,
            };
        }

        /// <summary>
        /// loads the Exception Type Mappings.
        /// </summary>
        private void LoadExceptionTypeMappings()
        {
            this.ExceptionTypeMappings = new Dictionary<Type, HttpStatusCode>
            {
                { typeof(NotImplementedException), HttpStatusCode.NotImplemented },
                { typeof(ValidationException), HttpStatusCode.BadRequest },
                { typeof(InvalidCastException), HttpStatusCode.BadRequest },
                { typeof(InvalidOperationException), HttpStatusCode.BadRequest },
            };
        }
    }
}
