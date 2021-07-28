using Common.Models.DataTransferModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.PropertyLookup.Models;
using Services.PropertyLookup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PropertyLookup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyLookupController : ControllerBase
    {
        /// <summary>
        /// propertyLookupService
        /// </summary>
        private readonly IPropertyLookupService propertyLookupService;

        /// <summary>
        /// Intializes a new instance of property Lookup Controller
        /// </summary>
        /// <param name="propertyLookupService"></param>
        public PropertyLookupController(IPropertyLookupService propertyLookupService)
        {
            this.propertyLookupService = propertyLookupService;
        }


        /// <summary>
        /// GetDataById
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDataById")]
        public async Task<SampleTable> GetDataById()
        {
            return await this.propertyLookupService.GetData().ConfigureAwait(false);
        }

        /// <summary>
        /// ParameterMethod
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ParameterMethod")]
        public async Task<string> ParameterMethod(int orderId)
        {
            await Task.Delay(1).ConfigureAwait(false);
            InterApiData interApiData = new InterApiData();
            interApiData.Data = "Shravan";
            var data = JsonConvert.SerializeObject(interApiData);
            return data;
        }

        /// <summary>
        /// Trying different types of concepts via this calling method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Trial")]
        public async Task ConceptTrial()
        {
            
            await Task.Delay(1).ConfigureAwait(false);

            //Subtract concepts = new Concepts();
            //concepts.Display();

            Concepts concepts = new Concepts();
            concepts.CheckingExceptionFilter();

            concepts.VirtualWithAbstract();
        }

        /// <summary>
        /// Trying Caching works or not
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Caching")]
        public async Task<string> CacheTrial()
        {
           return await this.propertyLookupService.CacheTrial();
        }

        /// <summary>
        /// TryingSignalR
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TryingSignalR")]
        public async Task TryingSignalR()
        {
            await this.propertyLookupService.SignalR().ConfigureAwait(false);
        }
    }
}
