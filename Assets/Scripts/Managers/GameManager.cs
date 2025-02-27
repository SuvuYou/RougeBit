using UnityEngine;

class GameManager : Singlton<GameManager>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WaveManager.Instance.StartNextWave();
        } 
    }
}