using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    private IEnumerator Start()
    {
        var character = GameManager.Instance.SelectedCharacter;
        var common = GameManager.Instance.PoolData;

        if (character == null)
        {
            Debug.LogError("No character selected!");
            yield break;
        }

        // Init pool
        PoolManager.Instance.InitCharacterPools(character.PoolData.pools);
        PoolManager.Instance.InitCommonPools(common.pools);
        // waiting 1 frame to finish
        yield return null;

        
        SceneManager.LoadScene("MainScene");
    }
}
