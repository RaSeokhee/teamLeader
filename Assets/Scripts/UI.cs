using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UI : MonoBehaviour
{

    //Task UI
    public GameObject workControlObject;
    private WorkControl workControl;
    private List<WorkData> _works;

    public GameObject[] _taskSprites;

    private List<float> _taskGoalXpos = new List<float>();
    private List<float> _taskGoalYpos = new List<float>();
    private List<int> _usingTaskOrder = new List<int>();
    private List<int> _waitingTaskOrder = new List<int>();

    private float _basicTaskXpos = -7f;
    private float _basicTaskYpos = 4f;
    private float _basicWaitingTaskXpos = -15f;
    private float _tasksSpacing = 2f;
    private float _taskMoveSpeed = 6f;
    private int _totalTaskSprites = 7;
    private int _totalTaskSpritesPerScreen = 5;

    private int _previousChosenTaskNum = -1;
    private float _chosenTaskMoveDistance = 0.6f;

    //character description UI
    public GameObject _characterDescriptionText;
    private List<CharacterData> _characters;

    public GameObject assignWorkObject;
    private AssignWork assignWork;

    //Character And Working Badge UI
    public GameObject characterControlObject;
    private CharacterControl characterControl;

    public GameObject[] _characterSprites; //At first, it's all active
    public GameObject[] _workingBadgeSprites; //At first, it's all non active

    private List<int> _visibleCharacterOrder = new List<int>();
    private List<int> _visibleWorkingBadgeOrder = new List<int>();

    private List<float> _visibleCharacterGoalXpos = new List<float>();

    private float _basicCharacterXpos = 15f;
    private float _basicCharacterYpos = 1.5f;
    private float _charactersSpacing = 15f;
    private float _characterMoveSpeed = 6f;
    private float _chosenCharacterXpos = 2f;

    private float _basicWorkingBadgeXpos = 7.5f;
    private float _basicWorkingBadgeYpos = 3.5f;
    private float _workingBadgesSpacing = 2f;

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

    //CharacterDotBar UI
    public GameObject[] _characaterDotSprites;
    private List<float> _visibleDotGoalXpos = new List<float>();
    private int _previousChosenDot = -1;

    private float _basicDotScale = 0.2f;
    private float _bigDotSclae = 0.3f;
    private float _characterDotSpacing = 1f;
    private float _basicCharacterDotXpos = 12f;
    private float _basicCharacterDotYpos = 4f;

    //WorkEndAlarm function
    private void _addWorkEndCharacterToList(int charNum)
    {
        if(_textShouldMoveCount == 0) 
        {
            _isFirstEndCharacter=true; 
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
        if(_textShouldMoveCount > 0)
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
        }else
        {
            _timer = 0f;
        }

    }

    
    //character function
    public void beWorkingCharacterUI(int i)
    {
        if (Mathf.Abs(_chosenCharacterXpos - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1]) < 0.0001f)  
        {
            moveCharacterToStart();
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);

            //Character Dot Bar UI
            _visibleDotGoalXpos.RemoveAt(_visibleDotGoalXpos.Count -1);
            moveDotOutsideScreen(i);
        } else
        {
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);

            //Character Dot Bar UI
            _visibleDotGoalXpos.RemoveAt(_visibleDotGoalXpos.Count - 1);
            moveDotOutsideScreen(i);
        }
        _visibleCharacterOrder.Remove(i);
        _characterSprites[i].gameObject.SetActive(false);
        
        _visibleWorkingBadgeOrder.Add(i);
        _workingBadgeSprites[i].gameObject.SetActive(true);
        
        if(_visibleCharacterOrder.Count < 1) // if count is zero
        {
            _previousChosenDot = -1;
        }
    }

    public void aintWorkingCharacterUI(int i)
    {
        _visibleCharacterOrder.Add(i);
        _visibleWorkingBadgeOrder.Remove(i);
        _addWorkEndCharacterToList(i);
        _characterSprites[i].gameObject.SetActive(true);
        _workingBadgeSprites[i].gameObject.SetActive(false);

        if(_visibleCharacterGoalXpos.Count > 0)
        {
            _visibleCharacterGoalXpos.Add(_visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1] + _charactersSpacing);
            _visibleDotGoalXpos.Add(_visibleDotGoalXpos[_visibleDotGoalXpos.Count - 1] + 1f);
        }
        else
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos);
            _visibleDotGoalXpos.Add(2f);
            
        }
        

    }

    public void moveCharacterLeft()
    {
        if (_visibleCharacterOrder.Count > 1)
        {
            if (Mathf.Abs(_chosenCharacterXpos - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count-1]) < 0.0001f)
            {
                int Count = _visibleCharacterOrder.Count;
                _visibleCharacterGoalXpos.Clear();

                //charater dot UI
                _visibleDotGoalXpos.Clear();

                for (int i = 0; i < Count; i++)
                {
                    _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

                    //charater dot UI
                    _visibleDotGoalXpos.Add(2f + i * 1f);
                }

            
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] -= _charactersSpacing;

                    //charater dot UI
                    _visibleDotGoalXpos[i] -= _characterDotSpacing;
                }
            }

            
        }
        
    }

    public void moveCharacterRight()
    {
        if (_visibleCharacterOrder.Count > 1)
        {
            if (Mathf.Abs(_chosenCharacterXpos - _visibleCharacterGoalXpos[0]) < 0.0001f)
            {
                int Count = _visibleCharacterOrder.Count;
                _visibleCharacterGoalXpos.Clear();

                //charater dot UI
                _visibleDotGoalXpos.Clear();

                for (int i = Count; i > 0; i--)
                {
                    _visibleCharacterGoalXpos.Add(_chosenCharacterXpos - (i-1) * _charactersSpacing);

                    //charater dot UI
                    _visibleDotGoalXpos.Add(2f - (i - 1) * 1f);
                }
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] += _charactersSpacing;

                    //charater dot UI
                    _visibleDotGoalXpos[i] += _characterDotSpacing;
                }
            }

            
        }
    }

    public void moveCharacterToStart()
    {
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();

        //charater dot UI
        _visibleDotGoalXpos.Clear();

        for (int i = 0; i < count; i++)
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

            //charater dot UI
            _visibleDotGoalXpos.Add(2f + i * 1f);
        }

        
    }

    public void moveCharacterToEnd()
    {
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();

        //charater dot UI
        _visibleDotGoalXpos.Clear();

        for (int i = count; i > 0; i--)
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos - (i - 1) * _charactersSpacing);

            //charater dot UI
            _visibleDotGoalXpos.Add(2f - (i - 1) * 1f);
        }

        
    }

    private void moveCharacterOutsideScreen(int spriteNum)
    {
        Transform tf = _characterSprites[spriteNum].transform;
        tf.position = new Vector3(_basicCharacterXpos, _basicCharacterYpos, 0f);
    }

    IEnumerator _moveCharacterDescriptionCenter()
    {
        Transform tf = _characterDescriptionText.transform;


        while (Mathf.Abs(tf.position.x - _chosenCharacterXpos) > 0.01f)
        {
            float newX = Mathf.Lerp(tf.position.x, _chosenCharacterXpos, Time.deltaTime * _characterMoveSpeed);
            tf.position = new Vector3(newX, -2.5f, 0f);

            yield return null;
        }

        tf.position = new Vector3(_chosenCharacterXpos, -2.5f, 0f);
    }

    //characterDotBar function
    private void moveDotOutsideScreen(int dotNum)
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

    //task function
    private void moveTaskOutsideScreenUsing(int spriteNum)
    {
        Transform tf = _taskSprites[spriteNum].transform;
        tf.position = new Vector3(_basicTaskXpos, _basicTaskYpos - 6 * _tasksSpacing, 0f);
    }
    

    private void moveTaskOutsideScreenWaiting(int spriteNum)
    {
        Transform tf = _taskSprites[spriteNum].transform;
        tf.position = new Vector3(_basicWaitingTaskXpos, _basicTaskYpos - 4 * _tasksSpacing, 0f);
    }

    

    public void popWork(int index)
    {
        if (index < 0 || index > _usingTaskOrder.Count - 1)
        {
            Debug.LogWarning($"UI: popWork에 잘못된 index값이 주어짐. 주어진 값: {index}");
        }

        Transform tf = _taskSprites[_usingTaskOrder[index]].transform;
        tf.position = new Vector3(_basicTaskXpos, _basicTaskYpos - index * _tasksSpacing, 0f);

        moveTaskLeft(_usingTaskOrder[index]);

        _waitingTaskOrder.Add(_usingTaskOrder[index]);
        _usingTaskOrder.RemoveAt(index);
        
    }

    private void moveTaskLeft(int taskNum)
    {
        _taskGoalXpos[taskNum] = _basicWaitingTaskXpos;
    }

    private void moveTaskRight(int taskNum)
    {
        _taskGoalXpos[taskNum] = _basicTaskXpos;
    }

    public void hightlightTask (int index)
    { 
        if (_previousChosenTaskNum != -1)
        {
            _taskGoalXpos[_previousChosenTaskNum] = _basicTaskXpos;
        }

        if(index < _usingTaskOrder.Count)
        {
            _taskGoalXpos[_usingTaskOrder[index]] += _chosenTaskMoveDistance;
            _previousChosenTaskNum = _usingTaskOrder[index];
        }
        
    }

    public void noHightLightAnyTask()
    {
        //if assign task which tasknum is _previousChosenTaskNum, it should add to _waitingTaskOrder before run this function.
        if (_previousChosenTaskNum != -1 && !_waitingTaskOrder.Contains(_previousChosenTaskNum)) //this case applies _previousChosenTaskNum task isn't assigned
        {
            _taskGoalXpos[_previousChosenTaskNum] = _basicTaskXpos;
        }

        _previousChosenTaskNum = -1;
    }

    

    void Start()
    {
        workControl = workControlObject.GetComponent<WorkControl>();
        characterControl = characterControlObject.GetComponent<CharacterControl>();
        assignWork = assignWorkObject.GetComponent<AssignWork>();

        //character, workingBadge, characterDotBar preprocessing.
        int totalcharacterNum = characterControl.getTotalCharacterNum();

        if (_characterSprites.Length != totalcharacterNum || _workingBadgeSprites.Length != totalcharacterNum || _characaterDotSprites.Length != totalcharacterNum)
        {
            Debug.LogWarning("UI: CharacterControl 정보 개수와 캐릭터 관련 스프라이트 개수가 일치하지 않습니다.");
        }


        for (int i = 0; i < totalcharacterNum; i++)
        {
            _visibleCharacterOrder.Add(i);
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

            moveCharacterOutsideScreen(i);

            _visibleDotGoalXpos.Add(2f + i * 1f);
            moveDotOutsideScreen(i);
        }

        highlightDot(0);

        //task preprocessing

        if (_totalTaskSprites != _taskSprites.Length)
        {
            Debug.LogWarning("UI: 등록된 task sprites 개수와 변수에 담긴 스프라이트 개수가 일치하지 않습니다.");
        }



        for (int i = 0; i < _totalTaskSpritesPerScreen; i++)
        {
           _taskGoalYpos.Add(_basicTaskYpos - i * _tasksSpacing); // for only visible task sprites in screen, immutable

        }

        _works = workControl.getWorkData();

        int totalWork = _works.Count; 
        for (int i = 0; i < _totalTaskSprites; i++) // apply to all sprites
        {

            if (i < totalWork && i < _totalTaskSpritesPerScreen)
            {
                TextMeshPro tmp = _taskSprites[i].GetComponentInChildren<TextMeshPro>();

                if (tmp != null)
                {
                    tmp.text = _works[i].workName;
                }

                _usingTaskOrder.Add(i);
                _taskGoalXpos.Add(_basicTaskXpos);
                moveTaskOutsideScreenUsing(i);
            } else
            {
                _waitingTaskOrder.Add(i);
                moveTaskOutsideScreenWaiting(i);
                _taskGoalXpos.Add(_basicWaitingTaskXpos);
            }

            
        }

        //characterDescription preprocessing
        _characters = characterControl.getCharacterData();

        StartCoroutine(_moveCharacterDescriptionCenter());

        _totalWorkEndCount = _workEndTextList.Count;
        
        //workEndAlarm preprocessing
        if (_workEndTextList.Count != _workEndAlarmColor.Length)
        {
            Debug.LogWarning("UI: _workEndTextList 개수와 _workEndAlarmColor 개수가 일치하지 않습니다.");
        }


        
    }

    

    // Update is called once per frame
    void Update()
    {
      
        //task UI

        //apply to using task sprites, move  current Ypos to goal Ypos
        if (_usingTaskOrder.Count > 0)
        {
            for (int i = 0; i < _usingTaskOrder.Count; i++)
            {
                Transform tf = _taskSprites[_usingTaskOrder[i]].transform;

                Vector3 pos = tf.position;
                pos.y = Mathf.Lerp(pos.y, _taskGoalYpos[i], Time.deltaTime * _taskMoveSpeed);
                tf.position = pos;

            }
        }
        //Debug.Log($"UI: _usingTaskOrder: {string.Join(", ", _usingTaskOrder)}, _waitingTaskOrder: {string.Join(", ", _waitingTaskOrder)}");

        //apply to all sprites, move current Xpos to goal Xpos
        for (int i = 0; i < _taskGoalXpos.Count; i++) 
        {
            Transform tf = _taskSprites[i].transform;
            Vector3 pos = tf.position;

            pos.x = Mathf.Lerp(pos.x, _taskGoalXpos[i], Time.deltaTime * _taskMoveSpeed);
            tf.position = pos;
        }
        //Debug.Log($"UI: _taskGoalXpos: {string.Join(", ", _taskGoalXpos)}");
        //Debug.Log($"UI: _taskGoalYpos: {string.Join(", ", _taskGoalYpos)}");

        TextMeshPro tmp;

        //refill left work
        int totalWork = workControl.getLeftWork(); 
        if (_waitingTaskOrder.Count > 0 && _usingTaskOrder.Count < totalWork && _usingTaskOrder.Count < _totalTaskSpritesPerScreen)
        {
            
            Transform tf = _taskSprites[_waitingTaskOrder[0]].transform;
            tf.position = new Vector3(_basicWaitingTaskXpos, _basicTaskYpos - _tasksSpacing * 4, 0f);

            tmp = _taskSprites[_waitingTaskOrder[0]].GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = _works[4].workName;
            }
            

            _usingTaskOrder.Add(_waitingTaskOrder[0]);
            moveTaskRight(_waitingTaskOrder[0]);

            _waitingTaskOrder.RemoveAt(0);
        }


        //Badge UI
        float xPosB = _basicWorkingBadgeXpos; 
        
        for (int i = _visibleWorkingBadgeOrder.Count; i > 0; i--)
        {
            Transform tf = _workingBadgeSprites[_visibleWorkingBadgeOrder[i-1]].transform;
            tf.position = new Vector3(xPosB, _basicWorkingBadgeYpos, 0f);
            xPosB -= _workingBadgesSpacing;
        }


        //workEndAlarm manage
        _workEndAlarmManage();


        //Character, Dot Bar, Character Description UI
        tmp = _characterDescriptionText.GetComponent<TextMeshPro>();

        if (_visibleCharacterOrder.Count > 0)
        {
            

            for (int i = 0; i < _visibleCharacterOrder.Count; i++)
            {
                Transform tf = _characterSprites[_visibleCharacterOrder[i]].transform;

                Vector3 pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleCharacterGoalXpos[i], Time.deltaTime * _characterMoveSpeed);
                tf.position = pos;

                //Character Dot UI
                tf = _characaterDotSprites[_visibleCharacterOrder[i]].transform;
                pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleDotGoalXpos[i], Time.deltaTime * _characterMoveSpeed);
                tf.position = pos;

                highlightDot(_visibleCharacterOrder[assignWork.getChosenCharacterNum()]);
            }

            if (tmp != null)
            {
                tmp.text = _characters[_visibleCharacterOrder[assignWork.getChosenCharacterNum()]].characterName;
            }

        } else
        {
            if (tmp != null)
            {
                tmp.text = "";
            }
            
        }

        //Debug.Log($"UI: _firstCharacterXpos: {_firstCharacterXpos} _goalFirstCharacterXpos: {_goalFirstCharacterXpos}");
        //Debug.Log($"UI: _visibleWorkingBadgeOrder: {string.Join(", ", _visibleWorkingBadgeOrder)}, _visibleCharacterOrder: {string.Join(", ", _visibleCharacterOrder)}");

    }
}

