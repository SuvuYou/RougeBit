using UnityEngine;

public class ShieldWeaponVisual : BaseWeaponVisual
{
    [SerializeField] private Transform _centerTransform;
    [SerializeField] private float _distanceFromCenter = 2f;

    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.3f; 

    protected override void Update()
    {
        _scanForProjectiles();

        base.Update();
    } 

    private void _scanForProjectiles()
    {
        if (ProjectileManager.Instance.TryGetClosestProjectile(transform.position, out BaseProjectile closestProjectile))
        {
            Vector3 newTarget = Vector3.SmoothDamp(_currentTarget, closestProjectile.transform.position, ref _velocity, smoothTime);

            newTarget = _clampOutsideCircle(newTarget);
            
            SetTarget(newTarget);

            transform.position = (newTarget - _centerTransform.position).normalized * _distanceFromCenter + _centerTransform.position;
        }
        else
        {
            Vector3 relativeTarget = _currentTarget - _centerTransform.position;
            Vector3 offset = Quaternion.Euler(0, 0, 180 * Time.deltaTime) * relativeTarget;

            Vector3 newTarget = _clampOutsideCircle(_centerTransform.position + offset);

            SetTarget(newTarget);
            transform.position = (newTarget - _centerTransform.position).normalized * _distanceFromCenter + _centerTransform.position;
        }
    }

    private Vector3 _clampOutsideCircle(Vector3 point)
    {
        Vector3 offset = point - _centerTransform.position;

        if (offset.magnitude == 0) offset = _getRandomTargetPosition();

        if (offset.magnitude < _distanceFromCenter)
        {
            return _centerTransform.position + offset.normalized * (_distanceFromCenter + 0.1f);
        }

        return point;
    }

    private Vector3 _getRandomTargetPosition() => _centerTransform.position + Random.insideUnitCircle.ToVector3WithZ(z: 0f) * _distanceFromCenter;


    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.transform.TryGetComponentInChildrenOfParent(out BaseProjectile projectile)) 
        {
           projectile.Redirect(speedMultiplier: 2f);
        }
    } 

    private void OnDrawGizmos()
    {
        // Draw target
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_currentTarget, 2f);
    }
}
