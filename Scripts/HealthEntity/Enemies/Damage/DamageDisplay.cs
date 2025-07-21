using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    [SerializeField] private GameObject DamageTextPrefab;
    [SerializeField] private Transform DamagePivot;

    public void InstantiateDamageGUI(float damage, AttackBase attack)
    {
        DamageTextPrefab.GetComponent<DamageText>().damage = damage;
        Instantiate(DamageTextPrefab, DamagePivot.position, Quaternion.identity);
    }
}
