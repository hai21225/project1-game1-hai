using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BaseCharacter SelectedCharacter;
    public PoolData PoolData;

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
}
