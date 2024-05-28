using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer {  get; private set; }
	private void Awake()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}

}
