using UnityEngine;

public class SwordsMan : PlayerBase
{
    public override string PlayerName => "SwordsMan";
    public override int MaxHP { get; set; } = 200;
    public override float AttackRange => 1f;
    public override int AttackDamage { get; set; } = 5;
    public override float AttackCoolTime => 1f;
}
