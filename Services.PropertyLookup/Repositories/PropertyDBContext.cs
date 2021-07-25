using Common.Repositories.DataAccess;
using Microsoft.EntityFrameworkCore;
using Services.PropertyLookup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PropertyLookup.Repositories
{
    public class PropertyDBContext: BaseDbContext
    {
        /// <summary>
        /// PropertyDBContext
        /// </summary>
        /// <param name="options"></param>
        public PropertyDBContext(DbContextOptions<PropertyDBContext> options): base(options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        /// <summary>
        /// Gets or Sets SampleTable
        /// </summary>
        public virtual DbSet<SampleTable> SampleTable { get; set; }

        /// <summary>
        /// Gets or Sets MyModel
        /// </summary>
        public virtual DbSet<MyModel> MyModel { get; set; }

        /// <summary>
        /// Gets or Sets MyModel
        /// </summary>
        public virtual DbSet<OpParameterModel> OPModel { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleTable>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Msg).IsFixedLength();
            });

            modelBuilder.Entity<MyModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<OpParameterModel>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
