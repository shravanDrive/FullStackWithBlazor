// <copyright file="SampleTable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DapperOperation.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// SampleTable
    /// </summary>
    public partial class SampleTable
    {
        [Column("id")]
        public int? Id { get; set; }

        [StringLength(10)]
        public string Msg { get; set; }
    }
}
