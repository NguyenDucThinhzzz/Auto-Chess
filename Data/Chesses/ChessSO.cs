using ResilientCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "new Chess SO")]
public class ChessSO : ScriptableObject
{
	[Header("Info")]
	public int ID;
	public string Name;
	public Sprite Image;
	[TextArea]
	public string Description;
	public ColorClass ColorClass;
	public ShapeClass ShapeClass;
	public BaseStatsData StatData;
	[Header("Audio")]
	public AudioClip Shooting;
}
