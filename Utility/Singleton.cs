using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance { get; private set; }
	public virtual void Awake()
	{
		if(Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this as T;
	}
}
public class SingletonPersistence<T> : Singleton<T> where T : MonoBehaviour
{
	public override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(Instance);
	}
}
