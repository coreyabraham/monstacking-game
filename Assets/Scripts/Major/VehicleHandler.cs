using UnityEngine;

public class VehicleHandler : MonoBehaviour
{
    [field: Header("Settings - Spawning")]
    [field: SerializeField] private float IntervalBetweenSpawns { get; set; } = 1.0f;

    private bool debounce = false;

    private void FixedUpdate()
    {
        if (!GameHandler.Instance.IsGameRunning() || debounce) return;

        if (GameHandler.Instance.GetTimeElapsed() > 30.0f)
        {
            debounce = true;
            GameHandler.Instance.SetPlayerScore(1000);
            GameHandler.Instance.StopGame();
        }
    }
}
