using UnityEngine;

public abstract class RotationStrategyBase : MonoBehaviour, IRotationStrategy
{ 
    public abstract void SetTargetPosition(Vector3 targetPosition);
    public abstract void Rotate();

    public virtual void SetPivotPoint(Transform pivotPoint) {}
}