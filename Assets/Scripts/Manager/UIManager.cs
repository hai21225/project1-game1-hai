using System.Collections;
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
    private bool _isDead=false;
    private Coroutine _showMenuCoroutine;

    private void Awake()
    {
        _canvas.SetActive(false);
        SoundManager.Instance.PlayGameBackGround();
    }
    public void SetCharacter(BaseCharacter character)
    {
        if (_character != null)
        {
            _character.OnDead -= ShowMenuOnDead;
        }

        _character = character;
        _character.OnDead += ShowMenuOnDead;
    }

    private void OnEnable()
    {
        _menu.OnClick += ShowMenu;
        _home.OnClick += Home;
        _continue.OnClick += Continue;
        _restart.OnClick += Restart;
    }
    private void OnDisable()
    {
        _menu.OnClick -= ShowMenu;
        _home.OnClick -= Home;
        _continue.OnClick -= Continue;
        _restart.OnClick -= Restart;
    }

    private void ShowMenu()
    {
        Time.timeScale = 0f;
        _canvas.SetActive(true);
    }

    private void ShowMenuOnDead()
    {
        if (_isDead) return;

        _isDead = true;

        if (_showMenuCoroutine != null)
            StopCoroutine(_showMenuCoroutine);

        _showMenuCoroutine = StartCoroutine(ShowMenuDelay());
    }

    private IEnumerator ShowMenuDelay()
    {
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 0f;
        _canvas.SetActive(true);
    }

    private void Continue()
    {
        if (_showMenuCoroutine != null)
        {
            StopCoroutine(_showMenuCoroutine);
            _showMenuCoroutine = null;
        }
        if (_isDead)
        { 
            Restart();
            return; 
        
        }
        Time.timeScale = 1f;
        _canvas.SetActive(false);
    }

    private void Restart()
    {
        if (_showMenuCoroutine != null)
        {
            StopCoroutine(_showMenuCoroutine);
            _showMenuCoroutine = null;
        }

        _character.ResetState();
        GameSession.instance.ResetSession();
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
        SceneManager.LoadScene("HomeScene");
    }

}