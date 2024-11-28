using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    [DisallowMultipleComponent]
    public class Issue : MonoBehaviour
    {
        [SerializeField] private Button back;

        private void Start()
        {
            back.onClick.AddListener(UIController.instance.DisableIssuePanel);
        }
    }
}


