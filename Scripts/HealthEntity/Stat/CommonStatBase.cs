using UnityEngine;
[System.Serializable]
public class CommonStatBase
{
    [field: SerializeField, Header("오브젝트 공통 스탯")] public float HP { get; set; }
    [field: SerializeField] public float defense { get; set; }
}
