using System.Collections;
using System.Linq;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private BaseAttack _attack;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private AttackAnimationController _attackAnimationController;

    private void Start()
    {
        var attack = Instantiate(_attack);
        attack.Setup(this.gameObject, _getTarget);

        attack.OnAttack += () => StartCoroutine(_disableMovementForSeconds(0.05f));
        attack.OnAttack += () => _attackAnimationController.TriggerAttackAnimation((_getTarget().transform.position - transform.position).normalized);
    }

    private IEnumerator _disableMovementForSeconds(float seconds)
    {
        _movement.DisableMovement();
        yield return new WaitForSeconds(seconds);
        _movement.EnableMovement(); 
    }

    private GameObject _getTarget()
    {
        return FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None)?.First().gameObject;
    }
}