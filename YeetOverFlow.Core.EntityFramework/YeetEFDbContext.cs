using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using YeetOverFlow.Strings;
using YeetOverFlow.Core.Application.Events;
using System.Collections.Generic;

namespace YeetOverFlow.Core.EntityFramework
{
    public abstract class YeetEFDbContext : YeetEfDbContext<YeetList, YeetItem>
    {
        public YeetEFDbContext(DbContextOptions<YeetEFDbContext> options) : base(options) { }
    }

    //https://github.com/dotnet/efcore/issues/23069
    //Migration problem with modelBuilder.UseIdentityColumns() was fixed by adding the NuGet package Microsoft.EntityFrameworkCore.SqlServer
    public class YeetEfDbContext<TParent, TChild> : DbContext
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public DbSet<YeetLibrary<TParent>> YeetLibraries { get; set; }
        public DbSet<TChild> YeetItems { get; set; }
        public DbSet<TParent> YeetLists { get; set; }
        //public DbSet<YeetEvent> Events { get; set; }
        public DbSet<YeetEvent<TChild>> Events { get; set; }
        public DbSet<YeetItemAddedEvent<TChild>> AddedEvents { get; set; }
        public DbSet<YeetItemMovedEvent<TChild>> MovedEvents { get; set; }
        public DbSet<YeetItemRemovedEvent<TChild>> RemovedEvents { get; set; }
        public DbSet<YeetItemUpdatedEvent<TChild>> UpdatedEvents { get; set; }
        public YeetEfDbContext() : base() { }

        public YeetEfDbContext(DbContextOptions<YeetEfDbContext<TParent, TChild>> options) : base(options) { }
        protected YeetEfDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new YeetLibraryEntityConfiguration<TParent, TChild>());
            modelBuilder.ApplyConfiguration(new YeetItemEntityConfiguration<TChild>());
            //modelBuilder.ApplyConfiguration(new YeetItemBaseEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new YeetEventEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetEventEntityConfiguration<TChild>());
            modelBuilder.ApplyConfiguration(new YeetListEntityConfiguration<TParent, TChild>());
            modelBuilder.ApplyConfiguration(new YeetItemAddedEventEntityConfiguration<TChild>());
            modelBuilder.ApplyConfiguration(new YeetItemRemovedEventEntityConfiguration<TChild>());
            modelBuilder.ApplyConfiguration(new YeetItemUpdatedEventEntityConfiguration<TChild>());
        }
    }

    //https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
    internal class YeetLibraryEntityConfiguration<TParent, TChild> : IEntityTypeConfiguration<YeetLibrary<TParent>>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<YeetLibrary<TParent>> builder)
        {
            builder.HasKey(lib => lib.Guid);
            builder.ToTable(StringHelper.Pluralize("YeetLibrary"));

            builder.HasOne(lib => lib.Root)
                .WithOne()
                .HasForeignKey<TParent>("RootGuid")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class YeetItemEntityConfiguration<TChild> : IEntityTypeConfiguration<TChild>
        where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<TChild> builder)
        {
            builder.HasKey(itm => itm.Guid);
            builder.Property(itm => itm.Guid).HasField("_guid");
            builder.ToTable(StringHelper.Pluralize(typeof(TChild).Name));
            builder.Property(itm => itm.Sequence).HasField("_sequence");
        }
    }

    //internal class YeetItemBaseEntityConfiguration : IEntityTypeConfiguration<YeetItemBase>
    //{
    //    public void Configure(EntityTypeBuilder<YeetItemBase> builder)
    //    {
    //        builder.HasKey(itm => itm.Guid);
    //        builder.ToTable(StringHelper.Pluralize(nameof(YeetItemBase)));
    //        builder.Property(itm => itm.Sequence).HasField("_sequence");
    //        //builder.Property<Guid?>("YeetListBaseGuid");
    //        //builder.HasOne<YeetListBase>()
    //        //       .WithMany(lst => lst.Children)
    //        //       .HasForeignKey("YeetListBaseGuid")
    //        //       .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    internal class YeetListEntityConfiguration<TParent, TChild> : IEntityTypeConfiguration<TParent>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<TParent> builder)
        {
            builder.ToTable(StringHelper.Pluralize(typeof(TParent).Name));

            //builder.Property(itm => itm.Children).HasField("_children");
            //builder.Property<Guid?>("YeetListBaseGuid");
            //builder.HasOne<YeetListBase>()
            //    .WithOne()
            //    .HasForeignKey<YeetListBase>("YeetListBaseGuid")
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }

    //internal class YeetEventEntityConfiguration : IEntityTypeConfiguration<YeetEvent>
    //{
    //    public void Configure(EntityTypeBuilder<YeetEvent> builder)
    //    {
    //        builder.HasKey(evnt => evnt.Guid);
    //    }
    //}


    internal class YeetEventEntityConfiguration<TChild> : IEntityTypeConfiguration<YeetEvent<TChild>>
        where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<YeetEvent<TChild>> builder)
        {
            builder.HasKey(evnt => evnt.Guid);
        }
    }

    internal class YeetItemAddedEventEntityConfiguration<TChild> : IEntityTypeConfiguration<YeetItemAddedEvent<TChild>>
        where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<YeetItemAddedEvent<TChild>> builder)
        {
            builder.Property(evnt => evnt.Child)
                .HasConversion(
                    child => JsonConvert.SerializeObject(child),
                    child => JsonConvert.DeserializeObject<TChild>(child));
        }
    }

    internal class YeetItemRemovedEventEntityConfiguration<TChild> : IEntityTypeConfiguration<YeetItemRemovedEvent<TChild>>
        where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<YeetItemRemovedEvent<TChild>> builder)
        {
            builder.Property(evnt => evnt.TargetChild)
                .HasConversion(
                    child => JsonConvert.SerializeObject(child),
                    child => JsonConvert.DeserializeObject<TChild>(child));
        }
    }

    internal class YeetItemUpdatedEventEntityConfiguration<TChild> : IEntityTypeConfiguration<YeetItemUpdatedEvent<TChild>>
    where TChild : YeetItem
    {
        public void Configure(EntityTypeBuilder<YeetItemUpdatedEvent<TChild>> builder)
        {
            builder.Property(evnt => evnt.Original)
                .HasConversion(
                    fields => JsonConvert.SerializeObject(fields),
                    fields => JsonConvert.DeserializeObject<IDictionary<string, string>>(fields));

            builder.Property(evnt => evnt.Updates)
                .HasConversion(
                    fields => JsonConvert.SerializeObject(fields),
                    fields => JsonConvert.DeserializeObject<IDictionary<string, string>>(fields));
        }
    }

}
