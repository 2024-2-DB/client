using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    [Header("캐릭터 오브젝트")]
    [SerializeField] private GameObject selectCharacter;
    [SerializeField] private GameObject unselectCharacter;

    [Header("메뉴 햄버거 버튼")]
    [SerializeField] private Button menuButton;
    
    public void DeactiveTagSelecter(GameObject _go)
    {
        _go.SetActive(false);

        selectCharacter.SetActive(false);
        unselectCharacter.SetActive(false);

        menuButton.gameObject.SetActive(false);
    }

    public void DeactiveGroupSelecter(GameObject _go)
    {
        _go.SetActive(false);
    }
}
