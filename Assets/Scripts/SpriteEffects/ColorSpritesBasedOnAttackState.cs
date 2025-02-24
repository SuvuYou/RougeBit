using UnityEngine;

class ColorSpritesBasedOnAttackState : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BaseAttack _attack;

    [SerializeField] private AttackStateColorDictionary _colorByState = new ();

    private void Start()
    {
        _attack.OnAttack += () => spriteRenderer.color = _colorByState[BaseAttack.AttackState.Attacking];
        _attack.OnReload += () => spriteRenderer.color = _colorByState[BaseAttack.AttackState.Reload];
        _attack.OnAim += () => spriteRenderer.color = _colorByState[BaseAttack.AttackState.Aiming];
        _attack.OnReadyForAttack += () => spriteRenderer.color = _colorByState[BaseAttack.AttackState.ReadyForAttack];
    }
}