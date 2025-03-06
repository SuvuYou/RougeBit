using UnityEngine;

public class Target : MonoBehaviour
{
    public bool IsTargetable { get; private set; } = true;

    public void SetIsTargetable(bool isTargetable) => IsTargetable = isTargetable;
}