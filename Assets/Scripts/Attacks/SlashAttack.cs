using System;
using UnityEngine;

class SlashAttack : BaseAttack
{
    [SerializeField] private AttackAnimation _attackAnimation;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackDamage = 50f;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition + _attackDirection * 1.5f;

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _attackDistance) return;

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack) 
    {
        _updateAttackState();
        _spawnAttackAnimation();
        _handleCollision(); 

        finishAttack();
    }

    private void _updateAttackState()
    {
        if (!_isTargetFound) return;

        _attackDirection = (_target.transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;
    }

    private void _spawnAttackAnimation() 
    {
        var attackAnimation = Instantiate(_attackAnimation, _getSpawnPosition(), Quaternion.identity);
        attackAnimation.AttackIntoDirection(_attackDirection);
    }

    private void _handleCollision()
    {    
        var angle = _attackAnimation.transform.rotation.z;
        var colliders = Physics2D.OverlapBoxAll(_getSpawnPosition(), new Vector2(_spriteRenderer.bounds.size.x, _spriteRenderer.bounds.size.y), angle, _enemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            Transform parent = collider.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
            {
                damagable.TakeDamage(_attackDamage);
            }

            if (_addsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
            {
                knockable.AddKnockback(_attackDirection);
            }
        }
        
    }
}