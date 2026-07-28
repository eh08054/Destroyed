using UnityEngine;

public class Sorcerer : PlayerBase
{
    public override string PlayerName => "Sorcerer";
    public override int MaxHP { get; set; } = 200;
    public override float AttackRange => 1f;
    public override int ATK { get; set; } = 20;
    public override int DEF { get; set; } = 0;
    public override float AttackCoolTime => 1f;
}
