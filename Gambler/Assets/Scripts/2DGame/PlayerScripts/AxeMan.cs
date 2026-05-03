using UnityEngine;

public class AxeMan : PlayerBase
{
    public override string PlayerName => "AxeMan";
    public override int MaxHP => 200;
    public override float AttackRange => 1f;
    public override int AttackDamage => 20;
    public override float AttackCoolTime => 1f;
}
