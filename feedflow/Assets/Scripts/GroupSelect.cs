using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupSelect : MonoBehaviour
{
    [Header("�׷� ����Ʈ ��ư")]
    [SerializeField] private List<Group> groups;

    public void AddGroupButtons(Group _group)
    {
        groups.Add(_group);
    }

    public void AddEvent()
    {

    }
}
