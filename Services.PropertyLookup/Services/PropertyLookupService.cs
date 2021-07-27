using Common.Repositories.DataAccess;
using Connector.SignalR;
using Connectors.RedisCache;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.PropertyLookup.Models;
using Services.PropertyLookup.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PropertyLookup.Services
{
    public class PropertyLookupService : IPropertyLookupService
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger<PropertyLookupService> logger;

        /// <summary>
        /// The Http Client Factory
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Repository implementation
        /// </summary>
        private IRepositoryBase<SampleTable> repositoryBase;

        /// <summary>
        /// WorkEventHubContext
        /// </summary>
        private IHubContext<WorkEventHub> workEventHubContext;

        /// <summary>
        /// repositoryMyModel
        /// </summary>
        private IRepositoryBase<MyModel> repositoryMyModel;

        /// <summary>
        /// opModel
        /// </summary>
        private IRepositoryBase<OpParameterModel> opModel;

        /// <summary>
        /// Cache
        /// </summary>
        private ICacheHelper Cache;

        /// <summary>
        /// PropertyLookupService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryBase"></param>
        /// <param name="repositoryMyModel"></param>
        /// <param name="opModel"></param>
        /// <param name="Cache"></param>
        /// <param name="workEventHubContext"></param>
        public PropertyLookupService(ILogger<PropertyLookupService> logger, IConfiguration configuration, IRepositoryBase<SampleTable> repositoryBase, IRepositoryBase<MyModel> repositoryMyModel, IRepositoryBase<OpParameterModel> opModel, ICacheHelper Cache, IHubContext<WorkEventHub> workEventHubContext)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.repositoryBase = repositoryBase;
            this.repositoryMyModel = repositoryMyModel;
            this.opModel = opModel;
            this.Cache = Cache;
            this.workEventHubContext = workEventHubContext;
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <returns></returns>
        public async Task<SampleTable> GetData()
        {
            var output = new SqlParameter("@product_count", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
            var opmodel = await this.opModel.ExecSp($"EXEC uspFindProductByModel @product_count = {output} out").ConfigureAwait(false);
            var qwerydata = await this.repositoryMyModel.ExecSp($"EXEC SelectAllCustomers").ConfigureAwait(false);

                //return await this.repositoryBase.GetByIdAsync(1).ConfigureAwait(false);
                var data = await this.repositoryBase.GetByConditionAsync(item => item.Id == 1).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        /// <summary>
        /// CacheTrial
        /// </summary>
        /// <returns></returns>
        public async Task<string> CacheTrial()
        {
            SampleTable sampleTable = new SampleTable();
            
            sampleTable.Id = 1;
            sampleTable.Msg = "caching";

            string uniqueKey = Guid.NewGuid().ToString();
            await this.Cache.SetValueAsync<SampleTable>(CacheScheme.Order, uniqueKey, sampleTable, 30).ConfigureAwait(false);

            SampleTable sampleCachetable = await this.Cache.ReadValueAsync<SampleTable>(CacheScheme.Order, uniqueKey).ConfigureAwait(false);
            string returnValue = JsonConvert.SerializeObject(sampleCachetable);
            return returnValue;
        }

        /// <summary>
        /// SignalR
        /// </summary>
        /// <returns></returns>
        public async Task SignalR()
        {
            await this.workEventHubContext.Clients.All.SendAsync("InvokeMethod").ConfigureAwait(false);
        }
    }
}
