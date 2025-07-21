using UnityEngine;

public class LaserProjectile : ProjectileBase
{
    [SerializeField] private float range = 10f;
    [SerializeField] private float width = 0.5f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private LayerMask blockMask;

    private RaycastHit[] hitBuffer = new RaycastHit[10];

    protected override void FireReady()
    {
        SetShootAngle();
        Vector3 origin = transform.position;
        Vector3 dir = shootAngle;

        float actualRange = range;
        if (Physics.Raycast(origin, dir, out RaycastHit blockHit, range, blockMask))
            actualRange = blockHit.distance;

        int hitCount = Physics.SphereCastNonAlloc(origin, width, dir, hitBuffer, actualRange, hitMask);
        for (int i = 0; i < hitCount; i++)
        {
            if (hitBuffer[i].collider.TryGetComponent(out Health health))
                health.TakeDamage(CalculateDamage());
        }

        Destroy(gameObject);
    }
}
