using UnityEngine;
using UnityEngine.Events;

public class ShieldWeapon : BaseWeapon
{
    [SerializeField] RotationController _rotationController;

    public Vector3QuaternionUnityEvent OnRedirect;

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask) 
    {
        base.Setup(attacker, enemyLayerMask);

        _rotationController.SetupPivotPoint(attacker.transform);
    }

    public override void SetTarget(Target targetPosition, bool isTargetFound = false)
    {
        if (isTargetFound)
        {
            _rotationController.DisableIdleRotation();
            _rotationController.SetNewTarget(targetPosition.transform.position);
        }
        else
        {
            _rotationController.EnableIdleRotation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.transform.TryGetComponentInChildrenOfParent(out BaseProjectile projectile))
        {
            if (projectile.CurrentVarient == BaseProjectile.Varients.Player && WeaponTargetType == TargetType.EnemyProjectile) return;
            if (projectile.CurrentVarient == BaseProjectile.Varients.Enemy && WeaponTargetType == TargetType.PlayerProjectile) return;

           projectile.Redirect(speedMultiplier: 2f);

           var projectileLookDirection = projectile.Direction;
           var angle = Mathf.Atan2(projectileLookDirection.y, projectileLookDirection.x) * Mathf.Rad2Deg;
           var quaternionRotation = Quaternion.Euler(0, 0, angle);

           OnRedirect?.Invoke(projectile.gameObject.transform.position, quaternionRotation);
        }
    } 
}
