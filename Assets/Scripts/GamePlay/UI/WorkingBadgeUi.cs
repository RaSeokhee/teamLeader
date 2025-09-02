using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class WorkingBadgeUi : MonoBehaviour
{

    [SerializeField] private CharacterDataMgr characterDataMgr;
    [SerializeField] private GameObject[] _workingBadgeSprites; 
    private List<int> _visibleWorkingBadgeOrder = new List<int>();

    private float _basicWorkingBadgeXposInScreen = 7.5f;
    private float _basicWorkingBadgeYpos = 3.5f;
    private float _workingBadgesSpacing = 2f;
    private float _basicWorkingBadgeXposOutScreen = 12.5f;

    public void addVisibleWorkingBadgeOrder(int badgeNum) { _visibleWorkingBadgeOrder.Add(badgeNum); }
    public void removeVisibleWorkingBadgeOrder(int badgeNum) { _visibleWorkingBadgeOrder.Remove(badgeNum); }
    public void moveWorkingBadgeOutsideScreen(int spriteNum)
    {
        Transform tf = _workingBadgeSprites[spriteNum].transform;
        tf.position = new Vector3(_basicWorkingBadgeXposOutScreen, _basicWorkingBadgeYpos, 0f);
    }

    void Start()
    {
        if (_workingBadgeSprites.Length != characterDataMgr.getTotalCharacterNum())
        {
            Debug.LogWarning("WorkingBadgeUI: CharacterControl 정보 개수와 working badge 스프라이트 개수가 일치하지 않습니다.");
        }

        for (int i = 0; i < _workingBadgeSprites.Length; i++)
        {
            moveWorkingBadgeOutsideScreen(i);
        }
    }

    
    void Update()
    {
        float xPosB = _basicWorkingBadgeXposInScreen;

        for (int i = _visibleWorkingBadgeOrder.Count; i > 0; i--)
        {
            Transform tf = _workingBadgeSprites[_visibleWorkingBadgeOrder[i - 1]].transform;
            tf.position = new Vector3(xPosB, _basicWorkingBadgeYpos, 0f);
            xPosB -= _workingBadgesSpacing;
        }
    }
}
