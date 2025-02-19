using System.Collections;
using System.Linq;
using UnityEngine;

class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BaseAttack _attack;
    [SerializeField] private PalyerInputSO _palyerInputSO;
    [SerializeField] private CharacterMovement _movement;

    private void Start()
    {
        var attack = Instantiate(_attack);
        attack.Setup(this.gameObject, () => FindObjectsByType<FollowTargetMovement>(FindObjectsSortMode.None)?.First().gameObject);

        attack.OnAttack += () => StartCoroutine(_disableAttackForSeconds(0.05f));
    }

    private IEnumerator _disableAttackForSeconds(float seconds)
    {
        _movement.DisableMovement();
        yield return new WaitForSeconds(seconds);
        _movement.EnableMovement();
        
    }
}