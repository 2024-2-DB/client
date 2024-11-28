using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Base
{
    public class Issue_Comment : MonoBehaviour
    {
        [Header("종아요")]
        [SerializeField] private Button likeButton;
        [SerializeField] private TextMeshProUGUI likeCount;
        [SerializeField] private Image likeImage;
        [SerializeField] private bool isLike;
        [SerializeField] private int count;

        [Header("댓글 버튼")]
        [SerializeField] private Button commentButton;

        [Header("UI 오브젝트")]
        [SerializeField] private Button dim;
        [SerializeField] private RectTransform commentScroll;

        [Header("댓글 창 애니메이션")]
        [SerializeField] private float popupSpeed = 100f;
        [SerializeField] private float maxPosY = -150f;
        [SerializeField] private float minPosY = -1600f;



        private void Start()
        {
            //Like
            isLike = false;
            count = 3;
            UpdateLikeCountTMP();

            likeButton.onClick.AddListener(ClickLikeButton);

            //Comment
            commentButton.onClick.AddListener(EnableCommentPopUp);
            dim.onClick.AddListener(DisableCommentPopUp);


        }

        #region Comment

        private void EnableCommentPopUp()
        {
            //StartCoroutine(EnableCommentPopupCoroutine());
            dim.gameObject.SetActive(true);
            commentScroll.gameObject.SetActive(true);
        }

        private void DisableCommentPopUp()
        {
            //StartCoroutine(DisableCommentPopupCoroutine());
            dim.gameObject.SetActive(false);
            commentScroll.gameObject.SetActive(false);
        }


        IEnumerator EnableCommentPopupCoroutine()
        {
            Vector3 position = commentScroll.position;

            dim.gameObject.SetActive(true);

            while (true)
            {
                position.y += popupSpeed * Time.deltaTime;

                if (position.y >= maxPosY)
                {
                    position.y = maxPosY;
                    break;
                }

                commentScroll.position = position;

                yield return null;
            }

            commentScroll.localPosition = position;


            StopAllCoroutines();
        }

        IEnumerator DisableCommentPopupCoroutine()
        {
            Vector3 position = commentScroll.position;

            dim.gameObject.SetActive(false);

            while (true)
            {
                position.y -= popupSpeed * Time.deltaTime;

                if (position.y <= minPosY)
                {
                    position.y = minPosY;

                    break;
                }

                commentScroll.position = position;

                yield return null;
            }

            commentScroll.localPosition = position;

            StopAllCoroutines();
        }
        #endregion

        #region Like

        private void UpdateLikeCountTMP()
        {
            likeCount.text = count.ToString();
        }

        private void ClickLikeButton()
        {
            if (!isLike)
            {
                count++;
                UpdateLikeCountTMP();
                likeImage.color = new Color(1, 0, 0);
                isLike = true;
            }
            else
            {
                count--;
                UpdateLikeCountTMP();
                likeImage.color = new Color(1, 1, 1);
                isLike = false;
            }
        }

        #endregion

    }
}


