using UnityEngine;

public interface IRotationStrategy
{
    public virtual void SetTargetPosition(Vector3 target) {}
    public virtual void Rotate() {}
}
