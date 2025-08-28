using UnityEngine;
using System.Collections.Generic;
using TMPro;


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


    //Character And Working Badge UI
    public GameObject characterControlObject;
    private CharacterControl characterControl;

    public GameObject[] _characterSprites; //At first, it's all active
    public GameObject[] _workingBadgeSprites; //At first, it's all non active

    private List<int> _visibleCharacterOrder = new List<int>();
    private List<int> _visibleWorkingBadgeOrder = new List<int>(); 

    private List<float> _visibleCharacterGoalXpos = new List<float>();

    private float _basicCharacterXpos = 15f;
    private float _basicCharacterYpos = -1f;
    private float _charactersSpacing = 15f;
    private float _characterMoveSpeed = 5f;
    private float _chosenCharacterXpos = 2f;

    private float _basicWorkingBadgeXpos = 7.5f;
    private float _basicWorkingBadgeYpos = 3.5f;
    private float _workingBadgesSpacing = 2f;



    //character function
    public void beWorkingCharacterUI(int i)
    {
        if (Mathf.Abs(_chosenCharacterXpos - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1]) < 0.0001f)  
        {
            moveCharacterToStart();
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);
        } else
        {
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);
        }
        _visibleCharacterOrder.Remove(i);
        _visibleWorkingBadgeOrder.Add(i);
        _characterSprites[i].gameObject.SetActive(false);
        _workingBadgeSprites[i].gameObject.SetActive(true);
        
    }

    public void aintWorkingCharacterUI(int i)
    {
        _visibleCharacterOrder.Add(i);
        _visibleWorkingBadgeOrder.Remove(i);
        _characterSprites[i].gameObject.SetActive(true);
        _workingBadgeSprites[i].gameObject.SetActive(false);

        if(_visibleCharacterGoalXpos.Count > 0)
        {
            _visibleCharacterGoalXpos.Add(_visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1] + _charactersSpacing);
        }
        else
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos);
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
                for (int i = 0; i < Count; i++)
                {
                    _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);
                }
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] -= _charactersSpacing;
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
                for (int i = Count; i > 0; i--)
                {
                    _visibleCharacterGoalXpos.Add(_chosenCharacterXpos - (i-1) * _charactersSpacing);
                }
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] += _charactersSpacing;
                }
            }
        }
    }

    public void moveCharacterToStart()
    {
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();
        for (int i = 0; i < count; i++)
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);
        }
    }

    public void moveCharacterToEnd()
    {
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();
        for (int i = count; i > 0; i--)
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos - (i - 1) * _charactersSpacing);
        }
    }

    private void moveCharacterOutsideScreen(int spriteNum)
    {
        Transform tf = _characterSprites[spriteNum].transform;
        tf.position = new Vector3(_basicCharacterXpos, _basicCharacterYpos, 0f);
    }

    private void moveTaskOutsideScreenUsing(int spriteNum)
    {
        Transform tf = _taskSprites[spriteNum].transform;
        tf.position = new Vector3(-7f, -8f, 0f);
    }


    //task function
    private void moveTaskOutsideScreenWaiting(int spriteNum)
    {
        Transform tf = _taskSprites[spriteNum].transform;
        tf.position = new Vector3(-15f, -4f, 0f);
    }

    public void popWork(int index)
    {
        if (index < 0 || index > _usingTaskOrder.Count - 1)
        {
            Debug.LogWarning($"UI: popWork에 잘못된 index값이 주어짐. 주어진 값: {index}");
        }

        Transform tf = _taskSprites[_usingTaskOrder[index]].transform;
        tf.position = new Vector3(-7f, 4f - index * 2f, 0f);

        moveTaskLeft(_usingTaskOrder[index]);

        _waitingTaskOrder.Add(_usingTaskOrder[index]);
        _usingTaskOrder.RemoveAt(index);
    }

    private void moveTaskLeft(int taskNum)
    {
        _taskGoalXpos[taskNum] = -15f;
    }

    private void moveTaskRight(int taskNum)
    {
        _taskGoalXpos[taskNum] = -7f;
    }



    void Start()
    {
        workControl = workControlObject.GetComponent<WorkControl>();
        characterControl = characterControlObject.GetComponent<CharacterControl>();


        //character preprocessing.
        int totalcharacterNum = characterControl.getTotalCharacterNum();

        if (_characterSprites.Length != totalcharacterNum || _workingBadgeSprites.Length != totalcharacterNum)
        {
            Debug.LogWarning("UI: CharacterControl 정보 개수와 캐릭터 스프라이트 개수가 일치하지 않습니다.");
        }

        
        for (int i = 0; i < totalcharacterNum; i++)
        {
            _visibleCharacterOrder.Add(i);
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

            moveCharacterOutsideScreen(i);
        }


        //task preprocessing
        for (int i = 0; i < 5; i++)
        {
           _taskGoalYpos.Add(4f - i * (2f)); // 5 elements, immutable

        }

        _works = workControl.getWorkData();

        int totalWork = _works.Count; 
        for (int i = 0; i < 6; i++) // all sprites (6)
        {

            if (i < totalWork && i < 5)
            {
                TextMeshPro tmp = _taskSprites[i].GetComponentInChildren<TextMeshPro>();

                if (tmp != null)
                {
                    tmp.text = _works[i].workName;
                }

                _usingTaskOrder.Add(i);
                _taskGoalXpos.Add(-7f);
                moveTaskOutsideScreenUsing(i);
            } else
            {
                _waitingTaskOrder.Add(i);
                moveTaskOutsideScreenWaiting(i);
                _taskGoalXpos.Add(-15f);
            }

            
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
                pos.y = Mathf.Lerp(pos.y, _taskGoalYpos[i], Time.deltaTime * 3f);
                tf.position = pos;

            }
        }
        //Debug.Log($"UI: _usingTaskOrder: {string.Join(", ", _usingTaskOrder)}, _waitingTaskOrder: {string.Join(", ", _waitingTaskOrder)}");

        //apply to all sprites, move current Xpos to goal Xpos
        for (int i = 0; i < _taskGoalXpos.Count; i++) 
        {
            Transform tf = _taskSprites[i].transform;
            Vector3 pos = tf.position;

            pos.x = Mathf.Lerp(pos.x, _taskGoalXpos[i], Time.deltaTime * 3f);
            tf.position = pos;
        }
        //Debug.Log($"UI: _taskGoalXpos: {string.Join(", ", _taskGoalXpos)}");
        //Debug.Log($"UI: _taskGoalYpos: {string.Join(", ", _taskGoalYpos)}");


        //refill left work
        int totalWork = workControl.getLeftWork(); 
        if (_waitingTaskOrder.Count > 0 && _usingTaskOrder.Count < totalWork && _usingTaskOrder.Count < 5)
        {
            
            Transform tf = _taskSprites[_waitingTaskOrder[0]].transform;
            tf.position = new Vector3(-15f, -4f, 0f);

            TextMeshPro tmp = _taskSprites[_waitingTaskOrder[0]].GetComponentInChildren<TextMeshPro>();
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


        //Character UI
        if (_visibleCharacterOrder.Count > 0)
        {
            for (int i = 0; i < _visibleCharacterOrder.Count; i++)
            {
                Transform tf = _characterSprites[_visibleCharacterOrder[i]].transform;

                Vector3 pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleCharacterGoalXpos[i], Time.deltaTime * _characterMoveSpeed);
                tf.position = pos;

            }
        }

        //Debug.Log($"UI: _firstCharacterXpos: {_firstCharacterXpos} _goalFirstCharacterXpos: {_goalFirstCharacterXpos}");
        //Debug.Log($"UI: _visibleWorkingBadgeOrder: {string.Join(", ", _visibleWorkingBadgeOrder)}, _visibleCharacterOrder: {string.Join(", ", _visibleCharacterOrder)}");
    }
}

