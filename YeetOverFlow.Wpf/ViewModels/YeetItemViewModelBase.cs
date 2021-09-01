using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        public YeetItemViewModelBaseExtended() : base()
        {

        }

        public YeetItemViewModelBaseExtended(Guid guid) : base(guid)
        {

        }

        public event EventHandler<PropertyChangedExtendedEventArgs> PropertyChangedExtended;
        public event EventHandler<CollectionPropertyChangedEventArgs> CollectionPropertyChanged;

        protected virtual void OnPropertyChangedExtended(PropertyChangedExtendedEventArgs eventArgs, [CallerMemberName] string propertyName = null)
        {
            PropertyChangedExtended?.Invoke(this, eventArgs);
        }

        protected virtual void OnCollectionPropertyChanged(NotifyCollectionChangedEventArgs eventArgs, [CallerMemberName] string propertyName = null)
        {
            CollectionPropertyChanged?.Invoke(this, new CollectionPropertyChangedEventArgs(propertyName, this, eventArgs.Action, eventArgs.OldItems, eventArgs.NewItems));
        }

        protected virtual void OnCollectionPropertyChanged(CollectionPropertyChangedEventArgs eventArgs, [CallerMemberName] string propertyName = null)
        {
            CollectionPropertyChanged?.Invoke(this, eventArgs);
        }

        protected virtual void OnPropertyChangedExtended(object oldValue, object newValue, [CallerMemberName] string propertyName = null)
        {
            PropertyChangedExtended?.Invoke(this, new PropertyChangedExtendedEventArgs(propertyName, this, oldValue, newValue));
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
