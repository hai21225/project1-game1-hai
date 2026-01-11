using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    private void Start()
    {
        var character = GameManager.Instance.SelectedCharacter;
        var common = GameManager.Instance.PoolData;

        if (character == null)
        {
            Debug.LogError("No character selected!");
            return;
        }

        PoolManager.Instance.OnPoolInitFinished += LoadMainScene;
        // Init pool
        PoolManager.Instance.InitCharacterPools(character.PoolData.pools);
        PoolManager.Instance.InitCommonPools(common.pools);


    }


    private void LoadMainScene()
    {
        PoolManager.Instance.OnPoolInitFinished -= LoadMainScene;
        SceneManager.LoadScene("MainScene");
    }
}
