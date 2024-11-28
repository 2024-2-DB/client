using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour
{
    public string groupName;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public Button GetButton()
    {
        return button;
    }
}
