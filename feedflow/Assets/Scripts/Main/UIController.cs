//Unity
using UnityEngine.UI;
using UnityEngine;

namespace Base
{
    [DisallowMultipleComponent]
    public class UIController : MonoBehaviour
    {
        [Header("Singleton Pattern : Static Variable")]
        public static UIController instance;

        [Header("∆‰¿Ã¡ˆ")]
        [SerializeField] private GameObject feedPaenl;
        [SerializeField] private GameObject chatPanel;
        [SerializeField] private GameObject issuePanel;
        [SerializeField] private GameObject tagPanel;
        [SerializeField] private GameObject notifyPaenl;
        [SerializeField] private GameObject myPanel;

        private void Awake()
        {
            #region Singleton Pattern 
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
            #endregion


        }

        public void EnableChatPanel()
        {
            chatPanel.SetActive(true);
        }

        public void DisableChatPanel()
        {
            chatPanel.SetActive(false);
        }

        public void EnableIssuePanel()
        {
            issuePanel.SetActive(true);
        }

        public void DisableIssuePanel()
        {
            issuePanel.SetActive(false);
        }

        public void EnableTagPanel()
        {
            tagPanel.SetActive(true);
        }

        public void DisableTagPanel()
        {
            tagPanel.SetActive(false);
        }

        public void EnableNotiPanel()
        {
            notifyPaenl.SetActive(true);
        }

        public void DisableNotiPanel()
        {
            notifyPaenl.SetActive(false);
        }

        public void EnableFeedPanel()
        {
            issuePanel.SetActive(false);
            tagPanel.SetActive(false);
            notifyPaenl.SetActive(false);
            chatPanel.SetActive(false);
        }
    }
}

