using ResilientCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

public class BaseController : MonoBehaviour
{
	[field:Header("Require Components")]
	[field: SerializeField] public GameBoard GameBoard { get; private set; }
	[field: SerializeField] public ChessBuffSO ChessBuffs { get; private set; }
	public List<BaseChess> OnFieldChess;
	public Dictionary<ColorClass, ModifierBaseType> colorBuffs = new Dictionary<ColorClass, ModifierBaseType>();
	public Dictionary<ShapeClass, ModifierBaseType> shapeBuffs = new Dictionary<ShapeClass, ModifierBaseType>();
	public UnityEvent OnAddChessOnField, OnRemoveChessOnField;

	private Dictionary<ColorClass, int> colorCount;
	private Dictionary<ShapeClass, int> shapeCount;
	private void Awake()
	{
		OnFieldChess = new List<BaseChess>();
		colorCount = new Dictionary<ColorClass, int>();
		shapeCount = new Dictionary<ShapeClass, int>();
		OnAddChessOnField = new UnityEvent();
		OnRemoveChessOnField = new UnityEvent();
	}
	private void Start()
	{
		Initialize();
	}
	private void Update()
	{
		if (this is not PlayerController) return;
		//Debug.Log("");
	}
	//public methods
	public virtual void Initialize()
	{
		foreach (var e in Enum.GetValues(typeof(ColorClass)))
		{
			colorBuffs[(ColorClass)e] = null;
			colorCount[(ColorClass)e] = 0;
		}
		foreach (var e in Enum.GetValues(typeof(ShapeClass)))
		{
			shapeBuffs[(ShapeClass)e] = null;
			shapeCount[(ShapeClass)e] = 0;
		}
	}
	public void AddOnFieldChess(BaseChess chess)
	{
		if (OnFieldChess.Contains(chess)) return;

		OnFieldChess.Add(chess);
		OnAddChessOnField.Invoke();

		colorCount[chess.Data.ColorClass]++;
		shapeCount[chess.Data.ShapeClass]++;

		CheckBuffs();
	}
	public void RemoveOnFieldChess(BaseChess chess)
	{
		if (!OnFieldChess.Contains(chess)) return;

		chess.BaseStats.ResetAllBuff();
		OnFieldChess.Remove(chess);
		OnRemoveChessOnField.Invoke();

		colorCount[chess.Data.ColorClass]--;
		shapeCount[chess.Data.ShapeClass]--;

		CheckBuffs() ;
	}
	public void ResetAllChess()
	{
		foreach (var chess in OnFieldChess)
		{
			chess.ResetChess();
		}
	}
	public void DestroyChess()
	{
		for(int i = OnFieldChess.Count-1;i>=0;i--)
		{
			BaseChess chess = OnFieldChess[i];
			chess.PathNode.Chess = null;
			RemoveOnFieldChess(chess);
			Destroy(chess.gameObject);
		}
		OnFieldChess.Clear();
	}
	//protected virtual methods
	protected virtual void CheckBuffs()
	{
		foreach(var key in colorCount.Keys)
		{
			ColorBuffs(key, colorCount[key]);
		}
		foreach (var key in shapeCount.Keys)
		{
			ShapeBuffs(key, shapeCount[key]);
		}
	}
	protected virtual void ColorBuffs(ColorClass color, int value)
	{
		if (value == 0) return;
		RemoveColorBuff(color);

		//Choose Buff
		colorBuffs[color] = ChessBuffs.GetBuff(color.ToString()).GetBuffTarget(value);

		if (colorBuffs[color] is null) return;

		AddColorBuff(color, colorBuffs[color]);
	}
	protected virtual void AddColorBuff(ColorClass color,ModifierBaseType modifier)
	{
		//Add buff
		foreach (var chess in OnFieldChess.Where(n => n.Data.ColorClass == color))
		{
			chess.BaseStats.AddModifier(modifier);
		}
	}
	protected virtual void RemoveColorBuff(ColorClass color)
	{
		//Remove Buff if it existed
		if (colorBuffs[color] is null) return;

		foreach (var chess in OnFieldChess.Where(n => n.Data.ColorClass == color))
		{
			chess.BaseStats.RemoveModifier(colorBuffs[color]);
		}
		colorBuffs.Remove(color);
	}
	protected virtual void ShapeBuffs(ShapeClass shape, int value)
	{
		if (value == 0) return;
		RemoveShapeBuff(shape);
		//Choose Buff
		shapeBuffs[shape] = ChessBuffs.GetBuff(shape.ToString()).GetBuffTarget(value);

		if (shapeBuffs[shape] is null) return;
		AddShapeBuff(shape, shapeBuffs[shape]);
	}
	protected virtual void AddShapeBuff(ShapeClass shape, ModifierBaseType modifier)
	{
		//Add buff
		foreach (var chess in OnFieldChess.Where(n => n.Data.ShapeClass == shape))
		{
			chess.BaseStats.AddModifier(modifier);
		}
	}
	protected virtual void RemoveShapeBuff(ShapeClass shape)
	{
		//Remove Buff if it existed
		if (shapeBuffs[shape] is null) return;

		foreach (var chess in OnFieldChess.Where(n => n.Data.ShapeClass == shape))
		{
			chess.BaseStats.RemoveModifier(shapeBuffs[shape]);
		}
		shapeBuffs.Remove(shape);
	}
}