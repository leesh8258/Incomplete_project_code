public static class BuffFactory
{
    public static Buff CreateBuff(BuffType type, float duration = 0)
    {
        switch (type)
        {
            case BuffType.PowerArmor:
                return new Buff(type, duration,
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if(enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsExpolosionResist = true;
                                stat.IsKnockbackResist = true;
                                stat.meleeAttackPower *= 1.2f;  
                                stat.defense *= 1.2f;
                            }

                        });
                    },
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsExpolosionResist = false;
                                stat.IsKnockbackResist = false;
                                stat.meleeAttackPower /= 1.2f;
                                stat.defense /= 1.2f;
                            }
                        });
                    });
            
            case BuffType.ExplosionResist:
                return new Buff(type, duration,
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsExpolosionResist = true;
                            }
                        });
                    },
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsExpolosionResist = false;
                            }
                        });
                    });

            case BuffType.Toughness:
                return new Buff(type, duration,
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsKnockbackResist = true;
                            }
                        });
                    },
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsKnockbackResist = false;
                            }
                        });
                    });

            case BuffType.Undying:
                return new Buff(type, duration,
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsUndying = true;
                            }
                        });
                    },
                    charater =>
                    {
                        charater.ApplyStatBoost(enemy =>
                        {
                            if (enemy.CharacterStat is EnemyStatBase stat)
                            {
                                stat.IsUndying = false;
                            }
                        });
                    });

            default:
                return new Buff(type, 0, null, null);
        }
    }
}
