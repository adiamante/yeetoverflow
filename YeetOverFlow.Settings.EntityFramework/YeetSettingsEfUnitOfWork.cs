using YeetOverFlow.Core;
using YeetOverFlow.Core.EntityFramework;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Settings.EntityFramework
{
    public class YeetSettingsEfUnitOfWork : YeetEfUnitOfWork<YeetSettingList, YeetSetting>, IYeetUnitOfWork<YeetSettingList, YeetSetting>
    {
        public YeetSettingsEfUnitOfWork(YeetEfDbContext<YeetSettingList, YeetSetting> context,
            IRepository<YeetLibrary<YeetSettingList>> libraries,
            IRepository<YeetSettingList> lists,
            IRepository<YeetSetting> items,
            IRepository<YeetEvent<YeetSetting>> events) : 
                base(context, libraries, lists, items, events)
        {
        }
    }
}
