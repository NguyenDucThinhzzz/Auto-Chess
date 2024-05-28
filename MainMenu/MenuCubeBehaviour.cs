using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCubeBehaviour : MonoBehaviour
{
    [field:SerializeField] public float MoveValue { get; private set; } = 2f;
	[field: SerializeField] public float Speed { get; private set; } = 1f;

	Vector3 originalPos;
    float timer = 0f;
	private void Awake()
	{
		originalPos = transform.position;
	}
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        transform.position = originalPos + new Vector3(0, Mathf.Sin(timer * Speed) * MoveValue,0);
		transform.Rotate(new Vector3(0,0.1f,0), Space.Self);
	}
}
