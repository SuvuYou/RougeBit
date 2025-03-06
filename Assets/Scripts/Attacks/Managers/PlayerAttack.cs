using System.Collections;
using UnityEngine;

class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BaseAttack _attack;
    [SerializeField] private PalyerInputSO _palyerInputSO;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private AttackAnimationController _attackAnimationController;

    private void Start()
    {
        (var enemy, var isEnemyFound) = _getTarget();
        _attack.Setup(this.gameObject, enemy);

        _attack.OnAttack.AddListener((Vector3 targetPosition) => StartCoroutine(_disableMovementForSeconds(0.05f)));
        _attack.OnAttack.AddListener((Vector3 targetPosition) => _triggerAttackAnimation());
    }

    private void Update()
    {
        (var enemy, var _) = _getTarget();
        _attack.SetTarget(enemy);
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

    private void _triggerAttackAnimation()
    {
        var (target, isTargetFound) = _getTarget();

        if (!isTargetFound) return;

        _attackAnimationController.TriggerAttackAnimation((target.transform.position - transform.position).normalized);
    }

    private (Target, bool) _getTarget()
    {
        bool isTargetFound = WaveManager.Instance.TryGetClosestEnemy(transform.position, out Enemy enemy);

        return (enemy, isTargetFound);
    }
}