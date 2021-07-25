using Services.PropertyLookup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PropertyLookup.Services
{
    public interface IPropertyLookupService
    {
        /// <summary>
        /// GetData
        /// </summary>
        /// <returns></returns>
        Task<SampleTable> GetData();
    }
}
