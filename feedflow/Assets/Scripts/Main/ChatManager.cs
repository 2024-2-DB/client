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

    // 유저 메시지에 대한 데이터를 담는 클래스
    [System.Serializable]
    public class MessageData
    {
        public int teamId = 1;
        public int userId = 1;    // userId 추가
        public string content = "안녕";  // 서버에서 기대하는 필드 이름에 맞게 정의
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
        [Header("채팅 관련 UI 요소")]
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

        [Header("날짜 표시 TMP")]
        [SerializeField] private TextMeshProUGUI todayText;

        [Header("Tag")]
        [SerializeField] private string tagName = "";

        [Header("한줄 당 최대 길이")]
        [SerializeField] private int wordMaxCount = 20;

        void Start()
        {
            apiUrlSend = "https://15.165.140.141:8081/api/message/send";
            apiUrlRecieve = "https://15.165.140.141:8081/api/message/recieve";
            apiUrlHistory = "https://15.165.140.141:8081/api/message";


            DateTime today = DateTime.Now;
            todayText.text = today.ToString("yyyy년 MM월 dd일");

            //버튼에 이벤트 추가
            sendButton.onClick.AddListener(SendMessage);
            backButton.onClick.AddListener(Back);

            // 채팅 히스토리 불러오기
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

        // 메세지를 전달하기 위한 메소드
        void SendMessage()
        {
            // 메세지가 없을 경우 Return
            if (string.IsNullOrEmpty(inputField.text) || IsSpace(inputField.text)) return;

            // User가 보낸 메시지 표시
            AddMessage(inputField.text, true);

            // 유저 메시지 보내기
            StartCoroutine(SendUserMessage(inputField.text));

            // 입력창 초기화
            inputField.text = "";
        }

        // 새로운 메세지를 추가하기 위한 메소드
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

            // 텍스트 컴포넌트 로드
            TextMeshProUGUI messageText = newMessage.transform.Find("[Image] Bubble/[TMP] Chat Text").GetComponent<TextMeshProUGUI>();

            if (messageText == null)
            {
                Debug.LogError("Can't Find Text Component");
                return;
            }

            // 메세지 설정
            messageText.text = WrapTextByWord(_message, wordMaxCount);

            // 스크롤뷰 최신화
            scrollView.verticalNormalizedPosition = 0f;
        }

        // 채팅 히스토리 불러오기 (GET 방식)
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

                // 인증서 무시 설정
                webRequest.certificateHandler = new IgnoreCertificateHandler();

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // JSON 응답 파싱
                    string responseText = webRequest.downloadHandler.text;
                    Debug.Log("Response: " + responseText);

                    // 응답을 JObject로 파싱
                    JObject jsonResponse = JObject.Parse(responseText);

                    // "data" 배열에서 "content" 값을 추출
                    JArray messages = (JArray)jsonResponse["data"];

                    foreach (var message in messages)
                    {
                        string content = message["content"].ToString();
                        string role = message["role"].ToString();

                        // role에 따라 true/false로 구분
                        bool isAI = (role == "user");

                        // AddMessage 호출 (content와 true/false 전달)
                        AddMessage(content, isAI);
                    }
                }
            }
        }

        // 유저 메시지 보내기 (POST 방식)
        IEnumerator SendUserMessage(string _message)
        {
            // 전송할 JSON 데이터 생성
            var messageData = new MessageData
            {
                teamId = 1,
                userId = 1,  // 유저 ID를 입력해주세요
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
                    // 서버에 메시지가 전송되었으면 AI 응답 받기
                    StartCoroutine(GetAIResponse());
                }
                else
                {
                    Debug.LogError($"Error: {webRequest.error}");
                }
            }
        }

        // AI 응답 받기 (POST 방식)
        IEnumerator GetAIResponse()
        {
            // AI 응답을 받기 위한 데이터
            var aiRequestData = new AIRequestData
            {
                teamId = 1,
                userId = 1,  // 유저 ID 입력
                isFirst = false
            };

            // 데이터 직렬화
            string jsonData = JsonUtility.ToJson(aiRequestData);
            Debug.Log(jsonData);  // JSON 데이터 확인

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(apiUrlRecieve, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // 인증서 무시 설정
                webRequest.certificateHandler = new IgnoreCertificateHandler();

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // AI 메시지를 채팅에 추가
                    string response = webRequest.downloadHandler.text;

                    // 데이터 필터링 및 줄바꿈 제거
                    string[] responseLines = response.Split(new[] { "data:" }, System.StringSplitOptions.RemoveEmptyEntries);

                    // 라인별 공백 제거 및 줄바꿈 삭제
                    for (int i = 0; i < responseLines.Length; i++)
                    {
                        //responseLines[i] = responseLines[i].Trim(); // 각 줄의 공백 제거
                    }

                    // 결합하고 *를 공백으로 대체
                    string combinedMessage = string.Join("", responseLines)
                                                  .Replace("*", " ")  // *를 공백으로 교체
                                                  .Trim();           // 앞뒤 공백 제거

                    Debug.Log($"Combined AI Message: {combinedMessage}");

                    // AI 메시지를 채팅에 추가
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
                    resultString += '\n'; // 줄바꿈 추가
                    currentCharCount = 0; // 글자 수 초기화
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

        // 인증서 무시 핸들러 (SSL 인증서 무시)
        public class IgnoreCertificateHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true; // 인증서 무시
            }
        }
    }
}
