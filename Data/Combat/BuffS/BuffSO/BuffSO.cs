using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Buff SO/new BuffSO")]
public class BuffSO : ScriptableObject
{
	public string Name;
	public Sprite Image;
	public List<BuffInfo> BuffStages = new List<BuffInfo>();
	public ModifierBaseType GetBuffTarget(int count)
	{
		for (int i= BuffStages.Count-1; i>=0 ;i--)
		{
			if (count >= BuffStages[i].CountTarget)
			{
				return BuffStages[i].Buff;
			}
		}
		return null;
	}
	public int GetTargetNumber(int count)
	{
		for (int i = 0; i < BuffStages.Count; i++)
		{
			if (count < BuffStages[i].CountTarget)
			{
				return BuffStages[i].CountTarget;
			}
		}
		return BuffStages.Last().CountTarget;
	}
}
