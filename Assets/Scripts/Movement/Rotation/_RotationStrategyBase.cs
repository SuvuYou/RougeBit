using UnityEngine;

public abstract class RotationStrategyBase : MonoBehaviour, IRotationStrategy
{
    public virtual void SetPivotPoint(Transform pivotPoint) { }
    public abstract void SetTargetPosition(Vector3 targetPosition);
    public abstract void Rotate();
}