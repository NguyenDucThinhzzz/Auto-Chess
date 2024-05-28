using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utility : Singleton<Utility>
{
    [field: SerializeField] public GameBoard Board { get; private set; }
	[field: SerializeField] public bool enableDebug { get; private set; } = false;
    public List<GameObject> DebugTileText = new List<GameObject>();
	private void Start()
	{
        DisableDebug();
	}
	private void Update()
	{
        if (!enableDebug) return;

        for(int i = 0;i< Board.Grid.Width; i++)
        {
			for (int j = 0; j < Board.Grid.Height; j++)
			{
                Board.Grid.GridArray[i, j].DebugTileText.text = i + "," + j + "\n" + Board.Grid.GridArray[i, j].Chess;
			}
		}
	}
	//Create Text in world space
	public TextMesh CreateTextInWorld(Transform parent, string text, Vector3 localPos, int fontSize, Color color, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = localPos;
        gameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.characterSize = 0.04f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        DebugTileText.Add(gameObject);
        return textMesh;
    }

    [ContextMenu("Enable Debugging")]
    public void EnableDebug()
    {
        enableDebug = true;
        foreach (GameObject gameObject in DebugTileText)
        {
            gameObject.SetActive(true);
        }
    }
	[ContextMenu("Disable Debugging")]
	public void DisableDebug()
	{
		enableDebug = false;

	    foreach (GameObject gameObject in DebugTileText)
		{
			gameObject.SetActive(false);
		}
	}
}
