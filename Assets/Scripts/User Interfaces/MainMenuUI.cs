using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public enum ButtonType
    {
        Start = 0,
        Scores,
        Exit
    }

    [field: Header("Assets")]
    [field: SerializeField] private GameObject MainContainer;

    [field: Space(5.0f)]

    [field: SerializeField] private Button StartButton;
    [field: SerializeField] private Button ScoresButton;
    [field: SerializeField] private Button ExitButton;

    private void ButtonClicked(ButtonType Type)
    {
        switch (Type)
        {
            case ButtonType.Start:
                {
                    MainContainer.SetActive(false);
                    GameHandler.Instance.StartGame();
                }
                break;
            
            case ButtonType.Scores:
                {
                    print("SHOW FILE DATAS HERE!");
                }
                break;
            
            case ButtonType.Exit:
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    Application.Quit(0);
                }
                break;
        }
    }

    private void Start()
    {
        StartButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Start));
        ScoresButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Scores));
        ExitButton?.onClick.AddListener(() => ButtonClicked(ButtonType.Exit));
    }
}
