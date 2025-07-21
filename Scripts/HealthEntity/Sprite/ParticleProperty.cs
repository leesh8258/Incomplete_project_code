using System;
using UnityEngine;

[Serializable]
public class ParticleProperty
{
    public enum ParticleState
    {
        Idle,
        Move,
        Chase,
        BeforeAttack,
        AttackA,
        AttackB,
        AttackC,
        Stun,
        Death,
        Hit
    }

    public ParticleState stateType;
    public ParticleSystem[] particleList;
}
