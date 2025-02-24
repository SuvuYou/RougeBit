using System.Collections;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemyComponent;   

    [SerializeField] private BaseAttack _attack;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private AttackAnimationController _attackAnimationController;

    private void Start()
    {
        _attack.Setup(this.gameObject, _enemyComponent.Target);

        _attack.OnAttack += () => StartCoroutine(_disableMovementForSeconds(0.5f));
        _attack.OnAttack += _triggerAttackAnimation;
    }

    private void OnDestroy()
    {
        Destroy(_attack.gameObject);
    }

    private IEnumerator _disableMovementForSeconds(float seconds)
    {
        _movement.DisableMovement();
        yield return new WaitForSeconds(seconds);
        _movement.EnableMovement(); 
    }

    private void _triggerAttackAnimation()
    {
        var (target, isTargetFound) = _getTarget();

        if (!isTargetFound) return;

        _attackAnimationController.TriggerAttackAnimation((target.transform.position - transform.position).normalized);
    }

    private (GameObject, bool) _getTarget()
    {
        return (_enemyComponent.Target, _enemyComponent.Target != null);
    }
}