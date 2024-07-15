using UnityEngine;

public class GameHandler : Singleton<GameHandler>
{
    [field: Header("Global Settings")]
    [field: SerializeField] private bool StartRunningOnStartup { get; set; } = false;
    [field: SerializeField] private float MaxTime { get; set; } = 180.0f;

    private float TimeElapsed = 0.0f;
    private float TimePlayedFor = 0.0f;

    private int CurrentPlayerScore = 0;
    private bool GameCurrentlyRunning = false;

    protected override void Initialize()
    {
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
    }

    public void StopGame()
    {
        if (!GameCurrentlyRunning)
        {
            Debug.LogWarning("GameHandler.cs | The game is NOT currently running! Use `StartGame` to run the game first!", this);
            return;
        }

        GameCurrentlyRunning = false;

        Debug.Log("GameHandler.cs | Stopping game! Data collected is displayed below:");
        Debug.Log("Player Score: " + CurrentPlayerScore.ToString(), this);
        Debug.Log("Time Played For: " + TimePlayedFor.ToString(), this);

        // MajorUI Should now open the "SaveContainer" Frame and have the player type in a name to save the data!
    }

    private void Update()
    {
        if (!GameCurrentlyRunning) return;
        if (TimeElapsed <= 0.0f) StopGame();

        TimeElapsed = Mathf.Clamp(TimeElapsed -= Time.deltaTime, 0.0f, MaxTime);
        TimePlayedFor += Time.deltaTime;
    }

    public bool IsGameRunning() => GameCurrentlyRunning;

    public float GetTimeElapsed() => TimeElapsed;
    public float GetTimePlayedFor() => TimePlayedFor;

    public int GetPlayerScore() => CurrentPlayerScore;
    public void SetPlayerScore(int Value) => CurrentPlayerScore = Value;
}
