using System;
using UnityEngine;

class ButtonUI : MonoBehaviour
{
    private Action _click;

    public void SetupButton(Action onClick)
    {
        _click = onClick;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction);

            if (hit.Length == 0)
            {
                return;
            }

            foreach (RaycastHit2D h in hit)
            {
                if (h.collider != null && h.collider.gameObject == this.gameObject)
                {
                    _click();
                }
            }
        }
    }
}