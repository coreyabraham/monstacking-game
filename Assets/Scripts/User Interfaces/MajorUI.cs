using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class MajorUI : MonoBehaviour
{
    private enum ButtonType
    {
        Main_Start = 0,
        Main_Scores,
        Main_Exit,

        Score_Back,
        Score_Left,
        Score_Right,

        Save_Finished,

        Time_Left,
        Time_Right,
        Time_Back,
        Time_Speed,
        Time_Finalized
    }

    [System.Serializable]
    public enum ContainerType
    {
        None = 0,
        Title,
        Score,
        Save,
        Time
    }

    [System.Serializable]
    public class MajorUIEvents
    {
        public UnityEvent Opened;
        public UnityEvent Closed;
    }

    [System.Serializable]
    public class Container
    {
        public ContainerType Type;
        public GameObject Frame;
        public MajorUIEvents Events;
    }

    private struct KeyboardKey
    {
        public char Key;
        public Button Button;
    }

    [field: Header("Assets - Main Menu")]
    [field: SerializeField] private Button StartButton;
    [field: SerializeField] private Button ScoresButton;
    [field: SerializeField] private Button ExitButton;

    [field: Header("Assets - Scoring")]
    [field: SerializeField] private Button ScoreBackButton;
    [field: SerializeField] private Button ScoreLeftButton;
    [field: SerializeField] private Button ScoreRightButton;

    [field: Space(2.5f)]

    [field: SerializeField] private TMP_Text NoDataLabel;
    [field: SerializeField] private GameObject ScoreListings;
    [field: SerializeField] private GameObject Template;

    [field: Header("Assets - Saving")]
    [field: SerializeField] private TMP_Text NameLabel;
    [field: SerializeField] private GameObject KeyboardKeysParent;
    [field: SerializeField] private Button KeyboardKeyTemplate;
    [field: SerializeField] private Button FinishedButton;

    [field: Header("Assets - Time Selection")]
    [field: SerializeField] private TMP_Text TimeLabel;

    [field: Space(2.5f)]
    
    [field: SerializeField] private Button TimeSpeedButton;
    [field: SerializeField] private Button TimeLeftButton;
    [field: SerializeField] private Button TimeRightButton;
    [field: SerializeField] private Button TimeBackButton;
    [field: SerializeField] private Button TimeStartButton;

    [field: Header("Assets - Miscellaneous")]
    [field: SerializeField] private int MaxMinutesSetable = 10;
    [field: SerializeField] List<Container> Containers;

    private List<ScoreTemplate> DataListings = new();
    private readonly char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ<".ToCharArray();

    private int DataListingIndex = 0;
    private int MinutesSelection = 1;

    private GameStats cachedStats;

    public void GameHasStopped(GameStats GameStats)
    {
        print(cachedStats.PlayerScore);
        print(cachedStats.TimePlayedFor);

        cachedStats = GameStats;
        ToggleContainers(ContainerType.Save);
    }

    public void ScoreContainerToggled()
    {
        bool isVisible = DataListings.Count <= 0;
        
        NoDataLabel.gameObject.SetActive(isVisible);
        ScoreLeftButton.gameObject.SetActive(!isVisible);
        ScoreRightButton.gameObject.SetActive(!isVisible);
    }

    private void CycleListingIndex(bool IsRight)
    {
        if (IsRight) DataListingIndex++;
        else DataListingIndex--;

        DataListingIndex = Mathf.Clamp(DataListingIndex, 0, DataListings.Count - 1);

        ScoreTemplate Index = DataListings[DataListingIndex];
        if (!Index) return;

        Index.gameObject.SetActive(true);

        foreach (ScoreTemplate listing in DataListings)
        {
            if (listing == Template || listing == Index) continue;
            listing.gameObject.SetActive(false);
        }
    }

    private void CycleTimeSelection(bool IsRight)
    {
        if (IsRight) MinutesSelection += 1;
        else MinutesSelection -= 1;
        
        MinutesSelection = Mathf.Clamp(MinutesSelection, 1, MaxMinutesSetable);

        string MinuteText = MinutesSelection.ToString();
        if (MinutesSelection < 10) MinuteText = "0" + MinutesSelection.ToString();

        TimeLabel.text = "Game Length: " + MinuteText + ":00";
    }

    private void ToggleContainers(ContainerType Target = ContainerType.None)
    {
        foreach (Container container in Containers)
        {
            if (container.Type != Target)
            {
                container.Frame?.SetActive(false);
                container.Events.Closed?.Invoke();

                continue;
            }

            container.Frame?.SetActive(true);
            container.Events.Opened?.Invoke();
        }
    }

    private void ButtonClicked(ButtonType Type)
    {
        switch (Type)
        {
            case ButtonType.Main_Start:
                {
                    ToggleContainers(ContainerType.Time);
                }
                break;
            
            case ButtonType.Main_Scores:
                {
                    ToggleContainers(ContainerType.Score);
                }
                break;
            
            case ButtonType.Main_Exit:
                {
                    ToggleContainers();

#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    Application.Quit(0);
                }
                break;

            case ButtonType.Score_Back:
                {
                    ToggleContainers(ContainerType.Title);
                }
                break;

            case ButtonType.Score_Left:
                {
                    CycleListingIndex(false);
                }
                break;

            case ButtonType.Score_Right:
                {
                    CycleListingIndex(true);
                }
                break;

            case ButtonType.Save_Finished:
                {
                    SavableData data = new()
                    {
                        name = NameLabel.text,
                        score = cachedStats.PlayerScore,
                        time = cachedStats.TimePlayedFor
                    };

                    DataHandler.Instance.SaveToFile(data, data.name);
                    ToggleContainers(ContainerType.Title);
                    GenerateScoreListings();
                }
                break;

            case ButtonType.Time_Left:
                {
                    CycleTimeSelection(false);
                }
                break;

            case ButtonType.Time_Right:
                {
                    CycleTimeSelection(true);
                }
                break;

            case ButtonType.Time_Back:
                {
                    ToggleContainers(ContainerType.Title);
                }
                break;

            case ButtonType.Time_Speed:
                {
                    GameHandler.Instance.AdjustVehicleSpeedOvertime = !GameHandler.Instance.AdjustVehicleSpeedOvertime;
                    TMP_Text label = TimeSpeedButton.GetComponentInChildren<TMP_Text>();
                    label.text = "Faster Speeds: [" + GameHandler.Instance.AdjustVehicleSpeedOvertime.ToString() + "]";
                }
                break;

            case ButtonType.Time_Finalized:
                {
                    ToggleContainers();

                    float Seconds = MinutesSelection * 60;
                    GameHandler.Instance.SetMaxTime(Seconds);
                    GameHandler.Instance.StartGame();
                }
                break;
        }
    }

    private void GenerateScoreListings()
    {
        DataHandler.Instance.UpdateCachedFiles();

        foreach (SavableData data in DataHandler.Instance.CachedData)
        {
            GameObject obj = Instantiate(Template);
            ScoreTemplate clone = obj.GetComponent<ScoreTemplate>();

            if (!clone) continue;

            clone.name = data.name;
            clone.NameLabel.text = "Name: " + data.name;
            clone.ScoreLabel.text = "Score: " + data.score.ToString();

            float minutes = Mathf.Floor(data.time / 60);
            float seconds = Mathf.Round(data.time % 60);

            string minutesText = minutes.ToString();
            string secondsText = seconds.ToString();

            if (minutes < 10) minutesText = "0" + minutes.ToString();
            if (seconds < 10) secondsText = "0" + Mathf.Round(seconds).ToString();

            clone.TimeLabel.text = "Time Played: " + minutesText + ":" + secondsText;

            obj.transform.SetParent(ScoreListings.transform);

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);

            obj.transform.localPosition = Vector3.zero;

            if (DataListings.Count > 0)
            {
                bool foundThisClone = false;

                foreach (ScoreTemplate score in DataListings)
                {
                    if (score != clone) continue;
                    foundThisClone = true;
                }

                if (foundThisClone) continue;
            }

            DataListings.Add(clone);
        }

        if (DataListings.Count == 0) return;

        ScoreTemplate first = DataListings[DataListingIndex];
        if (!first) return;
        first.gameObject.SetActive(true);
    }

    private void KeyboardKeyInput(KeyboardKey key)
    {
        if (key.Key == '<' && NameLabel.text.Length > 0)
        {
            NameLabel.text = NameLabel.text.Remove(NameLabel.text.Length - 1);
            return;
        }

        NameLabel.text += key.Key.ToString();
    }

    private void GenerateKeyboardKeys()
    {
        foreach (char alpha in Alphabet)
        {
            Button clone = Instantiate(KeyboardKeyTemplate);
            clone.gameObject.SetActive(true);

            TMP_Text label = clone.GetComponentInChildren<TMP_Text>();
            label.text = alpha.ToString();

            clone.transform.SetParent(KeyboardKeysParent.transform);
            clone.transform.localPosition = Vector3.zero;

            KeyboardKey key = new()
            {
                Key = alpha,
                Button = clone
            };

            key.Button.onClick.AddListener(() => KeyboardKeyInput(key));
        }
    }

    private void Start()
    {
        GenerateScoreListings();
        GenerateKeyboardKeys();

        ToggleContainers(ContainerType.Title);
        CycleTimeSelection(false);

        StartButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Start));
        ScoresButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Scores));
        ExitButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Exit));

        ScoreBackButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Back));
        ScoreLeftButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Left));
        ScoreRightButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Right));

        FinishedButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Save_Finished));

        TimeLeftButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Time_Left));
        TimeRightButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Time_Right));
        TimeBackButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Time_Back));
        TimeSpeedButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Time_Speed));
        TimeStartButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Time_Finalized));
    }
}
