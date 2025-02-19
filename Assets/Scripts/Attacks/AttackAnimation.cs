using System.Collections;
using UnityEngine;

class AttackAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        StartCoroutine(_destroyAfterSeconds(0.25f));
    }

    public void AttackIntoDirection(Vector3 attackDirection)
    {
        var isFlipped = attackDirection.x < 0;

        float sign, angle;

        if (isFlipped)
        {
            _spriteRenderer.flipX = true;

            sign = attackDirection.y > 0 ? -1 : 1;
            angle = Vector3.Angle(Vector3.left, attackDirection);
        }
        else
        {
            _spriteRenderer.flipX = false;

            sign = attackDirection.y < 0 ? -1 : 1;
            angle = Vector3.Angle(Vector3.right, attackDirection);       
        }

        transform.eulerAngles = Vector3.zero.With(z: sign * angle);
    }

    private IEnumerator _destroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}