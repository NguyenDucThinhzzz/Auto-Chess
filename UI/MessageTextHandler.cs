using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageTextHandler : MonoBehaviour
{
    public Animator Animator {  get; private set; }
    public TextMeshProUGUI Text { get; private set; }
	private void Awake()
	{
		Animator = GetComponent<Animator>();
		Text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void ShowText(string str)
	{
		Text.text = str;
		Animator.SetTrigger("SendMessage");
	}
}
