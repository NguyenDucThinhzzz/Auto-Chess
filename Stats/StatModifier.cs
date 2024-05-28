using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{
    public enum EModifierType
    {
        BaseFlat,
        Percentage,
        Flat,
    }
    [Serializable]
    public class StatModifier
    {
        [field: SerializeField] public float Value { get; private set; }
		[field: SerializeField] public EModifierType ModifierType { get; private set; }
        [field: SerializeField] public float Timer { get; set; } = -1;

        public StatModifier(float _value, EModifierType _modifierType, float _timer = -1f)
        {
            Value = _value;
            ModifierType = _modifierType;
            Timer = _timer;
        }

        public StatModifier() : this(0f, EModifierType.Flat) { }
        public StatModifier(StatModifier modifier) : this(modifier.Value, modifier.ModifierType, modifier.Timer) { }
		public override bool Equals(object obj)
		{
            StatModifier modifier = obj as StatModifier;
			return (modifier.Value == Value && modifier.ModifierType == ModifierType && modifier.Timer == Timer);
		}
	}
}
