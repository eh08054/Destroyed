using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public GameObject Enemy { get; private set; }
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector2 playerStartPosition;
    [SerializeField] private Vector2 enemyStartPosition;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        Enemy = Instantiate(enemyPrefab, enemyStartPosition, Quaternion.identity);
    }
}
