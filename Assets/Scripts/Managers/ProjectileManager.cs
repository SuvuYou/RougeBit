using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileManager : Singlton<ProjectileManager>, IResettable
{
    public void Reset() => ClearProjectiles();

    private List<BaseProjectile> _projectiles = new();

    public void AddProjectile(BaseProjectile projectile) => _projectiles.Add(projectile);

    public bool TryGetClosestProjectile(Vector3 position, out BaseProjectile closestProjectile) 
    {
        _clearNullProjectiles();

        float minDistance = float.MaxValue;
        closestProjectile = null;

        foreach (var projectile in _projectiles) 
        {
            // If projectile flies away ignore it
            if (Vector3.Dot((position - projectile.transform.position), projectile.Direction) < 0) continue;

            float distance = Vector3.Distance(position, projectile.transform.position);

            if (distance < minDistance) 
            {
                minDistance = distance;
                closestProjectile = projectile;
            }
        }

        return closestProjectile != null;
    }

    public void ClearProjectiles() 
    {
        foreach (var projectile in _projectiles) Destroy(projectile.gameObject);
        
        _projectiles.Clear();
    }

    private void _clearNullProjectiles()
    {
        _projectiles = _projectiles.Where((projectile) => projectile != null).ToList();
    }
}
