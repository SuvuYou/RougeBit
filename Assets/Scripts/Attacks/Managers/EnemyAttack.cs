using System.Collections;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemyComponent;   

    [SerializeField] private BaseAttack _attack;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private FollowTargetRotationController _weaponRotation;

    private void Start()
    {
        _attack.Setup(this.gameObject);
        _attack.SetTarget(_enemyComponent.Target);

        _attack.OnAttack.AddListener((Vector3 targetPosition) => StartCoroutine(_disableMovementForSeconds(0.5f)));
    
        if (_weaponRotation != null) _weaponRotation.SetTarget(_enemyComponent.Target);
    }

    private void OnDestroy()
    {
        Destroy(_attack.gameObject);
    }

    private IEnumerator _disableMovementForSeconds(float seconds)
    {
        _movement.AddMovementLock(MovementLock.AttackStunned);
        yield return new WaitForSeconds(seconds);
        _movement.RemoveMovementLock(MovementLock.AttackStunned);
    }
}