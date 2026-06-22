using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class SkillController : MonoBehaviour
{
    private GameObject currentSkillEffect;
    public ActiveSkill[] EquipedSkills { get; private set; }
    private PlayerBase playerBase;
    private Animator animator;
    public int skillIndex = 0;

    private void Start()
    {
        playerBase = GameManager.Instance.PlayerBase;
        EquipedSkills = new ActiveSkill[3];
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (EquipedSkills[0] == null || !EquipedSkills[0].IsReady) { return; }
            TryUseSkill(EquipedSkills[0], 0);
        }
    }

    private void TryUseSkill(ActiveSkill activeSkill, int index)
    {
        currentSkillEffect = activeSkill.ActiveData.skillEffectPrefab;
        if (activeSkill.ActiveData.attackType == ActiveSkillData.AttackType.projectile)
        {
            if (playerBase.currentWeapon.weaponType == WeaponData.WeaponType.Sword)
            {
                animator.SetTrigger("Projectile");
            }
            else { return; }
        }
        StartCoroutine(SkillCoolTime(activeSkill, index));
    }
    public void EquipSkill(int index, ActiveSkill activeSkill)
    {
        EquipedSkills[index] = activeSkill;
        UIManager.Instance.RegisterSkill(index, activeSkill);
    }
    public void PlaySkillParticle()
    {
        GameObject prefab = Instantiate(currentSkillEffect);
        if (prefab)
        {
            if (prefab.TryGetComponent(out ProjectileEffect projectileEff))
            {
                projectileEff.Fire();
            }
        }
    }

    private IEnumerator SkillCoolTime(ActiveSkill skill, int index)
    {
        skill.CurrentCooldown = skill.ActiveData.cooldown;
        UIManager.Instance.ActiveCoolDownImage(index);

        while(skill.CurrentCooldown > 0f)
        {
            skill.CurrentCooldown -= Time.deltaTime;
            skill.CurrentCooldown = Mathf.Max(0f, skill.CurrentCooldown);

            float ratio = skill.CurrentCooldown / skill.ActiveData.cooldown;
            UIManager.Instance.CoolDownImage(index, ratio);

            yield return null;
        }

        UIManager.Instance.CoolDownImage(index, 0f);
    }
}
