// <copyright file="UpperRepositories.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DapperOperation.Repositories
{
    using DapperOperation.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// UpperRepositories
    /// </summary>
    public class UpperRepositories : BaseRepositories<SampleTable>
    {
        /// <summary>
        /// Table
        /// </summary>
        public override string Table => "SampleTable";

        /// <summary>
        /// UpdateQuery
        /// </summary>
        public string UpdateQuery
        {
            get
            {
                return @"UPDATE [dbo].[SampleTable]
                SET
                Msg = '{0}'
                WHERE Id = '{1}';";
            }
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override SampleTable Map(dynamic result)
        {
            SampleTable sampleTable = new SampleTable();
            sampleTable.Id = result.Id;
            sampleTable.Msg = result.Msg;
            return sampleTable;
        }

        /// <summary>
        /// insertQuery
        /// </summary>
        public string InsertQuery
        {
            get
            {
                return @"INSERT INTO [dbo].[SampleTable]
                (
                [Id],
                [Msg])
                OUTPUT INSERTED.ID
                VALUES
                (
                @Id
                ,@Msg);";
            }
        }

        public List<string> GetSqlInBatches(List<SampleTable> userNames)
        {
            var insertsql = @"INSERT INTO [dbo].[SampleTable]
                            (
                            [Id],
                            [Msg])
                            VALUES";
            var valuessql = "('{0}','{1}')";
            var batchSize = 1000;

            var sqlToExecute = new List<string>();
            var numberofbatches = (int)Math.Ceiling((double)userNames.Count / batchSize);

            for (int i = 0; i < numberofbatches; i++)
            {
                var userToinsert = userNames.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = userToinsert.Select(
                    item => string.Format(
                        valuessql,
                        item.Id,
                        item.Msg));
                sqlToExecute.Add(insertsql + string.Join(",", valuesToInsert));
            }

            return sqlToExecute;
        }

        /// <summary>
        /// UpdateEntry
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns></returns>
        public string UpdateEntry(SampleTable userNames)
        {
            return string.Format(this.UpdateQuery, userNames.Msg, userNames.Id);
        }
    }
}
