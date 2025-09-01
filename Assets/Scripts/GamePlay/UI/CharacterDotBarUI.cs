using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class CharacterDotBarUI : MonoBehaviour
{
    public GameObject characterControlObject;
    private CharacterControl characterControl;

    public GameObject assignWorkObject;
    private AssignWork assignWork;

    //CharacterDotBar UI
    public GameObject[] _characaterDotSprites;
    private List<float> _visibleDotGoalXpos = new List<float>();
    private int _previousChosenDot = -1;

    private float _basicDotScale = 0.2f;
    private float _bigDotSclae = 0.3f;
    private float _characterDotSpacing = 1f;
    private float _basicCharacterDotXpos = 12f;
    private float _basicCharacterDotYpos = 4f;
    private float _dotMoveSpeed = 6f;

    private List<int> _visibleDotOrder = new List<int>();

    public void clearVisibleDotGoalXpos() {_visibleDotGoalXpos.Clear(); }

    public void addVisibleDotOrder(int dotNum)
    {
        _visibleDotOrder.Add(dotNum);
    }

    public void removeVisibleDotOrder(int dotNum)
    {
        _visibleDotOrder.Remove(dotNum);
    }

    public void removeLastVisibleDotGoalXpos()
    {
        _visibleDotGoalXpos.RemoveAt(_visibleDotGoalXpos.Count - 1);
    }

    public void addVisibleDotGoalXpos(float f)
    {
        _visibleDotGoalXpos.Add(f);
    }

    public void lastAddVisibleDotGoalXpos()
    {
        _visibleDotGoalXpos.Add(_visibleDotGoalXpos[_visibleDotGoalXpos.Count - 1] + 1f);
    }

    public void addSpacingToAllVisibleDotGoalXpos(int index)
    {
        _visibleDotGoalXpos[index] += _characterDotSpacing;
    }

    public void minusSpacingToAllVisibleDotGoalXpos(int index)
    {
        _visibleDotGoalXpos[index] -= _characterDotSpacing;
    }


    public void setPreviousChosenDot(int dotNum) { _previousChosenDot = dotNum; }

    

    //characterDotBar function
    public void moveDotOutsideScreen(int dotNum)
    {
        Transform tf = _characaterDotSprites[dotNum].transform;
        tf.position = new Vector3(_basicCharacterDotXpos, _basicCharacterDotYpos, 0f);

        tf.localScale = new Vector3(_basicDotScale, _basicDotScale, 0f);
    }

    private void highlightDot(int dotNum)
    {
        //Debug.Log($"UI: 이전: _previousChosenDot: {_previousChosenDot}");
        //after plus or minus chosen character number in AssignWork.cs
        Transform tf;

        if (_previousChosenDot != -1)
        {
            tf = _characaterDotSprites[_previousChosenDot].transform;
            tf.localScale = new Vector3(_basicDotScale, _basicDotScale, 0f);
        }

        tf = _characaterDotSprites[dotNum].transform;
        tf.localScale = new Vector3(_bigDotSclae, _bigDotSclae, 0f);
        _previousChosenDot = dotNum;

        //Debug.Log($"UI: 이후: _previousChosenDot: {_previousChosenDot}");
    }

    
    void Start()
    {
        characterControl = characterControlObject.GetComponent<CharacterControl>();
        assignWork = assignWorkObject.GetComponent<AssignWork>();

        int totalcharacterNum = characterControl.getTotalCharacterNum();

        if (_characaterDotSprites.Length != totalcharacterNum)
        {
            Debug.LogError("CharacterDotBarUI: CharacterControl 정보 개수와 dot bar 스프라이트 개수가 일치하지 않습니다.");
        }

        for (int i = 0; i < totalcharacterNum; i++)
        {
            _visibleDotOrder.Add(i);
            _visibleDotGoalXpos.Add(2f + i * 1f);
            moveDotOutsideScreen(i);

        }

        highlightDot(0);

        
    }

    
    void Update()
    {
        //Debug.Log($"_visibleDotGoalXpos: {string.Join(", ", _visibleDotGoalXpos)}");
        if (_visibleDotOrder.Count > 0)
        {

            for (int i = 0; i < _visibleDotOrder.Count; i++)
            {
                Transform tf = _characaterDotSprites[_visibleDotOrder[i]].transform;
                Vector3 pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleDotGoalXpos[i], Time.deltaTime * _dotMoveSpeed);
                tf.position = pos;

                highlightDot(_visibleDotOrder[assignWork.getChosenCharacterNum()]);
            }

        }
    }
}
