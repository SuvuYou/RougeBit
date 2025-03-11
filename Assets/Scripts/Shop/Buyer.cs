using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Buyer : MonoBehaviour
{
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

    public bool TrySelectRandomItemToBuy(out UpgradablesBundleController item)
    {
        List<UpgradablesBundleController> availableUpgradableItems = new ();
        item = null;

        availableUpgradableItems.AddRange(_spawnableUpgradableItems);
        availableUpgradableItems.AddRange(_spawnedUpgradableItems.Where(item => item.IsUpgradable));

        if (availableUpgradableItems.Count == 0) return false;

        item = availableUpgradableItems[Random.Range(0, availableUpgradableItems.Count)];;

        return true;
    }
}