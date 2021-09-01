using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using YeetOverFlow.Core;
using YeetOverFlow.Reflection;
using YeetOverFlow.Wpf.Commands;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetCommandManagerViewModel : YeetItemViewModelBase
    {
        #region Private Members
        YeetObservableList<YeetCommand> _commandHistory = new YeetObservableList<YeetCommand>();
        YeetObservableList<YeetCommand> _undoHistory = new YeetObservableList<YeetCommand>();
        ICommand _undoCommand, _redoCommand;
        Boolean _isFrozen = false;
        bool _isOpen;
        ILogger _logger;
        #endregion Private Members

        #region Initialization
        public YeetCommandManagerViewModel(ILogger<YeetCommandManagerViewModel> logger)
        {
            _logger = logger;
            ICollectionView col = CollectionViewSource.GetDefaultView(_undoHistory.Children);
            col.SortDescriptions.Add(new SortDescription("Sequence", ListSortDirection.Descending));
        }
        #endregion Initialization

        #region Properties
        #region CommandHistory
        public YeetObservableList<YeetCommand> CommandHistory
        {
            get { return _commandHistory; }
            set { SetValue(ref _commandHistory, value); }
        }
        #endregion CommandHistory
        #region UndoHistory
        public YeetObservableList<YeetCommand> UndoHistory
        {
            get { return _undoHistory; }
            set { SetValue(ref _undoHistory, value); }
        }
        #endregion UndoHistory
        #region UndoCommand
        public ICommand UndoCommand
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
        public ICommand RedoCommand
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
                YeetCommand cmd = _commandHistory[_commandHistory.Count - 1];
                _undoHistory.InsertChildAt(_undoHistory.Count, cmd);
                _commandHistory.RemoveChildAt(_commandHistory.Count - 1);
                cmd.Undo();
                _isFrozen = false;
                _logger.LogInformation($"Undo: {((YeetItem)cmd.Object).Name}.{cmd.Property} {cmd.NewValueDisplay} => {cmd.OldValueDisplay}");
            }
        }

        public void Redo()
        {
            if (_undoHistory.Count > 0)
            {
                _isFrozen = true;
                YeetCommand cmd = _undoHistory[_undoHistory.Count - 1];
                _commandHistory.InsertChildAt(_commandHistory.Count, cmd);
                _undoHistory.RemoveChildAt(_undoHistory.Count - 1);
                cmd.Execute();
                _isFrozen = false;
                _logger.LogInformation($"Redo: {((YeetItem)cmd.Object).Name}.{cmd.Property} {cmd.OldValueDisplay} => {cmd.NewValueDisplay}");
            }
        }

        public void AddCommand(YeetCommand cmd)
        {
            if (!_isFrozen)
            {
                _commandHistory.InsertChildAt(_commandHistory.Count, cmd);
            }
        }
        #endregion Methods
    }

    #region YeetCommand
    public abstract class YeetCommand : YeetItemViewModelBase
    {
        public abstract String Property { get; set; }
        public abstract object Object { get; set; }
        public String Display { get; set; }
        public abstract String NewValueDisplay { get; }
        public abstract String OldValueDisplay { get; }
        public abstract void Execute();
        public abstract void Undo();
    }
    #endregion YeetCommand

    #region YeetPropertyChangedCommand
    public class YeetPropertyChangedCommand : YeetCommand
    {
        public override String Property { get; set; }
        public override object Object { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public override string NewValueDisplay => NewValue?.ToString();
        public override string OldValueDisplay => OldValue?.ToString();

        public YeetPropertyChangedCommand(String property, object obj, object oldValue, object newValue)
        {
            Property = property;
            Object = obj;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override void Execute()
        {
            ReflectionHelper.PropertyInfoCollection[Object.GetType()][Property].SetValue(Object, NewValue);
        }

        public override void Undo()
        {
            ReflectionHelper.PropertyInfoCollection[Object.GetType()][Property].SetValue(Object, OldValue);
        }
    }
    #endregion YeetPropertyChangedCommand

    #region YeetCollectionPropertyChangedCommand
    public class YeetCollectionPropertyChangedCommand : YeetCommand
    {
        public override String Property { get; set; }
        public override object Object { get; set; }
        public IList OldItems { get; set; }
        public IList NewItems { get; set; }
        public override string NewValueDisplay => $"[{NewItems?.Count ?? 0}]";
        public override string OldValueDisplay => $"[{OldItems?.Count ?? 0}]";

        public YeetCollectionPropertyChangedCommand(String property, object obj, IList oldItems, IList newItems)
        {
            Property = property;
            Object = obj;
            OldItems = oldItems;
            NewItems = newItems;
        }

        public override void Execute()
        {
            if (OldItems != null)
            {
                foreach (var item in OldItems)
                {
                    ReflectionHelper.MethodInfoCollection[Object.GetType()]["RemoveChild"].Invoke(Object, new Object[] { item });
                }
            }

            if (NewItems != null)
            {
                foreach (var item in NewItems)
                {
                    ReflectionHelper.MethodInfoCollection[Object.GetType()]["AddChild"].Invoke(Object, new Object[] { item });
                }
            }
        }

        public override void Undo()
        {
            if (OldItems != null)
            {
                foreach (var item in OldItems)
                {
                    ReflectionHelper.MethodInfoCollection[Object.GetType()]["AddChild"].Invoke(Object, new Object[] { item });
                }
            }

            if (NewItems != null)
            {
                foreach (var item in NewItems)
                {
                    ReflectionHelper.MethodInfoCollection[Object.GetType()]["RemoveChild"].Invoke(Object, new Object[] { item });
                }
            }
        }
    }
    #endregion YeetCollectionPropertyChangedCommand
}
