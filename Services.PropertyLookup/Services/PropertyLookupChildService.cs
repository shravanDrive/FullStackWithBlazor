using Common.Repositories.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.PropertyLookup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PropertyLookup.Services
{
    public class PropertyLookupChildService: IPropertyLookupService
    {
        private readonly ILogger<PropertyLookupChildService> logger;

        /// <summary>
        /// The Http Client Factory
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Repository implementation
        /// </summary>
        private IRepositoryBase<SampleTable> repositoryBase;

        private IRepositoryBase<MyModel> repositoryMyModel;

        private IRepositoryBase<OpParameterModel> opModel;

        public PropertyLookupChildService(ILogger<PropertyLookupChildService> logger, IConfiguration configuration, IRepositoryBase<SampleTable> repositoryBase, IRepositoryBase<MyModel> repositoryMyModel, IRepositoryBase<OpParameterModel> opModel)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.repositoryBase = repositoryBase;
            this.repositoryMyModel = repositoryMyModel;
            this.opModel = opModel;
        }

        public async void trySomething()
        {
            await this.repositoryMyModel.GetByConditionAsync(item => item.ID == 1).ConfigureAwait(false);
        }

        public async Task<SampleTable> GetData()
        {
            await Task.Delay(5).ConfigureAwait(false);
            return new SampleTable();
        }
    }
}
