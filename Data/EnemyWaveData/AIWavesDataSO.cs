using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "new EnemyWavesData")]
public class AIWavesDataSO : ScriptableObject
{
    public List<AIWaveSO> Waves;
}
