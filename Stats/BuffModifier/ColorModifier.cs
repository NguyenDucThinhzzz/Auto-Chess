using ResilientCore;
public class ColorModifier
{
	public ColorClass ColorClass;
	public ModifierBaseType ModifierBaseType;
	public ColorModifier(ColorClass colorClass, EStatType statType, StatModifier modifier)
	{
		ColorClass = colorClass;
		ModifierBaseType = new ModifierBaseType(modifier,statType);
	}
}
