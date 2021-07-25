using Common.Repositories.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        private IRepositoryBase<MyModel> repositoryMyModel;

        private IRepositoryBase<OpParameterModel> opModel;

        /// <summary>
        /// Constructor for intantiation
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryBase"></param>
        public PropertyLookupService(ILogger<PropertyLookupService> logger, IConfiguration configuration, IRepositoryBase<SampleTable> repositoryBase, IRepositoryBase<MyModel> repositoryMyModel, IRepositoryBase<OpParameterModel> opModel)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.repositoryBase = repositoryBase;
            this.repositoryMyModel = repositoryMyModel;
            this.opModel = opModel;
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
    }
}
