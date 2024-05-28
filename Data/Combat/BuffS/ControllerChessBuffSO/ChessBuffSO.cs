using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Buff SO/new ChessBuffsSO")]
public class ChessBuffSO : ScriptableObject
{
	[Header("Color Buff")]
	public BuffSO RedBuff;
	public BuffSO BlueBuff;
	public BuffSO GreenBuff;
	public BuffSO YellowBuff;
	[Header("Shape Buff")]
	public BuffSO CubeBuff;
	public BuffSO SphereBuff;
	public BuffSO CapsuleBuff;


	public BuffSO GetBuff(string name)
	{
		switch (name)
		{
			case "Red":
				return RedBuff;
			case "Green":
				return GreenBuff;
			case "Blue":
				return BlueBuff;
			case "Yellow":
				return YellowBuff;
			case "Cube":
				return CubeBuff;
			case "Sphere":
				return SphereBuff;
			case "Capsule":
				return CapsuleBuff;
		}
		return null;
	}
}
