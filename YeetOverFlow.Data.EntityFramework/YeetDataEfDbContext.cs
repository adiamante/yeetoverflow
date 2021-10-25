using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeetOverFlow.Core;
using YeetOverFlow.Core.EntityFramework;

namespace YeetOverFlow.Data.EntityFramework
{
    public class YeetDataEfDbContext : YeetEfDbContext<YeetDataSet, YeetData>
    {
        public DbSet<YeetTable> Tables { get; set; }
        public DbSet<YeetColumnCollection> ColumnCollections { get; set; }
        public DbSet<YeetRowCollection> RowCollections { get; set; }
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
            modelBuilder.ApplyConfiguration(new YeetTableEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetColumnCollectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetRowCollectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetDataEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetColumnEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetRowEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetCellEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetDataGenericEntityConfiguration<YeetBooleanCell>());
            modelBuilder.ApplyConfiguration(new YeetDataGenericEntityConfiguration<YeetStringCell>());
            modelBuilder.ApplyConfiguration(new YeetDataGenericEntityConfiguration<YeetIntCell>());
            modelBuilder.ApplyConfiguration(new YeetDataGenericEntityConfiguration<YeetDoubleCell>());
            modelBuilder.ApplyConfiguration(new YeetDataGenericEntityConfiguration<YeetDateTimeCell>());
        }
    }

    internal class YeetDataGenericEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : YeetItem
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(typeof(T).Name);
        }
    }

    internal class YeetTableEntityConfiguration : IEntityTypeConfiguration<YeetTable>
    {
        public void Configure(EntityTypeBuilder<YeetTable> builder)
        {
            builder.ToTable(nameof(YeetTable));
            builder.HasOne(t => t.Columns)
                .WithOne()
                .HasForeignKey<YeetTable>("ColumnCollectionGuid")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(t => t.Rows)
                .WithOne()
                .HasForeignKey<YeetTable>("RowCollectionGuid")
                .OnDelete(DeleteBehavior.Cascade);

            //https://dotnetcoretutorials.com/2021/03/07/eager-load-navigation-properties-by-default-in-ef-core/
            builder.Navigation(t => t.Columns).AutoInclude();
            builder.Navigation(t => t.Rows).AutoInclude();
        }
    }

    internal class YeetColumnCollectionEntityConfiguration : IEntityTypeConfiguration<YeetColumnCollection>
    {
        public void Configure(EntityTypeBuilder<YeetColumnCollection> builder)
        {
            builder.ToTable(nameof(YeetColumnCollection));
            builder.Navigation(t => t.Children).AutoInclude();
        }
    }

    internal class YeetRowCollectionEntityConfiguration : IEntityTypeConfiguration<YeetRowCollection>
    {
        public void Configure(EntityTypeBuilder<YeetRowCollection> builder)
        {
            builder.ToTable(nameof(YeetRowCollection));
            builder.Navigation(t => t.Children).AutoInclude();
        }
    }

    internal class YeetDataEntityConfiguration : IEntityTypeConfiguration<YeetData>
    {
        public void Configure(EntityTypeBuilder<YeetData> builder)
        {
            builder.ToTable(nameof(YeetData));
            builder.Property(itm => itm.Key).HasField("_key");
        }
    }

    internal class YeetColumnEntityConfiguration : IEntityTypeConfiguration<YeetColumn>
    {
        public void Configure(EntityTypeBuilder<YeetColumn> builder)
        {
            builder.ToTable(nameof(YeetColumn));
        }
    }

    internal class YeetRowEntityConfiguration : IEntityTypeConfiguration<YeetRow>
    {
        public void Configure(EntityTypeBuilder<YeetRow> builder)
        {
            builder.ToTable(nameof(YeetRow));
            builder.HasMany(r => r.Children);
            builder.Navigation(t => t.Children).AutoInclude();
        }
    }

    internal class YeetCellEntityConfiguration : IEntityTypeConfiguration<YeetCell>
    {
        public void Configure(EntityTypeBuilder<YeetCell> builder)
        {
            builder.ToTable(nameof(YeetCell));;
        }
    }
}
