using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public int stageNum;
    public string stageName;
    public int totalEnemyCount;
    public GameObject backGroundPrefab;
    public float backgroundWidthSize;
    public float backgroundHeightSize;
    public float backgroundPPU;
    public Vector3 PlayerSpawnPosition;
    public List<EnemySpawnInfo> enemies;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyData enemyData;
    public EnemyData.Type enemyType;
    public int count;
}