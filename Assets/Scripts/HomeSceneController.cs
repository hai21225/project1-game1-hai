using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneController: MonoBehaviour
{

    [SerializeField] private UiButton[] _buttons;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
    }

    private void Start()
    {
        SoundManager.Instance.PlayDefault();
    }

    private void OnEnable()
    {
        _buttons[0].OnClick += LoadScene;
        _buttons[1].OnClick += LoadSceneSettings;
        _buttons[2].OnClick += Exit;
    }

    private void OnDisable()
    {
        _buttons[0].OnClick -= LoadScene;
        _buttons[1].OnClick -= LoadSceneSettings;
        _buttons[2].OnClick -= Exit;
    }
    private void LoadScene()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }
    private void Exit()
    {
        Application.Quit();
    }

    private void LoadSceneSettings()
    {
        // do it later
    }
}