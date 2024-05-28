using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.Events;
using System.Linq;

namespace ResilientCore
{
    public class Stat
    {
		public List<StatModifier> StatModifiers { get;}
        public UnityEvent OnChangeValue { get; set; } = new UnityEvent();

		public float BaseValue { get; private set; }
		public float AddedValue
        {
            get
            {
                return Value - BaseValue;
			}
        }
		public float Value 
        {
            get
            {
                if (isDirty)
                {
                    isDirty = false;
                    value = CalculateValue();
                }
                return value;
            }
        }

        private float value;
        private bool isDirty;

        public Stat(float _value) : this(_value, null) { }
        public Stat(float _value, List<StatModifier> _list = null) 
        {
            value = BaseValue = _value;
            if (_list == null) 
            {
                StatModifiers = new List<StatModifier>();
            }
            else
            {
                StatModifiers = new List<StatModifier>(_list);
            }   
        }

        public void AddModifier(StatModifier modifier)
		{
			StatModifiers.Add(new StatModifier(modifier));
			SetDirty();
		}


		public void RemoveModifier(StatModifier modifier)
        {
			StatModifiers.Remove(modifier);
			SetDirty();
        }
		public void RemoveAllModifier()
		{
			StatModifiers.Clear();
			SetDirty();
		}

		public void ChangeBaseValue(float _value)
        {
			BaseValue = _value;
			SetDirty();
        }

		private void SetDirty()
		{
			isDirty = true;
			OnChangeValue.Invoke();
		}
        private float CalculateValue()
        {
            float _baseFlatSum = 0f;
            float _percentageSum = 0f;
            float _flatSum = 0f;

            foreach (StatModifier modifier in StatModifiers)
            {
                if(modifier.ModifierType == EModifierType.BaseFlat)
                {
                    _baseFlatSum += modifier.Value;
                }
                else if(modifier.ModifierType == EModifierType.Percentage)
                {
                    _percentageSum += modifier.Value;
                }
                else if (modifier.ModifierType == EModifierType.Flat)
                {
                    _flatSum += modifier.Value;
                }
            }
            float _finalValue = (BaseValue + _baseFlatSum) * (1f + _percentageSum/100f) + _flatSum;
            return (float) Math.Round(_finalValue, 4);
        }

        public void UpdateModifierTimer()
        {
			for (int i= StatModifiers.Count-1; i>=0; --i)
            {
				if (StatModifiers[i].Timer == -1)
                    return;
                if(StatModifiers[i].Timer <= 0)
                {
                    RemoveModifier(StatModifiers[i]);
                    return;
                }
                StatModifiers[i].Timer -= Time.deltaTime;
            }
        }
	}
}
