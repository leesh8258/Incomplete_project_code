using UnityEngine;

[System.Serializable]
public class PlayerStatBase : CharacterStatBase
{
    [field: SerializeField] public float mentality { get; set; }
    [field: SerializeField] public float criticalChance { get; set; }
    [field: SerializeField] public float criticalDamage { get; set; }
    [field: SerializeField] public float createSpeed { get; set; }
    [field: SerializeField] public float createPower { get; set; }
    [field: SerializeField] public int potentialPower { get; set; }

    public PlayerStatBase Clone()
    {
        PlayerStatBase clone = new PlayerStatBase();
        clone.HP = this.HP;
        clone.defense = this.defense;
        clone.meleeAttackPower = this.meleeAttackPower;
        clone.meleeAttackSpeed = this.meleeAttackSpeed;
        clone.rangeAttackPower = this.rangeAttackPower;
        clone.rangeAttackSpeed = this.rangeAttackSpeed;
        clone.evasion = this.evasion;
        clone.resistance = this.resistance;
        clone.mentality = this.mentality;
        clone.criticalChance = this.criticalChance;
        clone.criticalDamage = this.criticalDamage;
        clone.createSpeed = this.createSpeed;
        clone.createPower = this.createPower;
        clone.potentialPower = this.potentialPower;
        return clone;
    }
}
