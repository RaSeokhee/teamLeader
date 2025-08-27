using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class UI : MonoBehaviour
{
    
    public GameObject workControlObject;
    private WorkControl workControl;
    private List<WorkData> _works;

    public GameObject characterControlObject;
    private CharacterControl characterControl;


    public GameObject[] _taskSprites;
    public GameObject[] _characterSprites; //At first, it's all active
    public GameObject[] _workingBadgeSprites; //At first, it's all non active

    private List<int> _visibleCharacterOrder = new List<int>();
    private List<int> _visibleWorkingBadgeOrder = new List<int>(); 

    private List<float> _visibleCharacterGoalXpos = new List<float>();


   
    public void beWorkingCharacterUI(int i)
    {
        if (Mathf.Abs(2f - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1]) < 0.0001f)  
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
            _visibleCharacterGoalXpos.Add(_visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1] + 10f);
        }
        else
        {
            _visibleCharacterGoalXpos.Add(2f);
        }
        
    }

    public void moveCharacterLeft()
    {
        Debug.Log("moveCharacterLeft()");
        if (_visibleCharacterOrder.Count > 1)
        {
            if (Mathf.Abs(2f - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count-1]) < 0.0001f)
            {
                int Count = _visibleCharacterOrder.Count;
                _visibleCharacterGoalXpos.Clear();
                for (int i = 0; i < Count; i++)
                {
                    _visibleCharacterGoalXpos.Add(2f + i * 10f);
                }
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] -= 10f;
                }
            }
        }
        
    }

    public void moveCharacterRight()
    {
        Debug.Log("oveCharacterRight()");
        if (_visibleCharacterOrder.Count > 1)
        {
            if (Mathf.Abs(2f - _visibleCharacterGoalXpos[0]) < 0.0001f)
            {
                int Count = _visibleCharacterOrder.Count;
                _visibleCharacterGoalXpos.Clear();
                for (int i = Count; i > 0; i--)
                {
                    _visibleCharacterGoalXpos.Add(2f - (i-1) * 10f);
                }
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] += 10f;
                }
            }
        }
    }

    public void moveCharacterToStart()
    {
        Debug.Log("moveCharacterToStart()");
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();
        for (int i = 0; i < count; i++)
        {
            _visibleCharacterGoalXpos.Add(2f + i * 10f);
        }
    }

    public void moveCharacterToEnd()
    {
        Debug.Log("moveCharacterToEnd()");
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();
        for (int i = count; i > 0; i--)
        {
            _visibleCharacterGoalXpos.Add(2f - (i - 1) * 10f);
        }
    }

    private void moveCharacterOutsideScreen(int spriteNum)
    {
        Transform tf = _characterSprites[spriteNum].transform;
        tf.position = new Vector3(15f, -1f, 0f);
    }

   
    void Start()
    {
        workControl = workControlObject.GetComponent<WorkControl>();
        characterControl = characterControlObject.GetComponent<CharacterControl>();

        int totalcharacterNum = characterControl.getTotalCharacterNum();

        if (_characterSprites.Length != totalcharacterNum || _workingBadgeSprites.Length != totalcharacterNum)
        {
            Debug.LogWarning("UI: CharacterControl 정보 개수와 캐릭터 스프라이트 개수가 일치하지 않습니다.");
        }

        
        for (int i = 0; i < totalcharacterNum; i++)
        {
            _visibleCharacterOrder.Add(i);
            _visibleCharacterGoalXpos.Add(2f + i * 10f);

            moveCharacterOutsideScreen(i);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
        _works = workControl.getWorkData();
        if (_works.Count > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                TextMeshPro tmp = _taskSprites[i].GetComponentInChildren<TextMeshPro>();

                if (tmp != null)
                {
                    tmp.text = _works[i].workName;
                }
            }
        }
        else
        {

            for (int i = 0; i < _works.Count; i++)
            {
                TextMeshPro tmp = _taskSprites[i].GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = _works[i].workName;
                }
            }

            for (int i = _works.Count; i < 5; i++)
            {
                _taskSprites[i].gameObject.SetActive(false);
            }

        }

        //visibleWorkingBadge
        float xPosB = 7.5f; 
        
        for (int i = _visibleWorkingBadgeOrder.Count; i > 0; i--)
        {
            Transform tf = _workingBadgeSprites[_visibleWorkingBadgeOrder[i-1]].transform;
            tf.position = new Vector3(xPosB, 3.5f, 0f);
            xPosB -= 2f;
        }


        //visibleCharacter
        if (_visibleCharacterOrder.Count > 0)
        {
            for (int i = 0; i < _visibleCharacterOrder.Count; i++)
            {
                Transform tf = _characterSprites[_visibleCharacterOrder[i]].transform;

                Vector3 pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleCharacterGoalXpos[i], Time.deltaTime * 5f);
                tf.position = pos;

            }
        }

        //Debug.Log($"UI: _firstCharacterXpos: {_firstCharacterXpos} _goalFirstCharacterXpos: {_goalFirstCharacterXpos}");
        //Debug.Log($"UI: _visibleWorkingBadgeOrder: {string.Join(", ", _visibleWorkingBadgeOrder)}, _visibleCharacterOrder: {string.Join(", ", _visibleCharacterOrder)}");
    }
}

