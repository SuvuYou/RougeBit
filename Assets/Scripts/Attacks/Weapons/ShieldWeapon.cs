using UnityEngine;

public class ShieldWeapon : BaseWeapon
{
    [SerializeField] RotationController _rotationController;

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
        }
    } 
}
