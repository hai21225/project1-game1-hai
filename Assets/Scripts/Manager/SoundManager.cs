using UnityEngine;

public class SoundManager: MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _defaultAudio;
    [SerializeField] private AudioSource _gameBackGroundAudio;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void PlayDefault()
    {
        _defaultAudio.Play();
        _gameBackGroundAudio.Stop();
    }

    public void PlayGameBackGround()
    {
        _gameBackGroundAudio.Play();
        _defaultAudio.Stop();
    }


}