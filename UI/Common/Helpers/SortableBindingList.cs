using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace UI.PresentationDesign.DesignUI.Classes.Helpers
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _Sorted = false;
        private PropertyDescriptor _sortProperty = null;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public SortableBindingList()
            : base()
        {
        }

        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return _Sorted; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;

            if (items != null)
            {
                PropertyComparer<T> pc = new PropertyComparer<T>(prop, direction);
                items.Sort(pc);
                _Sorted = true;

                _sortProperty = prop;
                _sortDirection = direction;

                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
            {
                _Sorted = false;
            }
        }

        protected override void RemoveSortCore()
        {
            _Sorted = false;
            //this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// пока не поддерживаем поиск
        /// </summary>
        protected override bool SupportsSearchingCore
        {
            get
            {
                return false;
            }
        }

    }


    public class PropertyComparer<T> : System.Collections.Generic.IComparer<T>
    {
        private PropertyDescriptor _property;
        private ListSortDirection _direction;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            _property = property;
            _direction = direction;
        }

        #region IComparer<T>

        public int Compare(T xWord, T yWord)
        {
            object xValue = GetPropertyValue(xWord, _property.Name);
            object yValue = GetPropertyValue(yWord, _property.Name);

            if (_direction == ListSortDirection.Ascending)
            {
                return CompareAscending(xValue, yValue);
            }
            else
            {
                return CompareDescending(xValue, yValue);
            }
        }

        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        private int CompareAscending(object xValue, object yValue)
        {
            int result;

            if (xValue is IComparable)
            {
                result = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue.Equals(yValue))
            {
                result = 0;
            }
            else result = xValue.ToString().CompareTo(yValue.ToString());

            return result;
        }

        private int CompareDescending(object xValue, object yValue)
        {
            return CompareAscending(xValue, yValue) * -1;
        }

        private object GetPropertyValue(T value, string property)
        {
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);
            return propertyInfo.GetValue(value, null);
        }
    }

}
