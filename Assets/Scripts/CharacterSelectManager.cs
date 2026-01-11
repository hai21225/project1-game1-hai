using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;


    [SerializeField] private UiButton _confirmButton;
    [SerializeField] private CharacterData[] characters;
    [SerializeField]private CharacterSelectItem itemPrefab;
    [SerializeField] private Transform contentParent;


    [Header("Preview UI")]
    [SerializeField] private Image previewImage;


    private CharacterSelectItem _currentSelectedItem;

    private CharacterData _selectedData;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        previewImage.enabled = false;
        foreach (var data in characters)
        {
            //Debug.Log("check1");
            var item = Instantiate(itemPrefab, contentParent);
            item.Init(data);
            //Debug.Log("check2");
        }
    }

    private void OnEnable()
    {
        _confirmButton.OnClick += ConfirmSelection;
    }
    private void OnDisable()
    {
        _confirmButton.OnClick-= ConfirmSelection;
    }

    public void OnItemSelected(CharacterSelectItem item)
    {
        if (_currentSelectedItem != null)
            _currentSelectedItem.SetSelected(false);

        _currentSelectedItem = item;
        _currentSelectedItem.SetSelected(true);

        _selectedData = item.CharacterData;

        ShowPreview(_selectedData);
    }

    private void ShowPreview(CharacterData data)
    {
        previewImage.sprite = data.portrait;
        previewImage.enabled = true;
    }




    public void ConfirmSelection()
    {
        if (_selectedData == null)
            return;

        GameManager.Instance.SelectedCharacter = _selectedData.characterPrefab;
        SceneManager.LoadScene("LoadingScene");
    }

    //public void SelectCharacter(CharacterData data)
    //{
    //    GameManager.Instance.SelectedCharacter = data.characterPrefab;
    //    SceneManager.LoadScene("LoadingScene");
    //}
}
