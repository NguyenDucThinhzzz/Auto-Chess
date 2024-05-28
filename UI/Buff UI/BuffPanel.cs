using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffPanel : MonoBehaviour
{
    [field: SerializeField] public Image Image { get; private set; }
	[field: SerializeField] public TextMeshProUGUI BuffName { get; private set; }
	[field: SerializeField] public TextMeshProUGUI BuffInfo { get; private set; }
	[field: SerializeField] public TextMeshProUGUI Count { get; private set; }
}
