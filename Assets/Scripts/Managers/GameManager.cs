using UnityEditor.Rendering;
using UnityEngine;

class GameManager : Singlton<GameManager>
{
    private Timer _waveTimer = new (0.5f);

    private void Start()
    {
        _waveTimer.Start();
    }

    private void Update()
    {
        _waveTimer.Update(Time.deltaTime);

        if (_waveTimer.IsRunning && _waveTimer.IsFinished)
        {
            WaveManager.Instance.StartNextWave();

            _waveTimer.Reset();
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     WaveManager.Instance.StartNextWave();
        // } 
    }
}