using UnityEngine;
using UnityEngine.Events;

public class GameHandler : Singleton<GameHandler>
{
    [System.Serializable]
    public class GameEvents
    {
        public UnityEvent GameStartedEvent;
        public UnityEvent<GameStats> GameStoppedEvent;
    }

    [field: Header("Public Settings")]
    public bool AdjustVehicleSpeedOvertime = false;
    public float VehicleSpeed = 1.0f;

    [field: Header("Global Settings")]
    [field: SerializeField] private bool StartRunningOnStartup { get; set; } = false;
    [field: SerializeField] private float MaxTime { get; set; } = 180.0f;
    [field: SerializeField] private float MinVehicleSpeed = 1.0f;
    [field: SerializeField] private float MaxVehicleSpeed = 50.0f;

    [field: Header("Events")]
    public GameEvents Events = new();

    private float DefaultVehicleSpeed;

    private float TimeElapsed = 0.0f;
    private float TimePlayedFor = 0.0f;

    private int CurrentPlayerScore = 0;
    private bool GameCurrentlyRunning = false;

    protected override void Initialize()
    {
        DefaultVehicleSpeed = VehicleSpeed;
        if (!StartRunningOnStartup) return;
        StartGame();
    }

    public void StartGame()
    {
        if (GameCurrentlyRunning)
        {
            Debug.LogWarning("GameHandler.cs | The game is already running! Use `StopGame()` to stop the current game first!", this);
            return;
        }
        GameCurrentlyRunning = true;
        TimeElapsed = MaxTime;
        Events.GameStartedEvent?.Invoke();
    }

    public void StopGame()
    {
        if (!GameCurrentlyRunning)
        {
            Debug.LogWarning("GameHandler.cs | The game is NOT currently running! Use `StartGame` to run the game first!", this);
            return;
        }

        GameCurrentlyRunning = false;

        Debug.Log("GameHandler.cs | Stopping game!");

        GameStats gameStats = new()
        {
            PlayerScore = CurrentPlayerScore,
            TimePlayedFor = TimePlayedFor,
        };

        Events.GameStoppedEvent?.Invoke(gameStats);

        CurrentPlayerScore = 0;
        TimePlayedFor = 0.0f;
        TimeElapsed = 0.0f;

        VehicleSpeed = DefaultVehicleSpeed;
    }

    private void Update()
    {
        if (!GameCurrentlyRunning) return;
        if (TimeElapsed <= 0.0f) StopGame();

        TimeElapsed = Mathf.Clamp(TimeElapsed -= Time.deltaTime, 0.0f, MaxTime);
        TimePlayedFor += Time.deltaTime;

        if (!AdjustVehicleSpeedOvertime) return;
        VehicleSpeed = Mathf.Lerp(MinVehicleSpeed, MaxVehicleSpeed, Mathf.Clamp01(TimePlayedFor / MaxTime));
    }

    public bool IsGameRunning() => GameCurrentlyRunning;

    public float GetTimeElapsed() => TimeElapsed;
    public float GetTimePlayedFor() => TimePlayedFor;

    public int GetPlayerScore() => CurrentPlayerScore;
    public void SetPlayerScore(int Value) => CurrentPlayerScore = Value;

    public void SetMaxTime(float Time)
    {
        if (GameCurrentlyRunning) return;
        MaxTime = Time;
    }
}
