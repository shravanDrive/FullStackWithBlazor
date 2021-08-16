// <copyright file="BaseRepositories.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DapperOperation.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;

    /// <summary>
    /// BaseRepositories
    /// </summary>
    public abstract class BaseRepositories<T>
    {
        /// <summary>
        /// query
        /// </summary>
        private string query = string.Empty;

        /// <summary>
        /// Connection
        /// </summary>
        public IDbConnection Connection
        {
            // ConfigurationManager.ConnectionStrings ["Server=DESKTOP-447IQGP\\SQLEXPRESS;Database=Sample;Trusted_Connection=True"].ConnectionString)
            get { return new SqlConnection("Server=DESKTOP-447IQGP\\SQLEXPRESS;Database=Sample;Trusted_Connection=True"); }
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public abstract T Map(dynamic result);

        /// <summary>
        /// Table
        /// </summary>
        public virtual string Table
        {
            get { return typeof(T).Name; }
        }

        /// <summary>
        /// QueryGetAll
        /// </summary>
        public virtual string QueryGetAll{ get; set; }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                var items = new List<T>();
                this.query = string.Format(this.QueryGetAll);

                using (IDbConnection dbConnection = this.Connection)
                {
                    dbConnection.Open();
                    var results = dbConnection.Query(this.query);
                    if (results != null && results.Count() > 0)
                    {
                        for (int i = 0; i < results.ToList().Count(); i++)
                        {
                            items.Add(this.Map(results.ElementAt(i)));
                        }
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="Table_Name"></param>
        /// <param name="insertQuery"></param>
        /// <returns></returns>
        public virtual bool Insert(T table_Name, string insertQuery)
        {
            using (IDbConnection dbConnection = this.Connection)
            {
                dbConnection.Open();
                var result = dbConnection.Query<int>(insertQuery, this.Map(table_Name));
                return result != null;
            }
        }

        /// <summary>
        /// BulkInsert
        /// </summary>
        /// <param name="insertQuerylist"></param>
        public void BulkInsert(List<string> insertQuerylist)
        {
            using (IDbConnection dbConnection = this.Connection)
            {
                foreach (var insertQuery in insertQuerylist)
                {
                    dbConnection.Open();
                    dbConnection.Execute(insertQuery);
                }
            }
        }

        public void ExecuteQuery(string query)
        {
            using (IDbConnection dbConnection = this.Connection)
            {
                dbConnection.Open();
                var result = dbConnection.Execute(query);
            }
        }

        /// <summary>
        /// ExecuteStoredProcedure
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> ExecuteStoredProcedure(string spName)
        {
            try
            {
                var items = new List<T>();
                using (IDbConnection dbConnection = this.Connection)
                {
                    var procedure = spName;
                    var results = dbConnection.Query(procedure, commandType: CommandType.StoredProcedure).ToList();
                    if (results != null && results.Count() > 0)
                    {
                        for (int i = 0; i < results.ToList().Count(); i++)
                        {
                            items.Add(this.Map(results.ElementAt(i)));
                        }
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


    }
}
