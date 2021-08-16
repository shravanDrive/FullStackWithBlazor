// <copyright file="UserServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DapperOperation.Services
{
    using DapperOperation.Model;
    using DapperOperation.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// UserServices
    /// </summary>
    public class UserServices
    {
        /// <summary>
        /// upperRepositories
        /// </summary>
        private UpperRepositories upperRepositories;

        /// <summary>
        /// UserServices
        /// </summary>
        public UserServices()
        {
            this.upperRepositories = new UpperRepositories();
        }

        /// <summary>
        /// Insert
        /// </summary>
        public void Insert()
        {
            SampleTable data = new SampleTable();
            data.Id = 1;
            data.Msg = "sdsd";

            this.upperRepositories.Insert(data, this.upperRepositories.InsertQuery);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public List<SampleTable> Get()
        {
            // for specific id
            this.upperRepositories.GetAll().Select(item => item.Id == 1);
            return this.upperRepositories.GetAll().ToList();
        }

        /// <summary>
        /// UpdateEntry
        /// </summary>
        public void UpdateEntry()
        {
            SampleTable data = new SampleTable();
            data.Id = 1;
            data.Msg = "sdsd";
            this.upperRepositories.ExecuteQuery(this.upperRepositories.UpdateEntry(data));
        }

    }
}
