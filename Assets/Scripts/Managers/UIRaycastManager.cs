using UnityEngine;

class UIRaycastManager : Singlton<UIRaycastManager>
{
    [SerializeField] private LayerMask _uiLayerMask;

    public RaycastHit2D[] UIElementHits;

    public bool IsElementHit => UIElementHits.Length > 0;

    public bool IsObjectHit(GameObject obj)
    {
        if (!IsElementHit) return false;

        foreach (RaycastHit2D uIElementHit in UIElementHits)
        {
            if (uIElementHit.collider != null && uIElementHit.collider.gameObject == obj)
            {
                return true;
            }
        }

        return false;
    } 

    private void Update()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPos = new (worldPoint.x, worldPoint.y);

        UIElementHits = Physics2D.RaycastAll(touchPos, Vector2.zero, 1000, _uiLayerMask);
    }
}