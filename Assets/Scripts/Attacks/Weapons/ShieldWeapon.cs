using UnityEngine;

public class ShieldWeapon : MonoBehaviour
{
    [SerializeField] RotationController _rotationController;

    protected void Update()
    {
        _scanForProjectiles();
    } 

    private void _scanForProjectiles()
    {
        if (ProjectileManager.Instance.TryGetClosestProjectile(transform.position, out BaseProjectile closestProjectile))
        {
            _rotationController.DisableIdleRotation();
            _rotationController.SetNewTarget(closestProjectile.transform.position);
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
           projectile.Redirect(speedMultiplier: 2f);
        }
    } 
}
