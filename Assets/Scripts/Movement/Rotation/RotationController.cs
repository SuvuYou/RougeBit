using System.Collections.Generic;
using UnityEngine;

public class RotationController : Upgradable<MonoBehaviour>
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO upgradeValuesSet) => _smoothTime = upgradeValuesSet.RotationControllerUpgradeValues.SmoothTime;

    [Header("References")]
    [SerializeField] private List<RotationStrategyBase> _rotationStrategies;
    [SerializeField] private Transform _centerTransform;

    [Header("Clamp outside circle options")]
    [SerializeField] private bool _shouldClampOutsideCircle = true;
    [SerializeField] private float _distanceFromCenter = 0f;

    [Header("Smooth options")]
    [SerializeField] private bool _useSmoothRotation = true;
    [SerializeField] private float _smoothTime = 0.5f; 
    private Vector3 _velocity = Vector3.zero;

    [Header("Enable Idle Circular Motion")]
    [SerializeField] private bool _shouldEnableIdleCircularMotion = false;
    [SerializeField] private float _idleCircularMotionSpeed = 1f;
    public bool IsIdleRotationEnabled { get; private set; } = false;

    public void EnableIdleRotation() => IsIdleRotationEnabled = true;
    public void DisableIdleRotation() => IsIdleRotationEnabled = false;

    public bool IsRotationEnabled { get; private set; } = true;

    public void EnableRotation() => IsRotationEnabled = true;
    public void DisableRotation() => IsRotationEnabled = false;

    protected Vector3 _currentTarget;
    protected Vector3 _goalTarget;

    public void SetNewTarget(Vector3 newTarget)
    {
        _goalTarget = newTarget;
    }

    protected virtual void Update()
    {
        if (_shouldEnableIdleCircularMotion && IsIdleRotationEnabled) _stepToCircilarMotion();
        else _stepToGoalTarget();

        if (IsRotationEnabled && _currentTarget != null)
        {       
            foreach (RotationStrategyBase strategy in _rotationStrategies)
            {
                strategy.Rotate();
            }
        }
    } 

    private void _setCurrentTarget(Vector3 target) 
    {
        _currentTarget = target;

        foreach (RotationStrategyBase strategy in _rotationStrategies)
        {
            strategy.SetTargetPosition(target);
        }
    }

    private void _stepToGoalTarget()
    {
        Vector3 newTarget = _useSmoothRotation ? Vector3.SmoothDamp(_currentTarget, _goalTarget, ref _velocity, _smoothTime) : _goalTarget;

        if(_shouldClampOutsideCircle) newTarget = _clampOutsideCircle(newTarget);
        
        _setCurrentTarget(newTarget);
    }

    private void _stepToCircilarMotion()
    {
        Vector3 relativeTarget = _currentTarget - _centerTransform.position;
        Vector3 offset = Quaternion.Euler(0, 0, 90 * _idleCircularMotionSpeed * Time.deltaTime) * relativeTarget;

        Vector3 newTarget = _centerTransform.position + offset;

        if(_shouldClampOutsideCircle) newTarget = _clampOutsideCircle(newTarget);
        
        _setCurrentTarget(newTarget);
    }

    private Vector3 _clampOutsideCircle(Vector3 point)
    {
        Vector3 offset = point - _centerTransform.position;

        if (offset.magnitude == 0) offset = _getRandomTargetPosition();

        if (offset.magnitude < _distanceFromCenter)
        {            
            return _centerTransform.position + offset.normalized * (_distanceFromCenter + 0.1f);
        }

        return point;
    }

    private Vector3 _getRandomTargetPosition() => _centerTransform.position + Random.insideUnitCircle.ToVector3WithZ(z: 0f) * _distanceFromCenter;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_currentTarget, 2f);
    }
}
