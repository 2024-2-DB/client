using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupSelect : MonoBehaviour
{
    [Header("그룹 리스트 버튼")]
    [SerializeField] private List<Group> groups;

    public void AddGroupButtons(Group _group)
    {
        groups.Add(_group);
    }

    public void AddEvent()
    {

    }
}
