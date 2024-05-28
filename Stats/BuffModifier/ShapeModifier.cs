using ResilientCore;
public class ShapeModifier
{
	public ShapeClass ShapeClass;
	public ModifierBaseType ModifierBaseType;
	public ShapeModifier(ShapeClass shapeClass, EStatType statType, StatModifier modifier)
	{
		ShapeClass = shapeClass;
		ModifierBaseType = new ModifierBaseType(modifier,statType);
	}
}
