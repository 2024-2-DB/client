//Unity
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//System
using System.Collections;
using System;

//TMPro
using TMPro;

//Metwork
using Newtonsoft.Json.Linq;

namespace Base
{

    // ���� �޽����� ���� �����͸� ��� Ŭ����
    [System.Serializable]
    public class MessageData
    {
        public int teamId = 1;
        public int userId = 1;    // userId �߰�
        public string content = "�ȳ�";  // �������� ����ϴ� �ʵ� �̸��� �°� ����
    }

    [System.Serializable]
    public class AIRequestData
    {
        public int teamId = 1;
        public int userId = 1;
        public bool isFirst = false;
    }

    [System.Serializable]
    public class HistoryData
    {
        public int teamId = 1;
        public int userId = 1;
    }

    [DisallowMultipleComponent]
    public class ChatManager : MonoBehaviour
    {
        [Header("ä�� ���� UI ���")]
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private RectTransform content;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject userBubble;
        [SerializeField] private GameObject otherBubble;

        [Header("API URL")]
        [SerializeField] private string apiUrlSend = "https://15.165.140.141:8081/api/message/send";
        [SerializeField] private string apiUrlRecieve = "https://15.165.140.141:8081/api/message/recieve";
        [SerializeField] private string apiUrlHistory = "https://15.165.140.141:8081/api/message";

        [Header("��¥ ǥ�� TMP")]
        [SerializeField] private TextMeshProUGUI todayText;

        [Header("Tag")]
        [SerializeField] private string tagName = "";

        [Header("���� �� �ִ� ����")]
        [SerializeField] private int wordMaxCount = 20;

        void Start()
        {
            apiUrlSend = "https://15.165.140.141:8081/api/message/send";
            apiUrlRecieve = "https://15.165.140.141:8081/api/message/recieve";
            apiUrlHistory = "https://15.165.140.141:8081/api/message";


            DateTime today = DateTime.Now;
            todayText.text = today.ToString("yyyy�� MM�� dd��");

            //��ư�� �̺�Ʈ �߰�
            sendButton.onClick.AddListener(SendMessage);
            backButton.onClick.AddListener(Back);

            // ä�� �����丮 �ҷ�����
            //StartCoroutine(GetChatHistory());
        }

        private void Update()
        {
            inputField.Select();

            //Test Code : Pc Mode
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessage();
            }
        }

        public void Init(string _tag)
        {
            tagName = _tag;
        }

        // �޼����� �����ϱ� ���� �޼ҵ�
        void SendMessage()
        {
            // �޼����� ���� ��� Return
            if (string.IsNullOrEmpty(inputField.text) || IsSpace(inputField.text)) return;

            // User�� ���� �޽��� ǥ��
            AddMessage(inputField.text, true);

            // ���� �޽��� ������
            StartCoroutine(SendUserMessage(inputField.text));

            // �Է�â �ʱ�ȭ
            inputField.text = "";
        }

        // ���ο� �޼����� �߰��ϱ� ���� �޼ҵ�
        void AddMessage(string _message, bool _isUser)
        {
            GameObject newMessage = null;

            if (_isUser)
            {
                newMessage = Instantiate(userBubble, content);
                newMessage.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleRight;
            }
            else
            {
                newMessage = Instantiate(otherBubble, content);
                newMessage.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
            }

            // �ؽ�Ʈ ������Ʈ �ε�
            TextMeshProUGUI messageText = newMessage.transform.Find("[Image] Bubble/[TMP] Chat Text").GetComponent<TextMeshProUGUI>();

            if (messageText == null)
            {
                Debug.LogError("Can't Find Text Component");
                return;
            }

            // �޼��� ����
            messageText.text = WrapTextByWord(_message, wordMaxCount);

            // ��ũ�Ѻ� �ֽ�ȭ
            scrollView.verticalNormalizedPosition = 0f;
        }

