using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Reflection;
using YeetOverFlow.Wpf.Commands;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetCommandManagerViewModel : YeetItemViewModelBase
    {
        #region Private Members
        YeetObservableList<YeetCommandViewModel> _commandHistory = new YeetObservableList<YeetCommandViewModel>();
        YeetObservableList<YeetCommandViewModel> _undoHistory = new YeetObservableList<YeetCommandViewModel>();
        System.Windows.Input.ICommand _undoCommand, _redoCommand;
        ICommandDispatcher _commandDispatcher;
        Boolean _isFrozen = false;
        bool _isOpen;
        ILogger _logger;
        IMapper _mapper;
        #endregion Private Members

        #region Initialization
        public YeetCommandManagerViewModel(ICommandDispatcher commandDispatcher, ILogger<YeetCommandManagerViewModel> logger)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
            ICollectionView col = CollectionViewSource.GetDefaultView(_undoHistory.Children);
            col.SortDescriptions.Add(new SortDescription("Sequence", ListSortDirection.Descending));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PropertyChangedEventArgs, YeetCommandViewModel>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetCommandViewModel, PropertyChangedEventArgs>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetPropertyChangedCommandViewModel, PropertyChangedExtendedEventArgs>()
                    .ReverseMap();

                cfg.CreateMap<YeetCollectionPropertyChangedCommandViewModel, CollectionPropertyChangedEventArgs>()
                    .ReverseMap();

                cfg.CreateMap<YeetMultiPropertyChangedCommandViewModel, MultiPropertyChangedExtendedEventArgs>()
                    .ReverseMap();
            });

            _mapper = mapperConfig.CreateMapper();
        }
        #endregion Initialization

        #region Properties
        #region CommandHistory
        public YeetObservableList<YeetCommandViewModel> CommandHistory
        {
            get { return _commandHistory; }
            set { SetValue(ref _commandHistory, value); }
        }
        #endregion CommandHistory
        #region UndoHistory
        public YeetObservableList<YeetCommandViewModel> UndoHistory
        {
            get { return _undoHistory; }
            set { SetValue(ref _undoHistory, value); }
        }
        #endregion UndoHistory
        #region UndoCommand
        public System.Windows.Input.ICommand UndoCommand
        {
            get
            {
                return _undoCommand ?? (_undoCommand =
                    new RelayCommand(() => Undo()
                ));
            }
        }
        #endregion UndoCommand
        #region RedoCommand
        public System.Windows.Input.ICommand RedoCommand
        {
            get
            {
                return _redoCommand ?? (_redoCommand =
                    new RelayCommand(() => Redo()
                ));
            }
        }
        #endregion RedoCommand
        #region IsFrozen
        public Boolean IsFrozen
        {
            get { return _isFrozen; }
            set { _isFrozen = value; }
        }
        #endregion IsFrozen
        #region IsOpen
        public bool IsOpen
        {
            get { return _isOpen; }
            set { SetValue(ref _isOpen, value); }
        }
        #endregion IsOpen
        #endregion Properties

        #region Methods
        public void Undo()
        {
            if (_commandHistory.Count > 0)
            {
                _isFrozen = true;
                YeetCommandViewModel cmd = _commandHistory[_commandHistory.Count - 1];
                _undoHistory.InsertChildAt(_undoHistory.Count, cmd);
                _commandHistory.RemoveChildAt(_commandHistory.Count - 1);
                cmd.Undo();
                _isFrozen = false;
                _logger.LogInformation($"Undo: {((YeetItem)cmd.Object).Name}.{cmd.PropertyName} {cmd.NewValueDisplay} => {cmd.OldValueDisplay}");
            }
        }

        public void Redo()
        {
            if (_undoHistory.Count > 0)
            {
                _isFrozen = true;
                YeetCommandViewModel cmd = _undoHistory[_undoHistory.Count - 1];
                _commandHistory.InsertChildAt(_commandHistory.Count, cmd);
                _undoHistory.RemoveChildAt(_undoHistory.Count - 1);
                cmd.Execute();
                _isFrozen = false;
                _logger.LogInformation($"Redo: {((YeetItem)cmd.Object).Name}.{cmd.PropertyName} {cmd.OldValueDisplay} => {cmd.NewValueDisplay}");
            }
        }

        public void Handle<T>(PropertyChangedEventArgs args, IMapper mapper, bool isChildArgs = false) where T : YeetItem
        {            
            var cmd = _mapper.Map<YeetCommandViewModel>(args);
            
            if (!isChildArgs)
            {
                AddCommand(cmd, typeof(T));
            }

            switch (args)
            {
                case MultiPropertyChangedExtendedEventArgs mpcArg:
                    foreach (var childArgs in mpcArg.ArgsList)
                    {
                        Handle<T>(childArgs, mapper, true);
                    }
                    break;
                case PropertyChangedExtendedEventArgs pcArgs:
                    var updates = new Dictionary<string, string>() { { pcArgs.PropertyName, pcArgs.NewValue?.ToString() } };
                    var updateCmd = new UpdateYeetItemCommand<T>(((YeetItem)pcArgs.Object).Guid, updates) { DeferCommit = true };
                    _commandDispatcher.Dispatch<UpdateYeetItemCommand<T>, Result>(updateCmd);
                    break;
                case CollectionPropertyChangedEventArgs cpcArgs:
                    switch (cpcArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Move:
                            foreach (YeetItem data in cpcArgs.NewItems)
                            {
                                var mvCmd = new MoveYeetItemCommand<T>(((YeetItem)cpcArgs.Object).Guid, ((YeetItem)cpcArgs.Object).Guid, data.Guid, cpcArgs.NewStartingIndex) { DeferCommit = true };
                                _commandDispatcher.Dispatch<MoveYeetItemCommand<T>, Result>(mvCmd);
                            }
                            break;
                        default:
                            if (cpcArgs.NewItems != null)
                            {
                                foreach (YeetItem data in cpcArgs.NewItems)
                                {
                                    var addCmd = new AddYeetItemCommand<T>(((YeetItem)cpcArgs.Object).Guid, mapper.Map<T>(data), data.Sequence) { DeferCommit = true };
                                    _commandDispatcher.Dispatch<AddYeetItemCommand<T>, Result>(addCmd);
                                }
                            }
                            if (cpcArgs.OldItems != null)
                            {
                                foreach (YeetItem data in cpcArgs.OldItems)
                                {
                                    var rmvCmd = new RemoveYeetItemCommand<T>(((YeetItem)cpcArgs.Object).Guid, data.Guid) { DeferCommit = true };
                                    _commandDispatcher.Dispatch<RemoveYeetItemCommand<T>, Result>(rmvCmd);
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        public void AddCommand(YeetCommandViewModel cmd, Type targetType)
        {
            if (!_isFrozen)
            {
                cmd.CommandDispatcher = _commandDispatcher;
                cmd.TargetType = targetType;
                if (cmd is YeetMultiPropertyChangedCommandViewModel multiCmd)
                {
                    foreach (var childCmd in multiCmd.ArgsList)
                    {
                        childCmd.CommandDispatcher = _commandDispatcher;
                        childCmd.TargetType = targetType;
                    }
                }
                _commandHistory.InsertChildAt(_commandHistory.Count, cmd);
            }
        }

        public void DispatchSave<T>() where T : YeetItem
        {
            var cmd = new SaveCommand<T>();
            _commandDispatcher.Dispatch<SaveCommand<T>, Result>(cmd);
            IsOpen = false;
        }
        #endregion Methods
    }

    #region YeetCommandViewModel
    public abstract class YeetCommandViewModel : YeetItemViewModelBase
    {
        public abstract String PropertyName { get; set; }
        public abstract object Object { get; set; }
        public String Display { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public abstract String NewValueDisplay { get; }
        public abstract String OldValueDisplay { get; }
        public Type TargetType { get; set; }
        public ICommandDispatcher CommandDispatcher { get; set; }
        public abstract void Execute();
        public abstract void Undo();
    }
    #endregion YeetCommandViewModel

    #region YeetPropertyChangedCommandViewModel
    public class YeetPropertyChangedCommandViewModel : YeetCommandViewModel
    {
        public override String PropertyName { get; set; }
        public override object Object { get; set; }
        public override string NewValueDisplay => NewValue?.ToString();
        public override string OldValueDisplay => OldValue?.ToString();

        public YeetPropertyChangedCommandViewModel()
        {
        }

        public override void Execute()
        {
            if (PropertyName.StartsWith('_'))
            {
                ReflectionHelper.FieldInfoCollection[Object.GetType()][PropertyName].SetValue(Object, NewValue);

                var updates = new Dictionary<string, string>() { { PropertyName, NewValue?.ToString() } };
                var genericType = typeof(UpdateYeetItemCommand<>);
                var cmdType = genericType.MakeGenericType(new[] { TargetType });
                var updateCmd = Activator.CreateInstance(cmdType, ((YeetItem)Object).Guid, updates, true);

                typeof(ICommandDispatcher)
                    .GetMethod(nameof(ICommandDispatcher.Dispatch))
                    .MakeGenericMethod(new[] { cmdType, typeof(Result) })
                    .Invoke(CommandDispatcher, new[] { updateCmd });
            }
            else
            {
                ReflectionHelper.PropertyInfoCollection[Object.GetType()][PropertyName].SetValue(Object, NewValue);
            }
        }

        public override void Undo()
        {
            if (PropertyName.StartsWith('_'))
            {
                ReflectionHelper.FieldInfoCollection[Object.GetType()][PropertyName].SetValue(Object, OldValue);

                var updates = new Dictionary<string, string>() { { PropertyName, OldValue?.ToString() } };
                var genericType = typeof(UpdateYeetItemCommand<>);
                var cmdType = genericType.MakeGenericType(new[] { TargetType });
                var updateCmd = Activator.CreateInstance(cmdType, ((YeetItem)Object).Guid, updates, true);

                typeof(ICommandDispatcher)
                    .GetMethod(nameof(ICommandDispatcher.Dispatch))
                    .MakeGenericMethod(new[] { cmdType, typeof(Result) })
                    .Invoke(CommandDispatcher, new[] { updateCmd });
            }
            else
            {
                ReflectionHelper.PropertyInfoCollection[Object.GetType()][PropertyName].SetValue(Object, OldValue);
            }
        }
    }
    #endregion YeetPropertyChangedCommandViewModel

    #region YeetMultiPropertyChangedCommandViewModel
    public class YeetMultiPropertyChangedCommandViewModel : YeetPropertyChangedCommandViewModel
    {
        public virtual List<YeetCommandViewModel> ArgsList { get; private set; }
        public YeetMultiPropertyChangedCommandViewModel()
        {
        }
        public override void Execute()
        {
            foreach (var cmd in ArgsList)
            {
                cmd.Execute();
            }
        }

        public override void Undo()
        {
            ArgsList.Reverse();
            foreach (var cmd in ArgsList)
            {
                cmd.Undo();
            }
            ArgsList.Reverse();
        }
    }
    #endregion YeetMultiPropertyChangedCommandViewModel

    #region YeetCollectionPropertyChangedCommandViewModel
    public class YeetCollectionPropertyChangedCommandViewModel : YeetCommandViewModel
    {
        public override String PropertyName { get; set; }
        public override object Object { get; set; }
        public NotifyCollectionChangedAction Action { get; set; }
        public IList OldItems { get; set; }
        public IList NewItems { get; set; }
        public override string NewValueDisplay => $"[{NewItems?.Count ?? 0}]";
        public override string OldValueDisplay => $"[{OldItems?.Count ?? 0}]";
        public virtual int NewStartingIndex { get; set; }
        public virtual int OldStartingIndex { get; set; }

        public YeetCollectionPropertyChangedCommandViewModel()
        {
        }

        public override void Execute()
        {
            switch (Action)
            {
                case NotifyCollectionChangedAction.Move:
                    foreach (var item in NewItems)
                    {
                        ReflectionHelper.MethodInfoCollection[Object.GetType()][nameof(IYeetListBase<YeetItem>.MoveChild)].Invoke(Object, new Object[] { NewStartingIndex, item });
                    }
                    break;
                default:
                    if (OldItems != null)
                    {
                        foreach (var item in OldItems)
                        {
                            ReflectionHelper.MethodInfoCollection[Object.GetType()][nameof(IYeetListBase<YeetItem>.RemoveChild)].Invoke(Object, new Object[] { item });
                        }
                    }

                    if (NewItems != null)
                    {
                        foreach (var item in NewItems)
                        {
                            ReflectionHelper.MethodInfoCollection[Object.GetType()][nameof(IYeetListBase<YeetItem>.AddChild)].Invoke(Object, new Object[] { item });
                        }
                    }
                    break;
            }
        }

        public override void Undo()
        {
            switch (Action)
            {
                case NotifyCollectionChangedAction.Move:
                    foreach (var item in NewItems)
                    {
                        ReflectionHelper.MethodInfoCollection[Object.GetType()][nameof(IYeetListBase<YeetItem>.MoveChild)].Invoke(Object, new Object[] { OldStartingIndex, item });
                    }
                    break;
                default:
                    if (OldItems != null)
                    {
                        foreach (var item in OldItems)
                        {
                            ReflectionHelper.MethodInfoCollection[Object.GetType()][nameof(IYeetListBase<YeetItem>.AddChild)].Invoke(Object, new Object[] { item });
                        }
                    }

                    if (NewItems != null)
                    {
                        foreach (var item in NewItems)
                        {
                            ReflectionHelper.MethodInfoCollection[Object.GetType()][nameof(IYeetListBase<YeetItem>.RemoveChild)].Invoke(Object, new Object[] { item });
                        }
                    }
                    break;
            }
        }
    }
    #endregion YeetCollectionPropertyChangedCommandViewModel
}
