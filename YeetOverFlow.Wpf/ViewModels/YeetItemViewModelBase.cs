using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YeetOverFlow.Core;

namespace YeetOverFlow.Wpf.ViewModels
{
    #region YeetItemViewModelBase
    public abstract class YeetItemViewModelBase : YeetItem, INotifyPropertyChanged
    {
        public YeetItemViewModelBase() : base()
        {

        }

        public YeetItemViewModelBase(Guid guid) : base(guid)
        {

        }

        #region Name
        public override String Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }
        #endregion Name

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            SetValue<T>(ref backingField, value, true, propertyname);
        }

        protected virtual void SetValue<T>(ref T backingField, T value, Boolean doEqualityCheck, [CallerMemberName] string propertyname = null)
        {
            if (doEqualityCheck)
            {
                if (EqualityComparer<T>.Default.Equals(backingField, value))
                    return;
            }

            backingField = value;
            OnPropertyChanged(propertyname);
        }
        protected override void SetSequence(int sequence, YeetItem yeetItem)
        {
            base.SetSequence(sequence, yeetItem);
            OnPropertyChanged(nameof(YeetItem.Sequence));
        }
    }
    #endregion YeetItemViewModelBase

    #region PropertyChangedExtendedEventArgs
    //https://stackoverflow.com/questions/7677854/notifypropertychanged-event-where-event-args-contain-the-old-value
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public virtual object Object { get; private set; }
        public virtual object OldValue { get; private set; }
        public virtual object NewValue { get; private set; }
        //public virtual String Message { get; set; }

        public PropertyChangedExtendedEventArgs(string propertyName, object obj, object oldValue, object newValue)
            : base(propertyName)
        {
            Object = obj;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
    #endregion PropertyChangedExtendedEventArgs

    #region MultiPropertyChangedExtendedEventArgs
    public class MultiPropertyChangedExtendedEventArgs : PropertyChangedExtendedEventArgs
    {
        public virtual List<PropertyChangedEventArgs> ArgsList { get; private set; }
        public MultiPropertyChangedExtendedEventArgs(string propertyName, object obj, List<PropertyChangedEventArgs> argsList, object oldValue, object newValue)
            : base(propertyName, obj, oldValue, newValue)
        {
            ArgsList = argsList;
        }
    }
    #endregion MultiPropertyChangedExtendedEventArgs

    #region CollectionPropertyChangedEventArgs
    public class CollectionPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public virtual NotifyCollectionChangedAction Action { get; private set; }
        public virtual object Object { get; private set; }
        public virtual IList OldItems { get; private set; }
        public virtual IList NewItems { get; private set; }
        //public virtual String Message { get; set; }
        public CollectionPropertyChangedEventArgs(string propertyName, object obj, NotifyCollectionChangedAction action, IList oldValue, IList newValue)
            : base(propertyName)
        {
            Object = obj;
            Action = action;
            OldItems = oldValue;
            NewItems = newValue;
        }
    }
    #endregion CollectionPropertyChangedEventArgs

    #region YeetItemViewModelBaseExtended
    public abstract class YeetItemViewModelBaseExtended : YeetItemViewModelBase
    {
        bool _inChangeScope = false;
        List<PropertyChangedEventArgs> _propertyChanges = new List<PropertyChangedEventArgs>();

        public YeetItemViewModelBaseExtended() : base()
        {

        }

        public YeetItemViewModelBaseExtended(Guid guid) : base(guid)
        {

        }

        public event EventHandler<PropertyChangedExtendedEventArgs> PropertyChangedExtended;
        public event EventHandler<CollectionPropertyChangedEventArgs> CollectionPropertyChanged;

        public class ChangeScope : IDisposable {
            YeetItemViewModelBaseExtended _model;
            string _name;
            object _oldValue;
            object _newValue;

            public ChangeScope(YeetItemViewModelBaseExtended model, string name = "", object oldValue = null, object newValue = null)
            {
                _model = model;
                _model._inChangeScope = true;
                _name = name;
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public void Dispose() => _model.DispatchChanges(_name, _oldValue, _newValue);
        }

        private void DispatchChanges(string name = "", object oldValue = null, object newValue = null)
        {
            _inChangeScope = false;

            if (_propertyChanges.Count > 0)
            {
                OnPropertyChangedExtended(new MultiPropertyChangedExtendedEventArgs(name, this, new List<PropertyChangedEventArgs>(_propertyChanges), oldValue, newValue));
                _propertyChanges.Clear();
            }
        }

        protected virtual void OnPropertyChangedExtended(PropertyChangedExtendedEventArgs eventArgs, [CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine($"[{eventArgs.PropertyName}] {eventArgs.OldValue} => {eventArgs.NewValue}");

            if (_inChangeScope)
            {
                _propertyChanges.Add(eventArgs);
            }
            else
            {
                PropertyChangedExtended?.Invoke(this, eventArgs);
            }
        }

        protected virtual void OnPropertyChangedExtended(object oldValue, object newValue, [CallerMemberName] string propertyName = null)
        {
            OnPropertyChangedExtended(new PropertyChangedExtendedEventArgs(propertyName, this, oldValue, newValue));
        }

        protected virtual void OnCollectionPropertyChanged(CollectionPropertyChangedEventArgs eventArgs, [CallerMemberName] string propertyName = null)
        {
            if (eventArgs.NewItems != null && eventArgs.NewItems.Count > 0)
            {
                Debug.WriteLine($"Add {((YeetItem)eventArgs.NewItems[0]).Guid}");
            }

            if (eventArgs.OldItems != null && eventArgs.OldItems.Count > 0)
            {
                Debug.WriteLine($"Remove {((YeetItem)eventArgs.OldItems[0]).Guid}");
            }

            if (_inChangeScope)
            {
                _propertyChanges.Add(eventArgs);
            }
            else
            {
                CollectionPropertyChanged?.Invoke(this, eventArgs);
            }
        }

        protected virtual void OnCollectionPropertyChanged(NotifyCollectionChangedEventArgs eventArgs, [CallerMemberName] string propertyName = null)
        {
            OnCollectionPropertyChanged(new CollectionPropertyChangedEventArgs(propertyName, this, eventArgs.Action, eventArgs.OldItems, eventArgs.NewItems));
        }

        protected override void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            SetValue<T>(ref backingField, value, true, propertyName);
        }

        protected override void SetValue<T>(ref T backingField, T value, Boolean doEqualityCheck, [CallerMemberName] string propertyName = null)
        {
            if (doEqualityCheck)
            {
                if (EqualityComparer<T>.Default.Equals(backingField, value))
                    return;
            }

            T oldValue = backingField;
            backingField = value;
            OnPropertyChanged(propertyName);
            OnPropertyChangedExtended(oldValue, value, propertyName);
        }
    }
    #endregion YeetItemViewModelBaseExtended
}
