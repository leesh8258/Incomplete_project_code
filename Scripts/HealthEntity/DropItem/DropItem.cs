using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private bool isDropAll;          // 드랍 방식: OR (false) / AND (true)
    [SerializeField] private List<DropItemProperty> dropItems; // 드랍 아이템 목록 이것도 cs 분리시키자 지금하자

    private void DropItems()
    {
        if (dropItems == null || dropItems.Count == 0)
        {
            Debug.LogWarning("DropItems가 비어 있습니다.");
            return;
        }

        if (isDropAll)
        {
            // AND 모드: 한 개의 아이템만 선택 후 확률 계산
            var selectedItem = GetRandomItem(dropItems);
            if (selectedItem != null)
            {
                TryDropItem(selectedItem);
            }
        }

        else
        {
            // OR 모드: 모든 아이템 개별 확률 계산
            foreach (var dropItem in dropItems)
            {
                TryDropItem(dropItem);
            }
        }
    }

    private void TryDropItem(DropItemProperty dropItem)
    {
        float randomChance = Random.Range(0f, 100f);
        if (randomChance <= dropItem.dropChance)
        {
            int dropAmount = Random.Range(dropItem.minAmount, dropItem.maxAmount + 1);

            // 아이템 프리팹 생성
            Debug.Log($"아이템 {dropItem.itemPrefab.name} 드롭됨! 수량: {dropAmount}");
        }
    }

    private DropItemProperty GetRandomItem(List<DropItemProperty> dropItems)
    {
        if (dropItems == null || dropItems.Count == 0) return null;

        int randomIndex = Random.Range(0, dropItems.Count);
        return dropItems[randomIndex];
    }

}
