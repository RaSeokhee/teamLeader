using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;


public class TaskUi : MonoBehaviour
{

    [SerializeField] private WorkDataMgr workDataMgr;
    [SerializeField] private GameObject _taskPrefab;
    [SerializeField] private GameObject _tasksParent;
    private List<GameObject> _taskSprites = new List<GameObject>();

    private List<float> _taskGoalXpos = new List<float>();
    private List<float> _taskGoalYpos = new List<float>();
    private List<int> _usingTaskOrder = new List<int>();
    private List<int> _waitingTaskOrder = new List<int>();
    private List<WorkData> _works;

    private float _basicTaskXpos = -7f;
    private float _basicTaskYpos = 4f;
    private float _basicWaitingTaskXpos = -15f;
    private float _tasksSpacing = 2f;
    private float _taskMoveSpeed = 6f;
    private int _totalTaskSprites = 7;
    private int _totalTaskSpritesPerScreen = 5;

    private int _previousChosenTaskNum = -1;
    private float _chosenTaskMoveDistance = 0.6f;


    public void popWork(int index)
    {
        if (index < 0 || index > _usingTaskOrder.Count - 1)
        {
            Debug.LogWarning($"TaskUi: popWork에 잘못된 index값이 주어짐. 주어진 값: {index}");
        }

        Transform tf = _taskSprites[_usingTaskOrder[index]].transform;
        tf.position = new Vector3(_basicTaskXpos, _basicTaskYpos - index * _tasksSpacing, 0f);

        moveTaskLeft(_usingTaskOrder[index]);

        _waitingTaskOrder.Add(_usingTaskOrder[index]);
        _usingTaskOrder.RemoveAt(index);

    }

    public void hightlightTask(int index)
    {
        if (_previousChosenTaskNum != -1)
        {
            _taskGoalXpos[_previousChosenTaskNum] = _basicTaskXpos;
        }

        if (index < _usingTaskOrder.Count)
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

    private void moveTaskLeft(int taskNum)
    {
        _taskGoalXpos[taskNum] = _basicWaitingTaskXpos;
    }

    private void moveTaskRight(int taskNum)
    {
        _taskGoalXpos[taskNum] = _basicTaskXpos;
    }

    void Awake()
    {
        for (int i = 0; i < _totalTaskSprites; i++)
        {
            GameObject obj = Instantiate(_taskPrefab);
            obj.transform.SetParent(_tasksParent.transform);
            _taskSprites.Add(obj);
        }
    }

    void Start()
    {
        

        if (_totalTaskSprites != _taskSprites.Count)
        {
            Debug.LogWarning("TaskUI: 등록된 task sprites 개수와 변수에 담긴 스프라이트 개수가 일치하지 않습니다.");
        }



        for (int i = 0; i < _totalTaskSpritesPerScreen; i++)
        {
            _taskGoalYpos.Add(_basicTaskYpos - i * _tasksSpacing); // for only visible task sprites in screen, immutable

        }

        _works = workDataMgr.getWorkData();

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
            }
            else
            {
                _waitingTaskOrder.Add(i);
                moveTaskOutsideScreenWaiting(i);
                _taskGoalXpos.Add(_basicWaitingTaskXpos);
            }


        }
    }


    void Update()
    {
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
        
        //apply to all sprites, move current Xpos to goal Xpos
        for (int i = 0; i < _taskGoalXpos.Count; i++)
        {
            Transform tf = _taskSprites[i].transform;
            Vector3 pos = tf.position;

            pos.x = Mathf.Lerp(pos.x, _taskGoalXpos[i], Time.deltaTime * _taskMoveSpeed);
            tf.position = pos;
        }
        
        TextMeshPro tmp;

        //refill left work
        int totalWork = workDataMgr.getLeftWork();
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
    }
}