        // ä�� �����丮 �ҷ����� (GET ���)
        IEnumerator GetChatHistory()
        {
            var historyData = new HistoryData
            {
                teamId = 1,
                userId = 1
            };

            string jsonData = JsonUtility.ToJson(historyData);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(apiUrlHistory, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // ������ ���� ����
                webRequest.certificateHandler = new IgnoreCertificateHandler();

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // JSON ���� �Ľ�
                    string responseText = webRequest.downloadHandler.text;
                    Debug.Log("Response: " + responseText);

                    // ������ JObject�� �Ľ�
                    JObject jsonResponse = JObject.Parse(responseText);

                    // "data" �迭���� "content" ���� ����
                    JArray messages = (JArray)jsonResponse["data"];

                    foreach (var message in messages)
                    {
                        string content = message["content"].ToString();
                        string role = message["role"].ToString();

                        // role�� ���� true/false�� ����
                        bool isAI = (role == "user");

                        // AddMessage ȣ�� (content�� true/false ����)
                        AddMessage(content, isAI);
                    }
                }
            }
        }

        // ���� �޽��� ������ (POST ���)
        IEnumerator SendUserMessage(string _message)
        {
            // ������ JSON ������ ����
            var messageData = new MessageData
            {
                teamId = 1,
                userId = 1,  // ���� ID�� �Է����ּ���
                content = _message
            };

            string jsonData = JsonUtility.ToJson(messageData);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(apiUrlSend, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                webRequest.certificateHandler = new IgnoreCertificateHandler();

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // ������ �޽����� ���۵Ǿ����� AI ���� �ޱ�
                    StartCoroutine(GetAIResponse());
                }
                else
                {
                    Debug.LogError($"Error: {webRequest.error}");
                }
            }
        }

        // AI ���� �ޱ� (POST ���)
        IEnumerator GetAIResponse()
        {
            // AI ������ �ޱ� ���� ������
            var aiRequestData = new AIRequestData
            {
                teamId = 1,
                userId = 1,  // ���� ID �Է�
                isFirst = false
            };

            // ������ ����ȭ
            string jsonData = JsonUtility.ToJson(aiRequestData);
            Debug.Log(jsonData);  // JSON ������ Ȯ��

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(apiUrlRecieve, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // ������ ���� ����
                webRequest.certificateHandler = new IgnoreCertificateHandler();

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // AI �޽����� ä�ÿ� �߰�
                    string response = webRequest.downloadHandler.text;

                    // ������ ���͸� �� �ٹٲ� ����
                    string[] responseLines = response.Split(new[] { "data:" }, System.StringSplitOptions.RemoveEmptyEntries);

                    // ���κ� ���� ���� �� �ٹٲ� ����
                    for (int i = 0; i < responseLines.Length; i++)
                    {
                        //responseLines[i] = responseLines[i].Trim(); // �� ���� ���� ����
                    }

                    // �����ϰ� *�� �������� ��ü
                    string combinedMessage = string.Join("", responseLines)
                                                  .Replace("*", " ")  // *�� �������� ��ü
                                                  .Trim();           // �յ� ���� ����

                    Debug.Log($"Combined AI Message: {combinedMessage}");

                    // AI �޽����� ä�ÿ� �߰�
                    AddMessage(combinedMessage, false);

                }
                else
                {
                    Debug.LogError($"Error: {webRequest.error}");
                }
            }
        }

        private string WrapTextByWord(string _text, int _maxChar)
        {
            string resultString = "";
            int currentCharCount = 0;

            foreach (char c in _text)
            {
                resultString += c;
                currentCharCount++;

                if (currentCharCount >= _maxChar)
                {
                    resultString += '\n'; // �ٹٲ� �߰�
                    currentCharCount = 0; // ���� �� �ʱ�ȭ
                }
            }

            return resultString;
        }

        private bool IsSpace(string _text)
        {
            return _text.Trim().Length == 0;
        }

        private void Back()
        {
            UIController.instance.DisableChatPanel();
        }

        // ������ ���� �ڵ鷯 (SSL ������ ����)
        public class IgnoreCertificateHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true; // ������ ����
            }
        }
    }
}
