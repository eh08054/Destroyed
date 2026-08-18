using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHP;
    public float chaseRange;
    public float attackRange;
    public int attackDamage;
    public float attackCoolTime;
    public int defense;
    public float moveSpeed;
    public Vector3 offset;
    public enum Type { Normal, Elite, Boss}
    public enum State { Idle, Chase, CoolTime, Attack, Dead }
    public GameObject enemyPrefab;
    public Sprite enemyIcon;
    public ItemDropTable dropTable;
}
