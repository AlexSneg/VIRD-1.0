using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace UI.PresentationDesign.DesignUI.Services
{
    public class PropertyChangedHistoryEntry : IUndoRedoAction
    {
        object NewValue;
        object OldValue;
        //Dictionary<PropertyDescriptor, object> Chain;
        PropertyDescriptor SingleProperty = null;
        object _Target;
        object _ComponentChangedValue;
        Slide destSlide;

        private List<ValueTypeChainItem> NewChain;

        #region Nested

        private class ValueTypeChainItem
        {
            public ValueTypeChainItem(PropertyDescriptor propertyDescriptor,
                object value, object target)
            {
                PropertyDescriptor = propertyDescriptor;
                Value = value;
                Target = target;
            }

            public PropertyDescriptor PropertyDescriptor { get; private set; }

            public object Value { get; private set; }

            public object Target { get; private set; }
        }

        #endregion

        public PropertyChangedHistoryEntry(object ATarget, List<PropertyDescriptor> PropertyChain, object AOldValue)
        {
            destSlide = PresentationController.Instance.SelectedSlide;

            _Target = ATarget;
            OldValue = AOldValue;
            SingleProperty = PropertyChain[0];

            if (PropertyChain.Count == 1)
            {
                NewValue = SingleProperty.GetValue(_Target);
            }
            else
            {
                object value = null;
                object target = _Target;
                //Chain = new Dictionary<PropertyDescriptor, object>();
                NewChain = new List<ValueTypeChainItem>();
                bool shouldContinue = true;
                for (int i = PropertyChain.Count - 1; i >= 1 && shouldContinue; --i)
                {
                    value = PropertyChain[i].GetValue(target);
                    if (PropertyChain[i].PropertyType.IsValueType)
                    {
                        //Chain.Add(PropertyChain[i], value);
                        NewChain.Add(new ValueTypeChainItem(PropertyChain[i], value, target));
                        shouldContinue = false;
                        //break;
                    }
                    target = value;
                }
                //Chain.Add(PropertyChain[0], OldValue);
                NewChain.Add(new ValueTypeChainItem(PropertyChain[0], OldValue, target));

                if (value != null)
                    NewValue = PropertyChain[0].GetValue(value);

                _ComponentChangedValue = target;
            }
        }

        public void Undo()
        {
            //if (Chain == null)
            if (NewChain == null)
                SingleProperty.SetValue(_Target, OldValue);
            else
                ChangePropertyChainWithValue(OldValue);
        }

        public void Redo()
        {
            //if (Chain == null)
            if (NewChain == null)
                SingleProperty.SetValue(_Target, NewValue);
            else
                ChangePropertyChainWithValue(NewValue);
        }

        void ChangePropertyChainWithValue(object AValue)
        {
            object target = _Target;
            //foreach (var p in Chain)
            for (int i = NewChain.Count - 1; i >= 0 ; i--)
            {
                ValueTypeChainItem item = NewChain[i];
                if (item.PropertyDescriptor.PropertyType.IsValueType)
                {
                    object value = i == NewChain.Count - 1 ? AValue : item.Value;
                    item.PropertyDescriptor.SetValue(item.Target, value);
                }
                else
                {
                    SingleProperty.SetValue(_ComponentChangedValue, AValue);
                    break;
                }
                ////if (!p.Key.PropertyType.IsValueType)
                //if (p.Key.PropertyType.IsValueType)
                //{
                //    p.Key.SetValue(target, target = p.Value);       
                //    //p.Key.SetValue(target, p.Value);       
                //    //p.Key.SetValue(_ComponentChangedValue, p.Value);       
                //}
                //else
                //{
                //    SingleProperty.SetValue(_ComponentChangedValue, AValue);
                //    break;
                //}
            }
        }

        #region IUndoRedoAction Members

        public string Tag
        {
            get { return "Изменение свойства " + SingleProperty.DisplayName; }
        }

        public object Target
        {
            get { return _Target; } 
        }

        #endregion

        #region ISimpleUndoRedoAction Members

        public bool CanUndo()
        {
            return destSlide.IsLocked;
        }

        public bool CanRedo()
        {
            return destSlide.IsLocked;
        }

        #endregion
    }

}
