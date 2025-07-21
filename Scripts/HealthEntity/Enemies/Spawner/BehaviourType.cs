using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class BehaviourLayersAttribute : Attribute
{
    public string[] LayerNames { get; private set; }
    public BehaviourLayersAttribute(params string[] layerNames)
    {
        LayerNames = layerNames;
    }
}

public enum BehaviourType
{
    None,
    // 우선순위 순서대로 Layer 이름을 지정
    [BehaviourLayers("Center", "Structure", "Player", "Block")]
    StructureFirst,

    [BehaviourLayers("Player", "Center", "Structure", "Block")]
    PlayerFirst
}

public static class BehaviourTypeExtensions
{
    /// <summary>
    /// BehaviorType에 등록된 Layer 이름들을 LayerMask 배열로 반환
    /// 배열의 첫 번째 요소가 가장 높은 우선순위
    /// </summary>
    public static LayerMask[] GetLayerMasks(this BehaviourType behaviourType)
    {
        var type = typeof(BehaviourType);
        var memberInfo = type.GetMember(behaviourType.ToString());
        if (memberInfo != null && memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(BehaviourLayersAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                BehaviourLayersAttribute attr = (BehaviourLayersAttribute)attrs[0];
                string[] layerNames = attr.LayerNames;
                LayerMask[] masks = new LayerMask[layerNames.Length];
                for (int i = 0; i < layerNames.Length; i++)
                {
                    masks[i] = LayerMask.GetMask(layerNames[i]);
                }
                return masks;
            }
        }
        return new LayerMask[0];
    }
}
