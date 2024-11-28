//System
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TMPro;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TagSelecter : MonoBehaviour
{
    [Header("Chat Manager")]
    //[SerializeField] private ChatManager chat;

    [Header("��ȭ�Ϸ� ���� ��ư")]
    [SerializeField] private Button conversationButton;

    [Header("�±� ��ư ����Ʈ")]
    [SerializeField] private List<Button> tagButtons;

    [Header("ĳ���� ������Ʈ")]
    [SerializeField] private GameObject selectCharacter;
    [SerializeField] private GameObject unselectCharacter;

    private string currentTag = "";

    private string groupName = "";

    private void Start()
    {
        UpdateTag();
        UpdateConverButton();
    }

    private void OnEnable()
    {
        UpdateTag();
        UpdateConverButton();
    }

    public void UpdateTag()
    {
        foreach (Button button in tagButtons)
        {
            //�� ��ư�� �̺�Ʈ �߰�
            button.onClick.AddListener(() => SelectTag(button));
        }
    }

    public void AddTagButton(Button _button)
    {
        tagButtons.Add(_button);
    }

    public void SetGroup(string _groupName)
    {
        groupName = _groupName;
    }

    private void UpdateConverButton()
    {
        conversationButton.onClick.AddListener(CompleteSelectTag);
    }

    private void SelectTag(Button _button)
    {
        FeedTag feedTag = _button.transform.GetComponent<FeedTag>();

        if (feedTag != null) currentTag = feedTag.tagName;

        _button.interactable = false;

        ActiveAllButtons(_button);

        selectCharacter.SetActive(true);
        unselectCharacter.SetActive(false);
    }

    private void ActiveAllButtons(Button _button)
    {
        foreach (Button button in tagButtons)
        {
            if (button != _button)
                button.interactable = true;
        }
    }

    private void CompleteSelectTag()
    {
        //������Ʈ Ȱ��ȭ
        //chat.gameObject.SetActive(true);

        //���� �±� ����
        //chat.Init(currentTag);

        UIManager.instance.DeactiveTagSelecter(gameObject);
    }
}
