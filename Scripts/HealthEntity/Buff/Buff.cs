using System;

[System.Serializable]
public class Buff
{
    public BuffType type;
    public float duration;
    public Action<Character> applyEffect;
    public Action<Character> removeEffect;

    public Buff(BuffType type, float duration, Action<Character> applyEffect, Action<Character> removeEffect)
    {
        this.type = type;
        this.duration = duration;
        this.applyEffect = applyEffect;
        this.removeEffect = removeEffect;
    }
}
