using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXCleanerManager : Singlton<VFXCleanerManager>, IResettable
{
    public void Reset() => ClearVFXObjects();

    private List<GameObject> _vfxObjects = new();

    public void AddVFXObject(GameObject vfx) => _vfxObjects.Add(vfx);

    public void ClearVFXObjects() 
    {
        foreach (var vfx in _vfxObjects) Destroy(vfx);
        
        _vfxObjects.Clear();
    }

    private Timer _clearTimer = new (10f);

    private void Start() => _clearTimer.Start();

    private void Update()
    {
        _clearTimer.Update(Time.deltaTime);

        if (_clearTimer.IsFinished) 
        {
            _clearNullVFXObjects();

            _clearTimer.Reset();
        }
    }

    private void _clearNullVFXObjects()
    {
        _vfxObjects = _vfxObjects.Where((enemy) => enemy != null).ToList();
    }
}
