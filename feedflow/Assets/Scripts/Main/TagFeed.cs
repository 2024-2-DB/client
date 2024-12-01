using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Base
{
    public class TagFeed : MonoBehaviour
    {
        [SerializeField] private Button back;

        private void Start()
        {
            back.onClick.AddListener(UIController.instance.DisableTagPanel);
        }
    }
}

