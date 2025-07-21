using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public Slider hpSlider;
    public Slider mtSlider;
    public bool isRanged = false;
    public WeaponType weaponType;
    public float attakcRange;
    private float currentMT;
    private InputController inputController;

    [SerializeField] private PlayerStatBase playerStat;
    [SerializeField] private PlayerStatBase currentPlayerStat;

    public override CharacterStatBase CharacterStat { get { return currentPlayerStat; } }

    protected override void Awake()
    {
        base.Awake();
        inputController = GetComponent<InputController>();
    }

    private void Start()
    {
        currentPlayerStat = playerStat.Clone();

        currentHP = currentPlayerStat.HP;
        currentMT = currentPlayerStat.mentality;

        hpSlider.value = currentHP / currentPlayerStat.HP;
        mtSlider.value = currentMT / currentPlayerStat.mentality;
    }

    public void UseMP(float amount)
    {
        if (currentMT - amount < 0)
        {
            Debug.Log("MP가 부족합니다.");
            return;
        }
        currentMT -= amount;
        mtSlider.value = currentMT / currentPlayerStat.mentality;
    }

    public override void TakeDamage(float damage, AttackBase attackType)
    {
        base.TakeDamage(damage, attackType);
        hpSlider.value = currentHP / currentPlayerStat.HP;
    }

    public override void ApplyStatBoost(Action<Character> action)
    {
        action(this);
    }

    public void ApplyEquipmentStats(EquipmentItemSO equipmentItem)
    {
        currentPlayerStat.defense += equipmentItem.def;
        currentPlayerStat.criticalChance += equipmentItem.critChance;
        currentPlayerStat.criticalDamage += equipmentItem.critDamage;
        currentPlayerStat.resistance += equipmentItem.resistance;
        if (equipmentItem.equipmentType == EquipmentType.Weapon)
        {
            attakcRange = equipmentItem.attackRange;
            weaponType = equipmentItem.weaponType;
            switch (equipmentItem.weaponType)
            {
                case WeaponType.Sword:
                    currentPlayerStat.meleeAttackPower += equipmentItem.atk;
                    currentPlayerStat.meleeAttackSpeed += equipmentItem.atkSpeed;
                    inputController.EquipChangeSprite1();
                    isRanged = false;
                    break;
                case WeaponType.Spear:
                    currentPlayerStat.meleeAttackPower += equipmentItem.atk;
                    currentPlayerStat.meleeAttackSpeed += equipmentItem.atkSpeed;
                    inputController.EquipChangeSprite2();
                    isRanged = false;
                    break;
                case WeaponType.Shield:
                    currentPlayerStat.meleeAttackPower += equipmentItem.atk;
                    currentPlayerStat.meleeAttackSpeed += equipmentItem.atkSpeed;
                    inputController.EquipChangeSprite3();
                    isRanged = false;
                    break;
                case WeaponType.SemiAutomatic:
                    currentPlayerStat.rangeAttackPower += equipmentItem.atk;
                    currentPlayerStat.rangeAttackSpeed += equipmentItem.atkSpeed;
                    isRanged = true;
                    break;
                case WeaponType.FullyAutomatic:
                    currentPlayerStat.rangeAttackPower += equipmentItem.atk;
                    currentPlayerStat.rangeAttackSpeed += equipmentItem.atkSpeed;
                    isRanged = true;
                    break;
            }
        }
        else 
        {
            currentPlayerStat.meleeAttackPower += equipmentItem.atk;
            currentPlayerStat.meleeAttackSpeed += equipmentItem.atkSpeed;
            currentPlayerStat.rangeAttackPower += equipmentItem.atk;
            currentPlayerStat.rangeAttackSpeed += equipmentItem.atkSpeed;
        }
    }

    public void RemoveEquipmentStats(EquipmentItemSO equipmentItem)
    {
        currentPlayerStat.defense -= equipmentItem.def;
        currentPlayerStat.criticalChance -= equipmentItem.critChance;
        currentPlayerStat.criticalDamage -= equipmentItem.critDamage;
        currentPlayerStat.resistance -= equipmentItem.resistance;
        if (equipmentItem.equipmentType == EquipmentType.Weapon)
        {
            attakcRange = 0;
            weaponType = WeaponType.None;
            inputController.DeactivateWeaponSprite();
            switch (equipmentItem.weaponType)
            {
                case WeaponType.Sword:
                    currentPlayerStat.meleeAttackPower -= equipmentItem.atk;
                    currentPlayerStat.meleeAttackSpeed -= equipmentItem.atkSpeed;
                    isRanged = false;
                    break;
                case WeaponType.Spear:
                    currentPlayerStat.meleeAttackPower -= equipmentItem.atk;
                    currentPlayerStat.meleeAttackSpeed -= equipmentItem.atkSpeed;
                    isRanged = false;
                    break;
                case WeaponType.Shield:
                    currentPlayerStat.meleeAttackPower -= equipmentItem.atk;
                    currentPlayerStat.meleeAttackSpeed -= equipmentItem.atkSpeed;
                    isRanged = false;
                    break;
                case WeaponType.SemiAutomatic:
                    currentPlayerStat.rangeAttackPower -= equipmentItem.atk;
                    currentPlayerStat.rangeAttackSpeed -= equipmentItem.atkSpeed;
                    isRanged = false;
                    break;
                case WeaponType.FullyAutomatic:
                    currentPlayerStat.rangeAttackPower -= equipmentItem.atk;
                    currentPlayerStat.rangeAttackSpeed -= equipmentItem.atkSpeed;
                    isRanged = false;
                    break;
            }
        }
        else 
        {
            currentPlayerStat.meleeAttackPower -= equipmentItem.atk;
            currentPlayerStat.meleeAttackSpeed -= equipmentItem.atkSpeed;
            currentPlayerStat.rangeAttackPower -= equipmentItem.atk;
            currentPlayerStat.rangeAttackSpeed -= equipmentItem.atkSpeed;
        }
    }

    protected override void OnDeath()
    {
        Debug.Log("Player is dead");
    }
}
