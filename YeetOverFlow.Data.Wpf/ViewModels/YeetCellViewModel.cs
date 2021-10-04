using System;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetCellViewModel : YeetKeyedItemViewModel
    {
        YeetCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        //Would have preferred test to be a generic method but Automapper setup crashes with generic methods in nongeneric classes
        internal virtual object GetValue()
        {
            throw new InvalidOperationException("This type does not support returning a value");
        }
        internal virtual void SetValue(object val)
        {
            throw new InvalidOperationException("This type does not support setting a value");
        }
    }

    public class YeetCellViewModel<T> : YeetCellViewModel
    {
        T _value;

        YeetCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        #region Value
        public T Value
        {
            get { return _value; }
            set { SetValue(ref _value, value); }
        }

        internal override object GetValue()
        {
            return _value;
        }

        internal override void SetValue(object val)
        {
            Value = (T)val;
        }
        #endregion Value
    }

    public class YeetBooleanCellViewModel : YeetCellViewModel<bool>
    {
        public YeetBooleanCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetBooleanCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetStringCellViewModel : YeetCellViewModel<string>
    {
        public YeetStringCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetStringCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetIntCellViewModel : YeetCellViewModel<int>
    {
        public YeetIntCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetIntCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDoubleCellViewModel : YeetCellViewModel<double>
    {
        public YeetDoubleCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetDoubleCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDateTimeCellViewModel : YeetCellViewModel<DateTime>
    {
        public YeetDateTimeCellViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetDateTimeCellViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    //This only exists because Automapper crashes when a generic method exists in a class that is not generic
    public static class YeetCellViewModelHelper
    {
        public static T GetValue<T>(this YeetCellViewModel vm)
        {
            return (T)vm.GetValue();
        }

        public static void SetValue<T>(this YeetCellViewModel vm, T val)
        {
            vm.SetValue(val);
        }
    }
}
