using UnityEngine;

public class ShieldWeaponVisual : BaseWeaponVisual
{
    [SerializeField] private Transform _centerTransform;
    [SerializeField] private float _distanceFromCenter = 2f;

    protected override void Update()
    {
        _scanForProjectiles();

        base.Update();
    } 

    private void _scanForProjectiles()
    {
        if (ProjectileManager.Instance.TryGetClosestProjectile(transform.position, out BaseProjectile closestProjectile))
        {
            Vector3 newTarget = Vector3.Lerp(_currentTarget, closestProjectile.transform.position, Time.deltaTime);

            SetTarget(newTarget);

            transform.position = (newTarget - _centerTransform.position).normalized * _distanceFromCenter + _centerTransform.position;
        }
        else
        {
            Vector3 relativeTarget = _currentTarget - _centerTransform.position;
            Vector3 offset = Quaternion.Euler(0, 0, 180 * Time.deltaTime) * relativeTarget;
            Vector3 newTarget = _centerTransform.position + offset;

            SetTarget(newTarget);

            transform.position = (newTarget - _centerTransform.position).normalized * _distanceFromCenter + _centerTransform.position;
        }
    }
}
