using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UI : MonoBehaviour
{
    //TaskUI
    private TaskUI taskUI;
    public void popWork(int index) { taskUI.popWork(index); }
    public void hightlightTask(int index) { taskUI.hightlightTask(index); }
    public void noHightLightAnyTask() { taskUI.noHightLightAnyTask(); }

    //WorkEndAlarmUI
    private WorkEndAlarmUI workEndAlarmUI;

    //CharacterDotBarUI
    private CharacterDotBarUI characterDotBarUI;
    private void moveDotOutsideScreen(int dotNum) { characterDotBarUI.moveDotOutsideScreen(dotNum); }
    private void removeLastVisibleDotGoalXpos() { characterDotBarUI.removeLastVisibleDotGoalXpos(); }
    private void addVisibleDotGoalXpos(float f) { characterDotBarUI.addVisibleDotGoalXpos(f); }
    private void clearVisibleDotGoalXpos() { characterDotBarUI.clearVisibleDotGoalXpos(); }
    private void lastAddVisibleDotGoalXpos() { characterDotBarUI.lastAddVisibleDotGoalXpos(); }
    private void addSpacingToAllVisibleDotGoalXpos(int index) { characterDotBarUI.addSpacingToAllVisibleDotGoalXpos(index); }
    private void minusSpacingToAllVisibleDotGoalXpos(int index) { characterDotBarUI.minusSpacingToAllVisibleDotGoalXpos(index); }
    private void addVisibleDotOrder(int dotNum) { characterDotBarUI.addVisibleDotOrder(dotNum); }
    
    private void removeVisibleDotOrder(int dotNum) { characterDotBarUI.removeVisibleDotOrder(dotNum); }
    private void setPreviousChosenDot(int dotNum) { characterDotBarUI.setPreviousChosenDot(dotNum); }

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

    

    //character function
    public void beWorkingCharacterUI(int i)
    {
        if (Mathf.Abs(_chosenCharacterXpos - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1]) < 0.0001f)  
        {
            moveCharacterToStart();
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);

            //Character Dot Bar UI
            removeLastVisibleDotGoalXpos();
            moveDotOutsideScreen(i);
        } else
        {
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);

            //Character Dot Bar UI
            removeLastVisibleDotGoalXpos();
            moveDotOutsideScreen(i);
        }
        _visibleCharacterOrder.Remove(i);
        removeVisibleDotOrder(i);
        _characterSprites[i].gameObject.SetActive(false);
        
        _visibleWorkingBadgeOrder.Add(i);
        _workingBadgeSprites[i].gameObject.SetActive(true);
        
        if(_visibleCharacterOrder.Count < 1) // if count is zero
        {
            setPreviousChosenDot(-1);
        }
    }

    public void aintWorkingCharacterUI(int i)
    {
        _visibleCharacterOrder.Add(i);
        addVisibleDotOrder(i);
        _visibleWorkingBadgeOrder.Remove(i);
        workEndAlarmUI._addWorkEndCharacterToList(i);
        _characterSprites[i].gameObject.SetActive(true);
        _workingBadgeSprites[i].gameObject.SetActive(false);

        if(_visibleCharacterGoalXpos.Count > 0)
        {
            _visibleCharacterGoalXpos.Add(_visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1] + _charactersSpacing);
            lastAddVisibleDotGoalXpos();
        }
        else
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos);
            addVisibleDotGoalXpos(2f);
            
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
                clearVisibleDotGoalXpos();
                for (int i = 0; i < Count; i++)
                {
                    _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

                    //charater dot UI
                    addVisibleDotGoalXpos(2f + i * 1f);
                }

            
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] -= _charactersSpacing;

                    //charater dot UI
                    minusSpacingToAllVisibleDotGoalXpos(i);
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
                clearVisibleDotGoalXpos();

                for (int i = Count; i > 0; i--)
                {
                    _visibleCharacterGoalXpos.Add(_chosenCharacterXpos - (i-1) * _charactersSpacing);
                    //charater dot UI
                    addVisibleDotGoalXpos(2f - (i - 1) * 1f);
                   
                }
            }
            else
            {
                for (int i = 0; i < _visibleCharacterGoalXpos.Count; i++)
                {
                    _visibleCharacterGoalXpos[i] += _charactersSpacing;

                    //charater dot UI
                    addSpacingToAllVisibleDotGoalXpos(i);
                }
            }

            
        }
    }

    public void moveCharacterToStart()
    {
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();

        //charater dot UI
        clearVisibleDotGoalXpos();

        for (int i = 0; i < count; i++)
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

            //charater dot UI
            addVisibleDotGoalXpos(2f + i * 1f);
        }

        
    }

    public void moveCharacterToEnd()
    {
        int count = _visibleCharacterGoalXpos.Count;
        _visibleCharacterGoalXpos.Clear();

        //charater dot UI
        clearVisibleDotGoalXpos();
        for (int i = count; i > 0; i--)
        {
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos - (i - 1) * _charactersSpacing);
            //charater dot UI
            addVisibleDotGoalXpos(2f - (i - 1) * 1f);
            

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

   

    void Start()
    {
        taskUI = GetComponent<TaskUI>();
        if (taskUI == null) { Debug.LogError("UI: TaskUI 스크립트가 UI 오브젝트에 붙어있지 않습니다."); }

        workEndAlarmUI = GetComponent<WorkEndAlarmUI>();
        if (workEndAlarmUI == null) { Debug.LogError("UI: WorkEndAlarmUI 스크립트가 UI 오브젝트에 붙어있지 않습니다."); }


        characterDotBarUI = GetComponent<CharacterDotBarUI>();
        if (characterDotBarUI == null) { Debug.LogError("UI: characterDotBarUI 스크립트가 UI 오브젝트에 붙어있지 않습니다."); }

        characterControl = characterControlObject.GetComponent<CharacterControl>();
        assignWork = assignWorkObject.GetComponent<AssignWork>();

        //character, workingBadge, characterDotBar preprocessing.
        int totalcharacterNum = characterControl.getTotalCharacterNum();

        if (_characterSprites.Length != totalcharacterNum || _workingBadgeSprites.Length != totalcharacterNum)
        {
            Debug.LogWarning("UI: CharacterControl 정보 개수와 캐릭터 관련 스프라이트 개수가 일치하지 않습니다.");
        }


        for (int i = 0; i < totalcharacterNum; i++)
        {
            _visibleCharacterOrder.Add(i);
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

            moveCharacterOutsideScreen(i);


        }


        

        //characterDescription preprocessing
        _characters = characterControl.getCharacterData();

        StartCoroutine(_moveCharacterDescriptionCenter());

        
    }

    

    // Update is called once per frame
    void Update()
    {

        //Debug.Log($"_visibleCharacterGoalXpos: {string.Join(", ", _visibleCharacterGoalXpos)}");


        //Badge UI
        float xPosB = _basicWorkingBadgeXpos; 
        
        for (int i = _visibleWorkingBadgeOrder.Count; i > 0; i--)
        {
            Transform tf = _workingBadgeSprites[_visibleWorkingBadgeOrder[i-1]].transform;
            tf.position = new Vector3(xPosB, _basicWorkingBadgeYpos, 0f);
            xPosB -= _workingBadgesSpacing;
        }


        


        //Character, Dot Bar, Character Description UI
        TextMeshPro tmp = _characterDescriptionText.GetComponent<TextMeshPro>();

        if (_visibleCharacterOrder.Count > 0)
        {
            

            for (int i = 0; i < _visibleCharacterOrder.Count; i++)
            {
                Transform tf = _characterSprites[_visibleCharacterOrder[i]].transform;

                Vector3 pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleCharacterGoalXpos[i], Time.deltaTime * _characterMoveSpeed);
                tf.position = pos;

                
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

