using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[field: SerializeField] public Image HealthImage { get; private set; }
    public Slider Slider { get; private set; }
	private void Awake()
	{
		Slider = GetComponent<Slider>();
	}
	public void SetMaxHealth(float max)
	{
		Slider.maxValue = max;
		Slider.value = max;
	}
	public void ChangeValue(float value)
	{
		Slider.value = value;
	}
}
