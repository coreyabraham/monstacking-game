[System.Serializable]
public struct SavableData
{
    /// <summary>
    /// The amount of Time the player had the stack going for.
    /// </summary>
    public float time;

    /// <summary>
    /// The amount of objects stacked successfully without falling down.
    /// </summary>
    public int score;

    /// <summary>
    /// The name entered into the save for redistribution.
    /// </summary>
    public string name;
}