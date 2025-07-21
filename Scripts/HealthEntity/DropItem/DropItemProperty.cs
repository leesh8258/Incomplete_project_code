using UnityEngine;

[System.Serializable]
public class DropItemProperty
{
    public GameObject itemPrefab;           // 아이템 프리팹
    public float dropChance;                // 드랍 확률 (%)
    public int minAmount;                   // 최소 드랍 개수
    public int maxAmount;                   // 최대 드랍 개수
}
