using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Settings;
using YeetOverFlow.Wpf.ViewModels;
using System.Collections.Generic;

namespace YeetOverFlow.Wpf.Events
{
    public class YeetOverFlowWpfEventStore : IYeetEventStore<YeetEvent<YeetItem>, YeetItem>, 
            IYeetEventStore<YeetEvent<YeetSetting>, YeetSetting>
    {
        YeetCommandManagerViewModel _commandManager;
        YeetSettingLibraryViewModel _settings;
        public YeetOverFlowWpfEventStore(YeetCommandManagerViewModel commandManager, YeetSettingLibraryViewModel settings)
        {
            _commandManager = commandManager;
            _settings = settings;
        }

        public void DispatchEvent(YeetEvent<YeetItem> domainEvent)
        {

        }

        public void DispatchEvent(YeetEvent<YeetSetting> domainEvent)
        {
            switch (domainEvent)
            {
                case YeetItemUpdatedEvent<YeetSetting> updatedEvent:
                    foreach (var update in updatedEvent.Updates)
                    {
                        var updateCmd = new YeetPropertyChangedCommand(
                            update.Key, 
                            _settings[updatedEvent.TargetGuid], 
                            updatedEvent.Original[update.Key], 
                            update.Value
                        );
                        _commandManager.AddCommand(updateCmd);
                    }
                    break;
                case YeetItemAddedEvent<YeetSetting> addedEvent:
                    var addCmd = new YeetCollectionPropertyChangedCommand(
                            "Children",
                            _settings[addedEvent.TargetListGuid],
                            null,
                            new List<YeetSettingViewModel>() { _settings[addedEvent.Child.Guid] }
                        );
                    _commandManager.AddCommand(addCmd);
                    break;
                case YeetItemRemovedEvent<YeetSetting> removedEvent:
                    var removeCmd = new YeetCollectionPropertyChangedCommand(
                            "Children",
                            _settings[removedEvent.TargetListGuid],
                            new List<YeetSettingViewModel>() { _settings[removedEvent.TargetChildGuid] },
                            null
                        );
                    _commandManager.AddCommand(removeCmd);
                    break;
            }
        }
    }
}
