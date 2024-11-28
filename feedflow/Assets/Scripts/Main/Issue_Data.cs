using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Base
{
    [System.Serializable]
    public class IssueData
    {
        public string issueID;
        public string issueName;

        public int flowCount;
        public int likeCount;
        public int commentCount;

        public bool isLike;
    }

    public class Issue_Data : MonoBehaviour
    {
        [SerializeField]
        public List<IssueData> issues;

        [Header("남은 시간 표시")]
        [SerializeField] private TextMeshProUGUI remainTimeTMP;
        [SerializeField] private int remainTime;

        private void Start()
        {
            remainTime = 5671;

            StartCoroutine(UpdateRemainTime());
        }

        #region RemainTime

        IEnumerator UpdateRemainTime()
        {
            while (remainTime > 0)
            {
                remainTime--;

                int hour = remainTime / 3600;
                int minute = (remainTime - (hour * 3600)) / 60;
                int sec = remainTime - ((hour * 3600) + (minute * 60));

                remainTimeTMP.text = hour.ToString() + ":" + minute.ToString() + ":" + sec.ToString() + "초 남음";

                yield return new WaitForSeconds(1f);
            }
        }

        #endregion
    }
}

