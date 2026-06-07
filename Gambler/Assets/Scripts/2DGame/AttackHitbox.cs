using UnityEngine;
using System;

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
                var attackDamage = attackData.AttackDamage + GameManager.Instance.PlayerBase.AttackDamage
                    + GameManager.Instance.PlayerBase.currentWeapon.weaponDamage;
                enemyController.TakeDamage(attackDamage);
                AudioManager.instance.PlaySFX(attackData.Attack_Hit);
            }
        }
    }
}
