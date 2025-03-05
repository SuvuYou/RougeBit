using System.Collections;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _attacker;
    [SerializeField] private LayerMask _enemyLayerMask;

    [Header("Shockwave Settings")]
    [SerializeField] private float _maxKnockbackRadius = 5f;
    [SerializeField] private float _knockbackStepDelay = 0.1f;
    [SerializeField] private float _knockbackStep = 0.5f;
    [SerializeField] private float _knockbackForce = 0.1f;

    public void SendShockwave() => StartCoroutine(_sendKnockbackCoroutine(_maxKnockbackRadius, _knockbackStepDelay, _knockbackStep));

    private IEnumerator _sendKnockbackCoroutine(float maxKnockbackRadius, float knockbackStepDelay, float knockbackStep)
    {
        Vector3 shockwaveCenter = _attacker.position;

        float minKnockbackRadius = 0f;
        float currentKnockbackRadius = minKnockbackRadius;

        while (currentKnockbackRadius < maxKnockbackRadius)
        {
            _searchForKnockbackColliders(shockwaveCenter, currentKnockbackRadius);

            yield return new WaitForSeconds(knockbackStepDelay);

            currentKnockbackRadius += knockbackStep;
        }
    }

    private void _searchForKnockbackColliders(Vector3 shockwaveCenter, float radius) 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(shockwaveCenter, radius, _enemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            Transform parent = collider.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out BaseKnockback knockable))
            {
                knockable.AddKnockback(parent.position - shockwaveCenter, _knockbackForce);
            }
        }
    }
}
