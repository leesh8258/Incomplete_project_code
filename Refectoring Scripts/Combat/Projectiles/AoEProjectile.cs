using UnityEngine;

public class AoEProjectile : ProjectileBase
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private LayerMask damageMask;

    private float timer;

    private void Update()
    {
        if (IsFire) 
        {
            transform.position += shootAngle * speed * Time.deltaTime;
            timer += Time.deltaTime;

            if (timer >= lifeTime)
            {
                Explode();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, buffer, damageMask);
        for (int i = 0; i < hitCount; i++)
        {
            if (buffer[i].TryGetComponent(out Health health))
                health.TakeDamage(CalculateDamage());
        }
    }

    protected override void FireReady() 
    {
        SetShootAngle();
        IsFire = true; 
    }
}
