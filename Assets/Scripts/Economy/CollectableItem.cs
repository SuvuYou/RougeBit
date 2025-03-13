using UnityEngine;

class CollectableItem : MonoBehaviour
{
    public int XPValue { get; private set; }

    public void Initialize(int xpValue) => XPValue = xpValue;
}