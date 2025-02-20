using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Target { get; private set; }

    public void SetTarget(GameObject target) => Target = target;
}