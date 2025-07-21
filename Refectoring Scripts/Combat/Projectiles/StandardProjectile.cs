using UnityEngine;

public class StandardProjectile : ProjectileBase
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    private float timer;

    private void Update()
    {
        if (IsFire)
        {
            transform.position += shootAngle * speed * Time.deltaTime;
            timer += Time.deltaTime;

            if (timer >= lifeTime)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(CalculateDamage());
        }

        Destroy(gameObject);
    }

    protected override void FireReady()
    {
        SetShootAngle();
        IsFire = true; 
    }
}
