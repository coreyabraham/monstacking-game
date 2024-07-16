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

        Save_Finished
    }

    [System.Serializable]
    public enum ContainerType
    {
        None = 0,
        Title,
        Score,
        Save
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

    [field: Header("Assets - Main Menu")]
    [field: SerializeField] private Button StartButton;
    [field: SerializeField] private Button ScoresButton;
    [field: SerializeField] private Button ExitButton;

    [field: Header("Assets - Scoring")]
    [field: SerializeField] private Button BackButton;
    [field: SerializeField] private Button LeftButton;
    [field: SerializeField] private Button RightButton;

    [field: Space(2.5f)]

    [field: SerializeField] private GameObject ScoreListings;
    [field: SerializeField] private GameObject Template;

    [field: Header("Assets - Saving")]
    [field: SerializeField] private TMP_InputField NameInput;
    [field: SerializeField] private Button FinishedButton;

    [field: Header("Assets - Containers")]
    [field: SerializeField] List<Container> Containers;

    private List<ScoreTemplate> DataListings = new();
    private int DataListingIndex = 0;

    private GameStats cachedStats;

    public void GameHasStopped(GameStats gameStats)
    {
        print(cachedStats.PlayerScore);
        print(cachedStats.TimePlayedFor);

        cachedStats = gameStats;
        ToggleContainers(ContainerType.Save);
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
                    ToggleContainers();
                    GameHandler.Instance.StartGame();
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
                        name = NameInput.text,
                        score = cachedStats.PlayerScore,
                        time = cachedStats.TimePlayedFor
                    };

                    DataHandler.Instance.SaveToFile(data, data.name);
                    ToggleContainers(ContainerType.Title);
                }
                break;
        }
    }

    private void CycleListingIndex(bool IsRight)
    {
        // CYCLE INDEX AND VISUALS!
    }

    private void GenerateListings()
    {
        DataHandler.Instance.UpdateCachedFiles();

        foreach (SavableData data in DataHandler.Instance.CachedData)
        {
            // Read Data, then Clone a template, which'll be added to `DataListings`,  which'll then be readable via the Left and Right Score buttons!
            print(data.name);

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

            DataListings.Add(clone);
        }
    }

    private void Start()
    {
        GenerateListings();
        ToggleContainers(ContainerType.Title);

        StartButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Start));
        ScoresButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Scores));
        ExitButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Exit));

        BackButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Back));
        LeftButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Left));
        RightButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Right));

        FinishedButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Save_Finished));
    }
}
