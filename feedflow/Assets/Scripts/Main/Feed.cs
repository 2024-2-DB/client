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

        [Header("�� Ŀ���� �±�")]
        [SerializeField] private Button tagAllViewButton;

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

            tagAllViewButton.onClick.AddListener(UIController.instance.EnableTagPanel);

            notiButton.onClick.AddListener(UIController.instance.EnableNotiPanel);

            feedButton.onClick.AddListener(UIController.instance.EnableFeedPanel);
        }

    }
}

