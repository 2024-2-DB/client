//Unity
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    [DisallowMultipleComponent]
    public class Feed : MonoBehaviour
    {
        [Header("��� ��ư")]

        [Header("�̽� �÷ο�")]
        [SerializeField] private Button issueAllViewButton;

        [Header("�� �̺�Ʈ")]

        [Header("�׺���̼ǹ� �ϴ� ��ư")]
        [SerializeField] private Button feedButton;
        [SerializeField] private Button chatButton;
        [SerializeField] private Button notiButton;
        [SerializeField] private Button myButton;

        private void Start()
        {
            Init();
            
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            issueAllViewButton.onClick.AddListener(UIController.instance.EnableIssuePanel);

            chatButton.onClick.AddListener(UIController.instance.EnableChatPanel);
        }

    }
}

