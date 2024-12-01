using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Base
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private Button backButton;

        private void Start()
        {
            backButton.onClick.AddListener(UIController.instance.DisableNotiPanel);
        }
    }
}


