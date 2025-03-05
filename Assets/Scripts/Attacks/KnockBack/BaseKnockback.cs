using UnityEngine;

class BaseKnockback : MonoBehaviour
{
    [SerializeField] protected Transform _entity;
    [SerializeField] private CharacterMovement _characterMovement; 

    [SerializeField] float _knockbackSpeed = 0.1f;
    [SerializeField] float _precision = 0.1f;

    private bool _isEnabled = false;
    private Vector3 _knockbackTargetPosition;
    private float _elapsedTime = 0f;

    public void Update()
    {
        if (!_isEnabled) return;

        // Smooth step
        float t = _elapsedTime / _knockbackSpeed;
        t = t * t * (3f - 2f * t);

        _entity.position = Vector3.Lerp(_entity.position, _knockbackTargetPosition, t);
        _elapsedTime += Time.deltaTime;

        if (Vector3.Distance(_entity.position, _knockbackTargetPosition) <= _precision)
        {
            _entity.position = _knockbackTargetPosition;
            _isEnabled = false;
            _elapsedTime = 0;

            _characterMovement?.RemoveMovementLock(MovementLock.Knockback);
        }
    }

    public virtual void AddKnockback(Vector3 direction, float strength = 2f) 
    {
        _isEnabled = true;

        _characterMovement?.AddMovementLock(MovementLock.Knockback);

        _knockbackTargetPosition = _entity.position + direction.normalized * strength;
    }
}