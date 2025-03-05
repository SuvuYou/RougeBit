using System.Collections;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemyComponent;   

    [SerializeField] private BaseAttack _attack;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private BaseWeapon _weapon;

    private void Start()
    {
        _attack.Setup(this.gameObject, _enemyComponent.Target);

        _attack.OnAttack.AddListener(() => StartCoroutine(_disableMovementForSeconds(0.5f)));
    
        if (_weapon != null) _weapon.SetTarget(_enemyComponent.Target);
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
}