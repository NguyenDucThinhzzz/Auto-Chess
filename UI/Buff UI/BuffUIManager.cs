using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    [field:SerializeField] public BuffPanel BuffPanelPrefab {  get; private set; }
    public Dictionary<string,BuffPanel> Panels = new Dictionary<string,BuffPanel>();
    public void UpdatePanel(string name, BuffSO buff, int count)
    {
        if (!Panels.ContainsKey(name)) return;
		BuffPanel temp = Panels[name];
		ModifierBaseType mod = buff.GetBuffTarget(count);
		temp.Image.sprite = buff.Image;
        temp.BuffName.text = buff.Name;
		string a = (mod.Modifier.ModifierType is ResilientCore.EModifierType.Percentage) ? "%" : "";
		temp.BuffInfo.text = "+ " + mod.StatType.ToString() + " " + mod.Modifier.Value + a;
		temp.Count.text = count.ToString() +"/"+buff.GetTargetNumber(count);
		Panels[name] = temp;
	}
    public void AddPanel(string name, BuffSO buff, int count)
    {
		if (Panels.ContainsKey(name))
        {
            UpdatePanel(name, buff, count);
            return;
        }
		BuffPanel temp = Instantiate(BuffPanelPrefab, transform);
        ModifierBaseType mod = buff.GetBuffTarget(count);
		temp.Image.sprite = buff.Image;
		temp.BuffName.text = buff.Name;
        string a = (mod.Modifier.ModifierType is ResilientCore.EModifierType.Percentage) ? "%" : "";
		temp.BuffInfo.text = "+ " + mod.StatType.ToString() + " " + mod.Modifier.Value + a;
        temp.Count.text = count.ToString() + "/" + buff.GetTargetNumber(count);
        Panels[name] = temp;
    }
	public void RemovePanel(string name)
	{
        if (!Panels.ContainsKey(name)) return;
        Destroy(Panels[name].gameObject);
        Panels.Remove(name);
	}
}
