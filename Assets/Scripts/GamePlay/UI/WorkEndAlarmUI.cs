using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class WorkEndAlarmUI : MonoBehaviour
{
    public GameObject characterControlObject;
    private CharacterControl characterControl;

    //WorkEndAlarm UI
    public GameObject _workEndAlarmText;
    private List<string> _workEndTextList = new List<string> { "\n", "\n ", "\n", "\n", "\n" };
    private string[] _workEndAlarmColor = { "#E6FFE6", "#B3FFB3", "#00FF00", "#009900", "#004400" };
    private string _workEndText = "";
    private int _textShouldMoveCount = 0;
    private float _timer = 0f;
    private bool _isFirstEndCharacter = false;
    private float _workEndTextMoveTime = 0.2f; //the smaller, the faster
    private int _totalWorkEndCount = 0;
    private List<CharacterData> _characters;

    //WorkEndAlarm function
    public void _addWorkEndCharacterToList(int charNum)
    {
        if (_textShouldMoveCount == 0)
        {
            _isFirstEndCharacter = true;
        }
        _workEndTextList.Add($"+ {_characters[charNum].characterName}\n");
        _textShouldMoveCount = _workEndTextList.Count;
    }

    private void _remakeWorkEndText()
    {
        _workEndText = "";

        for (int i = 0; i < _totalWorkEndCount; i++)
        {
            _workEndText += $"<color={_workEndAlarmColor[i]}>{_workEndTextList[i]}</color>";
        }

        TextMeshPro tmp = _workEndAlarmText.GetComponent<TextMeshPro>();
        tmp.text = _workEndText;
    }

    private void _workEndAlarmManage()
    {
        if (_textShouldMoveCount > 0)
        {
            _timer += Time.deltaTime;

            if (_isFirstEndCharacter)
            {
                _timer = _workEndTextMoveTime;
                _isFirstEndCharacter = false;
            }

            if (_timer >= _workEndTextMoveTime)
            {

                if (_workEndTextList.Count < _totalWorkEndCount + 1)
                {
                    for (int i = 0; i < _totalWorkEndCount + 1 - _workEndTextList.Count; i++)
                    {
                        _workEndTextList.Add("\n");
                    }

                }

                _workEndTextList.RemoveAt(0);
                _textShouldMoveCount -= 1;
                _remakeWorkEndText();
                _timer = 0f;
            }
        }
        else
        {
            _timer = 0f;
        }

    }



    void Start()
    {
        characterControl = characterControlObject.GetComponent<CharacterControl>();

        if (_workEndTextList.Count != _workEndAlarmColor.Length)
        {
            Debug.LogWarning("WorkEndAlarmUI: _workEndTextList 개수와 _workEndAlarmColor 개수가 일치하지 않습니다.");
        }

        _characters = characterControl.getCharacterData();
        _totalWorkEndCount = _workEndTextList.Count;
    }

    
    void Update()
    {
        //workEndAlarm manage
        _workEndAlarmManage();
    }
}
