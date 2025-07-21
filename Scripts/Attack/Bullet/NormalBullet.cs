using System.Collections;
using UnityEngine;

public class NormalBullet : BulletBase
{
    private void OnEnable()
    {
        StartCoroutine(DestroyAfterDuration());
    }

    private void Update()
    {
        BulletMove();
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(bulletDuration);
        Destroy(this.gameObject);
    }

    protected override void BulletMove()
    {
        transform.position += bulletToTargetDirection * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HealthyEntity>(out HealthyEntity component))
        {
            component.TakeDamageAction?.Invoke(attackBase.ReturnAtk(), attackBase);

            if (buffType != BuffType.Null && component is Character character)
            {
                character.ApplyTemporaryBuff(buffType, buffDuration);
            }

            if (!IsPierce) Destroy(this.gameObject); 
        }
    }
}
