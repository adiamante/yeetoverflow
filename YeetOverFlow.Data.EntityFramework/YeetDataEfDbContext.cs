using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeetOverFlow.Core.EntityFramework;

namespace YeetOverFlow.Data.EntityFramework
{
    public class YeetDataEfDbContext : YeetEfDbContext<YeetDataSet, YeetData>
    {
        public DbSet<YeetTable> Tables { get; set; }
        public DbSet<YeetColumn> Columns { get; set; }
        public DbSet<YeetRow> Rows { get; set; }
        public DbSet<YeetCell> Cells { get; set; }
        public DbSet<YeetBooleanCell> BooleanCells { get; set; }
        public DbSet<YeetStringCell> StringCells { get; set; }
        public DbSet<YeetIntCell> IntCells { get; set; }
        public DbSet<YeetDoubleCell> DoubleCells { get; set; }
        public DbSet<YeetDateTimeCell> DateTimeCells { get; set; }

        public YeetDataEfDbContext(DbContextOptions<YeetDataEfDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new YeetDataEntityConfiguration());
        }
    }

    //internal class YeetDataSetEntityConfiguration : IEntityTypeConfiguration<YeetDataSet>
    //{
    //    public void Configure(EntityTypeBuilder<YeetDataSet> builder)
    //    {
    //        builder.ToTable(nameof(YeetDataSet));
    //    }
    //}

    internal class YeetDataEntityConfiguration : IEntityTypeConfiguration<YeetData>
    {
        public void Configure(EntityTypeBuilder<YeetData> builder)
        {
            builder.ToTable(nameof(YeetData));
            builder.Property(itm => itm.Key).HasField("_key");
        }
    }

}
