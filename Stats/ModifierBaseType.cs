using ResilientCore;
using System;
using UnityEngine;

[Serializable]
public class ModifierBaseType
{
	public StatModifier Modifier;
	public EStatType StatType;
	public ModifierBaseType(ModifierBaseType modifier)
	{
		Modifier = new StatModifier(modifier.Modifier);
		StatType = modifier.StatType;
	}
	public ModifierBaseType(StatModifier modifier, EStatType statType)
	{
		Modifier = modifier;
		StatType = statType;

	}
	public ModifierBaseType(float value, EModifierType modifierType, EStatType statType, float timer = -1)
	{
		Modifier = new StatModifier(value, modifierType, timer);
		StatType = statType;
	}
}
