using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class CharacterSelectionUi : MonoBehaviour
{
    [SerializeField] private AssignWork assignWork;
    [SerializeField] private CharacterDataMgr characterDataMgr;

    //TaskUI
    [SerializeField] private TaskUi taskUi;
    public void popWork(int index) { taskUi.popWork(index); }
    public void hightlightTask(int index) { taskUi.hightlightTask(index); }
    public void noHightLightAnyTask() { taskUi.noHightLightAnyTask(); }

    //WorkEndAlarmUI
    [SerializeField] private WorkEndAlarmUi workEndAlarmUi;

    //Character UI
    [SerializeField] private GameObject[] _characterSprites;
    private List<int> _visibleCharacterOrder = new List<int>();
    private List<float> _visibleCharacterGoalXpos = new List<float>();

    private float _basicCharacterXpos = 15f;
    private float _basicCharacterYpos = 1.5f;
    private float _charactersSpacing = 15f;
    private float _characterMoveSpeed = 6f;
    private float _chosenCharacterXpos = 2f;

    //WorkingBadge UI
    [SerializeField] private WorkingBadgeUi workingBadgeUi;
    private void addVisibleWorkingBadgeAndProgressBar(int badgeNum, int workAmount, int workSpeed) { workingBadgeUi.addVisibleWorkingBadgeAndProgressBar(badgeNum, workAmount, workSpeed); }
    private void removeVisibleWorkingBadgeOrder(int badgeNum) { workingBadgeUi.removeVisibleWorkingBadgeOrder(badgeNum); }
    private void moveWorkingBadgeOutsideScreen(int spriteNum) { workingBadgeUi.moveWorkingBadgeOutsideScreen(spriteNum); }

    //character description UI
    [SerializeField] private GameObject _characterDescriptionText;
    private List<CharacterData> _characters;

    //CharacterDotBar UI
    [SerializeField] private GameObject _dotBarDotPrefab;
    [SerializeField] private GameObject _dotBarParent;
    private List<GameObject> _characaterDotSprites = new List<GameObject>();
    private List<float> _visibleDotGoalXpos = new List<float>();
    private int _previousChosenDot = -1;

    private float _basicDotScale = 0.2f;
    private float _bigDotSclae = 0.3f;
    private float _characterDotSpacing = 1f;
    private float _basicCharacterDotXpos = 12f;
    private float _basicCharacterDotYpos = 4f;
    private float _dotMoveSpeed = 6f;

    private List<int> _visibleDotOrder = new List<int>();


    //characterDotBar function
    private void moveDotOutsideScreen(int dotNum)
    {
        Transform tf = _characaterDotSprites[dotNum].transform;
        tf.position = new Vector3(_basicCharacterDotXpos, _basicCharacterDotYpos, 0f);

        tf.localScale = new Vector3(_basicDotScale, _basicDotScale, 0f);
    }

    private void highlightDot(int dotNum)
    {
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
    }

    
    //character function
    public void beWorkingCharacterUI(int i, int workAmount, int workSpeed)
    {
        if (Mathf.Abs(_chosenCharacterXpos - _visibleCharacterGoalXpos[_visibleCharacterGoalXpos.Count - 1]) < 0.0001f)  
        {
            moveCharacterToStart();
            _visibleCharacterGoalXpos.RemoveAt(_visibleCharacterGoalXpos.Count - 1);
            moveCharacterOutsideScreen(i);

            //Character Dot Bar UI
            _visibleDotGoalXpos.RemoveAt(_visibleDotGoalXpos.Count - 1);
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
        _visibleDotOrder.Remove(i);
        _characterSprites[i].gameObject.SetActive(false);

        addVisibleWorkingBadgeAndProgressBar(i, workAmount, workSpeed);


        if (_visibleCharacterOrder.Count < 1) // if count is zero
        {
            _previousChosenDot = -1;
        }
    }

    public void aintWorkingCharacterUI(int i)
    {
        _visibleCharacterOrder.Add(i);
        _visibleDotOrder.Add(i);
        removeVisibleWorkingBadgeOrder(i);
        workEndAlarmUi._addWorkEndCharacterToList(i);
        _characterSprites[i].gameObject.SetActive(true);
        moveWorkingBadgeOutsideScreen(i);

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

    

    void Start()
    {
        _characters = characterDataMgr.getCharacterData();
        int totalcharacterNum = characterDataMgr.getTotalCharacterNum();

        for (int i = 0; i < totalcharacterNum; i++)
        {
            GameObject obj = Instantiate(_dotBarDotPrefab); 
            obj.transform.SetParent(_dotBarParent.transform);
            _characaterDotSprites.Add(obj);
        }

        if (_characterSprites.Length != totalcharacterNum)
        {
            Debug.LogWarning("UI: CharacterControl 정보 개수와 캐릭터 관련 스프라이트 개수가 일치하지 않습니다.");
        }

        if (_characaterDotSprites.Count != totalcharacterNum)
        {
            Debug.LogWarning("CharacterDotBarUI: CharacterControl 정보 개수와 dot bar 스프라이트 개수가 일치하지 않습니다.");
        }

        for (int i = 0; i < totalcharacterNum; i++)
        {
            _visibleCharacterOrder.Add(i);
            _visibleCharacterGoalXpos.Add(_chosenCharacterXpos + i * _charactersSpacing);

            moveCharacterOutsideScreen(i);

            _visibleDotOrder.Add(i);
            _visibleDotGoalXpos.Add(2f + i * 1f);
            moveDotOutsideScreen(i);
        }

        highlightDot(0);

        StartCoroutine(_moveCharacterDescriptionCenter());
    }

    

    // Update is called once per frame
    void Update()
    {
        TextMeshPro tmp = _characterDescriptionText.GetComponent<TextMeshPro>();

        if (_visibleCharacterOrder.Count > 0)
        {

            for (int i = 0; i < _visibleCharacterOrder.Count; i++)
            {
                Transform tf = _characterSprites[_visibleCharacterOrder[i]].transform;

                Vector3 pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleCharacterGoalXpos[i], Time.deltaTime * _characterMoveSpeed);
                tf.position = pos;

                tf = _characaterDotSprites[_visibleDotOrder[i]].transform;
                pos = tf.position;
                pos.x = Mathf.Lerp(pos.x, _visibleDotGoalXpos[i], Time.deltaTime * _dotMoveSpeed);
                tf.position = pos;

                highlightDot(_visibleDotOrder[assignWork.getChosenCharacterNum()]);
            }

            if (tmp != null)
            {
                tmp.text = _characters[_visibleCharacterOrder[assignWork.getChosenCharacterNum()]].getCharacterName();
            }

        } else
        {
            if (tmp != null)
            {
                tmp.text = "";
            }
            
        }
    }
}

