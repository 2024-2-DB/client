//Unity
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    [DisallowMultipleComponent]
    public class Feed : MonoBehaviour
    {
        [Header("상단 버튼")]

        [Header("이슈 플로우")]
        [SerializeField] private Button issueAllViewButton;

        [Header("팀 이벤트")]

        [Header("네비게이션바 하단 버튼")]
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

