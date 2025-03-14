using System.Collections.Generic;

public class CollectablesManager : Singlton<CollectablesManager>
{
    private List<CollectableItem> _collectables = new();

    public void AddCollectableItem(CollectableItem collectable) => _collectables.Add(collectable);

    public void ClearCollectableItem() 
    {
        foreach (var collectable in _collectables) Destroy(collectable.gameObject);
        
        _collectables.Clear();
    }
}
