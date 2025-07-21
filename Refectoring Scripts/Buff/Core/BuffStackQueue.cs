using System;
using System.Collections.Generic;
using System.Linq;

public class BuffStackQueue
{
    private readonly List<BuffStackEntry> stack = new();
    public event Action OnExpired;

    public BuffStackEntry Current => stack.FirstOrDefault(); // 가장 강력한 버프

    public void Add(BuffStackEntry entry)
    {
        stack.Add(entry);
        Sort();
    }

    public bool Tick(float deltaTime)
    {
        bool removed = false;

        for (int i = 0; i < stack.Count; i++)
        {
            stack[i].Duration -= deltaTime;
        }

        int before = stack.Count;
        stack.RemoveAll(e => e.Duration <= 0f);
        if (stack.Count != before)
        {
            Sort();
            OnExpired?.Invoke();
            removed = true;
        }

        return removed;
    }

    private void Sort()
    {
        stack.Sort((a, b) =>
        {
            float aStrength = GetStrength(a);
            float bStrength = GetStrength(b);

            int compare = bStrength.CompareTo(aStrength);
            if (compare != 0)
                return compare;

            return b.Duration.CompareTo(a.Duration);
        });
    }

    private float GetStrength(BuffStackEntry entry)
    {
        return entry.FlatBonus + entry.PercentBonus * 100f;
    }
}
