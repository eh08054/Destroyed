using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    public float speed = 10f;
    public float distance = 10f;
    private float moveDirection;
    private ParticleSystem mainParticle;

    private Vector3 initPosition;
    [SerializeField] private Vector3 offset;
    public void Fire()
    {
        mainParticle = GetComponent<ParticleSystem>();
        mainParticle.Play(true);
        PlayerController playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        initPosition = transform.position + playerController.transform.position + offset;
        transform.localScale = new Vector3(transform.localScale.x * playerController.MoveDirection, transform.localScale.y, transform.localScale.z);
        moveDirection = playerController.MoveDirection;
        transform.position = initPosition;
    }
    private void Update()
    {
        if(mainParticle && mainParticle.isPlaying)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right * moveDirection, speed * Time.deltaTime);

            if(Vector3.Distance(initPosition, transform.position) > distance)
            {
                mainParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.TryGetComponent(out EnemyController enemyController)) { return; }
            enemyController.TakeDamage(10);
        }
    }
}