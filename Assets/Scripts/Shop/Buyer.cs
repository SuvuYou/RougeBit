using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Buyer : MonoBehaviour, IResettable
{
    public void Reset() 
    {
        foreach (var spawnedUpgradableItem in _spawnedUpgradableItems.Except(_initialSpawnedUpgradableItems))
        {
            Destroy(spawnedUpgradableItem.gameObject);
        }

        _spawnableUpgradableItems.Clear();
        _spawnedUpgradableItems.Clear();
        
        _spawnableUpgradableItems.AddRange(_initialSpawnableUpgradableItems);
        _spawnedUpgradableItems.AddRange(_initialSpawnedUpgradableItems);

        foreach (var spawnedUpgradableItem in _spawnedUpgradableItems)
        {
            spawnedUpgradableItem.Reset();
        }
    }

    [SerializeField] private List<UpgradablesBundleController> _initialSpawnableUpgradableItems = new ();
    [SerializeField] private List<UpgradablesBundleController> _initialSpawnedUpgradableItems = new ();

    private List<UpgradablesBundleController> _spawnableUpgradableItems = new ();
    private List<UpgradablesBundleController> _spawnedUpgradableItems = new ();

    [SerializeField] private Transform _spawnParent;

    [SerializeField] private PlayerAttack _playerAttack;

    private void Awake()
    {
        _spawnableUpgradableItems.AddRange(_initialSpawnableUpgradableItems);
        _spawnedUpgradableItems.AddRange(_initialSpawnedUpgradableItems);
    }

    public void Purchace(UpgradablesBundleController upgradableItem) 
    {
        if (_spawnableUpgradableItems.Contains(upgradableItem))
        {
            _spawnableUpgradableItems.Remove(upgradableItem);

            var spawnedUpgradableItem = Instantiate(upgradableItem, _spawnParent);

            if (spawnedUpgradableItem.gameObject.transform.TryGetComponentInChildren(out BaseWeapon weapon))
            {
                _playerAttack.AddWeapon(weapon);
            }

            _spawnedUpgradableItems.Add(spawnedUpgradableItem);

            spawnedUpgradableItem.Reset();
        }
        else
        {
            upgradableItem.Upgrade();
        }
    }

    public bool TrySelectRandomItemsToBuy(int numberOfItems, out List<UpgradablesBundleController> items)
    {
        List<UpgradablesBundleController> availableUpgradableItems = new();
        items = new List<UpgradablesBundleController>();

        availableUpgradableItems.AddRange(_spawnableUpgradableItems);
        availableUpgradableItems.AddRange(_spawnedUpgradableItems.Where(item => item.IsUpgradable));

        if (availableUpgradableItems.Count == 0 || numberOfItems <= 0) return false;

        numberOfItems = Mathf.Min(numberOfItems, availableUpgradableItems.Count);

        items = availableUpgradableItems.OrderBy(_ => Random.value).Take(numberOfItems).ToList();

        return items.Count > 0;
    }
}