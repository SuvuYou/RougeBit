using System.Collections;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemyComponent;   

    [SerializeField] private BaseWeapon[] _weapons;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private LayerMask _enemyLayerMask;

    private void Start()
    {
        _setupAttacks();
        _setAttacksTarget();
    }
    
    private void Update()
    {
        _setAttacksTarget();
    }

    private void _setAttacksTarget()
    {  
        foreach (var weapon in _weapons)
        {
            weapon.SetTarget(_enemyComponent.Target, isTargetFound: _enemyComponent.Target != null);
        }
    }

    private void _setupAttacks()
    {
        foreach (var weapon in _weapons) 
        {
            weapon.Setup(this.gameObject, _enemyLayerMask);
        }
    }

    private IEnumerator _disableMovementForSeconds(float seconds)
    {
        _movement.AddMovementLock(MovementLock.AttackStunned);
        yield return new WaitForSeconds(seconds);
        _movement.RemoveMovementLock(MovementLock.AttackStunned);
    }
}