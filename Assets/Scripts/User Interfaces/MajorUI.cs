using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;
using System.Collections.Generic;

public class MajorUI : MonoBehaviour
{
    private enum ButtonType
    {
        Main_Start = 0,
        Main_Scores,
        Main_Exit,

        Score_Back,

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
    public class Container
    {
        public ContainerType Type;
        public GameObject Frame;

        public UnityEvent Opened;
        public UnityEvent Closed;
    }

    [field: Header("Assets - Main Menu")]
    [field: SerializeField] private Button StartButton;
    [field: SerializeField] private Button ScoresButton;
    [field: SerializeField] private Button ExitButton;

    [field: Header("Assets - Scoring")]
    [field: SerializeField] private Button BackButton;

    [field: Header("Assets - Saving")]
    [field: SerializeField] private TMP_InputField NameInput;
    [field: SerializeField] private Button FinishedButton;

    [field: Header("Assets - Containers")]
    [field: SerializeField] List<Container> Containers;

    private void ToggleContainers(ContainerType Target = ContainerType.None)
    {
        foreach (Container container in Containers)
        {
            if (container.Type != Target)
            {
                container.Frame?.SetActive(false);
                container.Closed?.Invoke();

                continue;
            }

            container.Frame?.SetActive(true);
            container.Opened?.Invoke();
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

            case ButtonType.Save_Finished:
                {
                    // WRITE SAVE USING `DataHandler.cs` HERE!
                    ToggleContainers(ContainerType.Title);
                }
                break;
        }
    }

    private void Start()
    {
        ToggleContainers(ContainerType.Title);

        StartButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Start));
        ScoresButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Scores));
        ExitButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Main_Exit));

        BackButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Score_Back));

        FinishedButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Save_Finished));
    }
}
