using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.PropertyLookup.Models
{
    [Table("SampleTable")]
    public partial class SampleTable
    {
        [Column("id")]
        public int? Id { get; set; }
        [StringLength(10)]
        public string Msg { get; set; }
    }
}
