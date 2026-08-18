using UnityEngine;
using System.Collections;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private AttackData attackData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = collision.GetComponentInParent<EnemyController>();
            if(enemyController != null)
            {
                if (enemyController.Enemy.CurrentState != EnemyData.State.Dead)
                {
                    var attackDamage = attackData.AttackDamage + GameManager.Instance.PlayerBase.ATK
                        + GameManager.Instance.PlayerBase.currentWeapon.weaponDamage
                        - enemyController.Enemy.Data.defense;
                    enemyController.TakeDamage(attackDamage);
                    AudioManager.instance.PlaySFX(attackData.Attack_Hit);
                    StartCoroutine(HitStop(enemyController.Animator));
                }
            }
        }
    }
    private IEnumerator HitStop(Animator animator, float duration = 0.05f)
    {
        animator.speed = 0f;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        animator.speed = 1f;
    }
}
