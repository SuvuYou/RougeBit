using UnityEngine;

public class Singlton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

            return;
        }

        Destroy(gameObject);
    }
}