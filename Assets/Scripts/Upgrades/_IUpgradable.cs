using UnityEngine;

public class Upgradable<T> : MonoBehaviour where T : MonoBehaviour 
{
    public virtual void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues) { } 
}
