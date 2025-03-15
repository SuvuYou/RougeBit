using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public bool IsCollectable = true;
    public int XPValue { get; private set; }

    public void Initialize(int xpValue) => XPValue = xpValue;

    public void Collect() 
    {
        IsCollectable = false;
        Destroy(gameObject);
    }
}