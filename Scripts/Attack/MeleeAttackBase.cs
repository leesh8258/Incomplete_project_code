
using UnityEngine;

public abstract class MeleeAttackBase : AttackBase
{
    protected Collider[] hitResults = new Collider[20];

    public override float ReturnAtk()
    {
        return character.CharacterStat.meleeAttackPower;
    }
}
