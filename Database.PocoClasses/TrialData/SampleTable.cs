using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.PocoClasses.TrialData
{
    public partial class SampleTable
    {
        [Column("id")]
        public int? Id { get; set; }
        [StringLength(10)]
        public string Msg { get; set; }
    }
}
