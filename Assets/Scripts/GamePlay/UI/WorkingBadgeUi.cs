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

    [SerializeField] private GameObject progressBarPrefab;
    [SerializeField] private GameObject _progressBarParent;
    private List<GameObject> _visibleProgressBar = new List<GameObject>();

    [SerializeField] private GameObject workingDescriptionPrefab;
    [SerializeField] private GameObject _workingDescriptionParent;
    private List<GameObject> _visibleWorkingDescription = new List<GameObject>();
    [SerializeField] private GameObject _grayBackground;


    public void addVisibleWorkingBadgeAndProgressBar(int badgeNum, int workAmount, int workSpeed) 
    { 
        _visibleWorkingBadgeOrder.Add(badgeNum);

        GameObject bar = Instantiate(progressBarPrefab);
        bar.transform.SetParent(_progressBarParent.transform);
        ProgressBarUi barScript = bar.GetComponent<ProgressBarUi>();
        barScript.setWorkAmountAndSpeed(workAmount, workSpeed);
        _visibleProgressBar.Add(bar);

        GameObject des = Instantiate(workingDescriptionPrefab);
        des.transform.SetParent(_workingDescriptionParent.transform);
        WorkingDescriptionUi desScript = des.GetComponent<WorkingDescriptionUi>();
        desScript.characterDataMgr = characterDataMgr;
        desScript.setCharNum(badgeNum);
        _visibleWorkingDescription.Add(des);
    }

    public void removeVisibleWorkingBadgeOrder(int badgeNum) 
    {
        int index = _visibleWorkingBadgeOrder.IndexOf(badgeNum);
        
        Destroy(_visibleProgressBar[index]);
        _visibleProgressBar.RemoveAt(index);

        Destroy(_visibleWorkingDescription[index]);
        _visibleWorkingDescription.RemoveAt(index);

        _visibleWorkingBadgeOrder.Remove(badgeNum);
    }

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

        _grayBackground.SetActive(false);
    }

    
    void Update()
    {
        float xPosB = _basicWorkingBadgeXposInScreen;

        for (int i = _visibleWorkingBadgeOrder.Count; i > 0; i--)
        {
            Transform tf = _workingBadgeSprites[_visibleWorkingBadgeOrder[i-1]].transform;
            tf.position = new Vector3(xPosB, _basicWorkingBadgeYpos, 0f);

            ProgressBarUi barScript = _visibleProgressBar[i-1].GetComponent<ProgressBarUi>();
            barScript.setOutBarPosX(xPosB);

            WorkingDescriptionUi desScript = _visibleWorkingDescription[i-1].GetComponent<WorkingDescriptionUi>();
            desScript.setDescriptionXpos(xPosB);

            xPosB -= _workingBadgesSpacing;
            
        }

        if(Input.GetKey(KeyCode.A))
        {
            _grayBackground.SetActive(true);
            _visibleWorkingDescription.ForEach(obj => obj.GetComponent<Renderer>().enabled = true);
        } else
        {
            _grayBackground.SetActive(false);
            _visibleWorkingDescription.ForEach(obj => obj.GetComponent<Renderer>().enabled = false);
        }

    }
}
