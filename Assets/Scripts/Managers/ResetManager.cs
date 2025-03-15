using UnityEngine;
using System.Linq;

class ResetManager : Singlton<ResetManager>
{
    private IResettable[] resettables;

    private void Start()
    {
        resettables = FindObjectsOfType<MonoBehaviour>().OfType<IResettable>().ToArray();
    }

    public void Reset()
    {
        foreach (var obj in resettables)
        {
            obj.Reset();
        }
    }
}