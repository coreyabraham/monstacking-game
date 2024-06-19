using UnityEngine;

public class GameHandler : Singleton<GameHandler>
{
    [field: Header("Global Settings")]
    [field: SerializeField] private bool StartRunningOnStartup { get; set; } = false;

    private float TimeElapsed = 0.0f;
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
        TimeElapsed = 0.0f;
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
        Debug.Log("Time Elapsed: " + TimeElapsed.ToString(), this);
        Debug.Log("Player Score: " + CurrentPlayerScore.ToString(), this);

        /*
            # TODO #
            1. Request Player Username (pull out virtual keyboard for this!)
            2. After the Player inputs their name, close the virtual keyboard input and save the file with that name to disk (score and time included from here obviously)
            3. ... i forgor 💀
        */
    }

    private void Update()
    {
        if (!GameCurrentlyRunning) return;
        TimeElapsed += Time.deltaTime;
    }

    public bool IsGameRunning() => GameCurrentlyRunning;
    public float GetTimeElapsed() => TimeElapsed;

    public int GetPlayerScore() => CurrentPlayerScore;
    public void SetPlayerScore(int Value) => CurrentPlayerScore = Value;
}
