using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YeetOverFlow.Core.EntityFramework;

namespace YeetOverFlow.Settings.EntityFramework
{
    //https://stackoverflow.com/questions/41829229/how-do-i-implement-dbcontext-inheritance-for-multiple-databases-in-ef7-net-co
    //Use protected constructor in base class to send DbContextOptions<YeetMediaEFDbContext>
    public class YeetSettingsEfDbContext : YeetEfDbContext<YeetSettingList, YeetSetting>
    {
        public DbSet<YeetSettingBoolean> YeetSettingBooleans { get; set; }
        public DbSet<YeetSettingString> YeetSettingStrings { get; set; }
        public DbSet<YeetSettingStringOption> YeetSettingStringOptions { get; set; }

        public YeetSettingsEfDbContext(DbContextOptions<YeetSettingsEfDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new YeetSettingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetSettingBooleanEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetSettingStringEntityConfiguration());
            modelBuilder.ApplyConfiguration(new YeetSettingOptionEntityConfiguration<YeetSettingStringOption, String>());
        }
    }

    internal class YeetSettingEntityConfiguration : IEntityTypeConfiguration<YeetSetting>
    {
        public void Configure(EntityTypeBuilder<YeetSetting> builder)
        {
            Func<Enum, string> iconToString = i =>
            {
                var str = JsonConvert.SerializeObject(new
                {
                    type = i.GetType().AssemblyQualifiedName,
                    value = i.ToString()
                });
                return str;
            };

            Func<string, Enum> stringToIcon = i =>
            {
                JObject jIcon = JObject.Parse(i);
                Type iconType = Type.GetType(jIcon["type"].ToString());
                return (Enum)Enum.Parse(iconType, jIcon["value"].ToString());
            };

            builder.ToTable(nameof(YeetSetting));
            builder.Property(itm => itm.Key).HasField("_key");
            builder.Property(itm => itm.Icon).HasConversion(
                i => iconToString(i), i => stringToIcon(i)
            );
            builder.Property(itm => itm.Icon2).HasConversion(
                i => iconToString(i), i => stringToIcon(i)
            );
        }
    }

    internal class YeetSettingBooleanEntityConfiguration : IEntityTypeConfiguration<YeetSettingBoolean>
    {
        public void Configure(EntityTypeBuilder<YeetSettingBoolean> builder)
        {
            builder.ToTable(nameof(YeetSettingBoolean));
        }
    }

    internal class YeetSettingStringEntityConfiguration : IEntityTypeConfiguration<YeetSettingString>
    {
        public void Configure(EntityTypeBuilder<YeetSettingString> builder)
        {
            builder.ToTable(nameof(YeetSettingString));
        }
    }

    internal class YeetSettingOptionEntityConfiguration<TDerived, T> : IEntityTypeConfiguration<TDerived>
        where TDerived : YeetSettingOption<T>
    {
        public void Configure(EntityTypeBuilder<TDerived> builder)
        {
            builder.ToTable(typeof(TDerived).Name);
            builder.Property(opt => opt.Options)
                .HasConversion(
                    opts => JsonConvert.SerializeObject(opts),
                    opts => JsonConvert.DeserializeObject<T[]>(opts));
        }
    }
}
