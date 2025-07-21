using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    private Dictionary<BuffType, Buff> activeBuffs = new Dictionary<BuffType, Buff>();
    private Dictionary<BuffType, Coroutine> coroutineReferences = new Dictionary<BuffType, Coroutine>();
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void AddBuff(Buff newBuff)
    {
        if(activeBuffs.TryGetValue(newBuff.type, out Buff existingBuff))
        {
            if (coroutineReferences.TryGetValue(newBuff.type, out Coroutine existingCoroutine))
            {
                StopCoroutine(existingCoroutine); // 기존 코루틴 중지
            }

            existingBuff.duration = newBuff.duration;
            Coroutine newCoroutine = StartCoroutine(RemoveBuffAfterDelay(existingBuff));
            coroutineReferences[newBuff.type] = newCoroutine;
            
            Debug.Log($"버프 {newBuff.type} 갱신 (새 duration: {newBuff.duration})");
        }

        else
        {
            activeBuffs[newBuff.type] = newBuff;
            newBuff.applyEffect(character);
           
            if (newBuff.duration > 0)
            {
                Coroutine newCoroutine = StartCoroutine(RemoveBuffAfterDelay(newBuff));
                coroutineReferences[newBuff.type] = newCoroutine;
            }

            Debug.Log($"버프 {newBuff.type} 추가");
        }


    }

    private IEnumerator RemoveBuffAfterDelay(Buff buff)
    {
        yield return new WaitForSeconds(buff.duration);

        buff.removeEffect(character);
        activeBuffs.Remove(buff.type);
        coroutineReferences.Remove(buff.type);

        Debug.Log($"버프 {buff.type} 제거");
    }
}
