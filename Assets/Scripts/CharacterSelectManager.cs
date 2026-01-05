using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;

    [SerializeField] private CharacterData[] characters;
    [SerializeField] private CharacterSelectItem itemPrefab;
    [SerializeField] private Transform contentParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var data in characters)
        {
            //Debug.Log("check1");
            var item = Instantiate(itemPrefab, contentParent);
            item.Init(data);
            //Debug.Log("check2");
        }
    }

    public void SelectCharacter(CharacterData data)
    {
        GameManager.Instance.SelectedCharacter = data.characterPrefab;
        SceneManager.LoadScene("LoadingScene");
    }
}
