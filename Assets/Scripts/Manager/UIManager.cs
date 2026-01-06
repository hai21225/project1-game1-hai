using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private EnemySpawner[] _spawners;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private UiButton _menu;
    [SerializeField] private UiButton _home;
    [SerializeField] private UiButton _continue;
    [SerializeField] private UiButton _restart;

    private BaseCharacter _character;
    private bool _isDead;
    private void Awake()
    {
        _canvas.SetActive(false);
    }
    public void SetCharacter(BaseCharacter character)
    {
        if (_character != null)
        {
            _character.OnDead -= ShowMenu;
        }

        _character = character;
        _character.OnDead += ShowMenuOnDead;
    }

    private void Start()
    {
        _menu.OnClick += ShowMenu;
        _home.OnClick += Home;
        _continue.OnClick += Continue;
        _restart.OnClick += Restart;
    }
    private void ShowMenu()
    {
        Time.timeScale = 0f;
        _canvas.SetActive(true);
    }

    private void ShowMenuOnDead()
    {
        _isDead = true;
        float t = 10f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
        }
        Time.timeScale = 0f;
        _canvas.SetActive(true);
    }

    private void Continue()
    {
        if(_isDead)
        { 
            Restart();
            return; 
        
        }
        Time.timeScale = 1f;
        _canvas.SetActive(false);
    }

    private void Restart()
    {
        _character.ResetState();
        _isDead=false;

        foreach (var spawner in _spawners)
        {
            spawner.ResetSpawner();
        }
        Time.timeScale = 1f;
        _canvas.SetActive(false);
    }

    private void Home()
    {
        Time.timeScale = 1f;
        PoolManager.Instance.ClearGroup(PoolGroup.Common);
        PoolManager.Instance.ClearGroup(PoolGroup.Character);
        SceneManager.LoadScene("CharacterSelectScene");
    }

}