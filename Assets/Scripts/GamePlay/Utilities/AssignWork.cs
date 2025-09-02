using UnityEngine;
using System.Collections.Generic;

public enum StateType
{
    nothingChosen,
    characterChosen
}

public class AssignWork : MonoBehaviour
{

    

    [SerializeField] private CharacterDataMgr characterDataMgr;
    [SerializeField] private WorkDataMgr workDataMgr;

    
    [SerializeField] private CharacterSelectionUi characterSelectionUi;


    private StateType _state = StateType.nothingChosen;

    
    private int _chosenCharacterNum = 0;
    private int _chosenWorkNum = 0;
    private int _totalWorkNum = 0;


    private List<int> _copyCharacterDataLoc = new List<int>();

    public void addCompletedCharacter(int n)
    {
        
        _copyCharacterDataLoc.Add(n);
    }

    public void assignWorkToCharacter()
    {
        WorkData result = workDataMgr.popWork(_chosenWorkNum);
        characterSelectionUi.popWork(_chosenWorkNum);
        characterDataMgr.setAssignment(_copyCharacterDataLoc[_chosenCharacterNum], result.workName, result.workAmount);
        characterSelectionUi.beWorkingCharacterUI(_copyCharacterDataLoc[_chosenCharacterNum]);
        _copyCharacterDataLoc.RemoveAt(_chosenCharacterNum);
        //Debug.Log(AssignWork: _chosenCharacterNum + " " + _copyCharacterDataLoc[_chosenCharacterNum]);
    }

    public int getChosenCharacterNum() { return _chosenCharacterNum; }


    private void switchStateByZ()
    {
        switch (_state)
        {
            case StateType.nothingChosen:
                _state = StateType.characterChosen;
                characterSelectionUi.hightlightTask(0);
                break;
            case StateType.characterChosen:
                assignWorkToCharacter();
                _state = StateType.nothingChosen;
                
                if (_chosenCharacterNum == _copyCharacterDataLoc.Count)
                {
                    _chosenCharacterNum = 0;
                }
                
                _chosenWorkNum = 0;
                characterSelectionUi.noHightLightAnyTask();
                //Debug.Log($"AssignWork: before error, _chosenCharacterNum: {_chosenCharacterNum}, _copyCharacterDataLoc: {string.Join(", ", _copyCharacterDataLoc)} ");

                break;
            default:
                break;
        }
    }

    private void switchStateByX()
    {
        switch (_state)
        {
            case StateType.nothingChosen:
                break;
            case StateType.characterChosen:
                _state = StateType.nothingChosen;
                _chosenWorkNum = 0;
                characterSelectionUi.noHightLightAnyTask();
                break;
            default:
                break;
        }
    }

    private void PlusCharacterOrTask()
    {
        if (_state == StateType.nothingChosen)
        {
            if(_chosenCharacterNum < _copyCharacterDataLoc.Count - 1)
            {
                _chosenCharacterNum += 1;
                characterSelectionUi.moveCharacterLeft();
            } else
            {
                _chosenCharacterNum = 0;
                characterSelectionUi.moveCharacterToStart();
            }

        }
        else if (_state == StateType.characterChosen)
        {
            if (_totalWorkNum > 5)
            {
                if (_chosenWorkNum < 4)
                {
                    _chosenWorkNum += 1;
                }
                else
                {
                    _chosenWorkNum = 0;
                }
            }
            else
            {
                if (_chosenWorkNum < _totalWorkNum - 1)
                {
                    _chosenWorkNum += 1;
                }
                else
                {
                    _chosenWorkNum = 0;
                }
            }
            characterSelectionUi.hightlightTask(_chosenWorkNum);


        }
    }

    private void MinusCharacterOrTask()
    {
        if (_state == StateType.nothingChosen)
        {
            if (_chosenCharacterNum > 0)
            {
                _chosenCharacterNum -= 1;
                characterSelectionUi.moveCharacterRight();
            }
            else
            {
                _chosenCharacterNum = _copyCharacterDataLoc.Count- 1;
                characterSelectionUi.moveCharacterToEnd();
            }
            
        }
        else if (_state == StateType.characterChosen)
        {
            if (_totalWorkNum > 5)
            {
                if (_chosenWorkNum > 0)
                {
                    _chosenWorkNum -= 1;
                }
                else
                {
                    _chosenWorkNum = 4;
                }
            }
            else
            {
                if (_chosenWorkNum > 0)
                {
                    _chosenWorkNum -= 1;
                }
                else
                {
                    _chosenWorkNum = _totalWorkNum - 1;
                }
            }
            characterSelectionUi.hightlightTask(_chosenWorkNum);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        

        for (int i = 0; i < characterDataMgr.getTotalCharacterNum(); i++)
        {
            _copyCharacterDataLoc.Add(i);
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log($"AssignWork: _chosenCharacterNum: {_chosenCharacterNum}, _copyCharacterDataLoc: {string.Join(", ", _copyCharacterDataLoc)} ");
        if (Input.GetKeyDown(KeyCode.Z) && _totalWorkNum > 0 && _copyCharacterDataLoc.Count > 0)
        {
            switchStateByZ();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            switchStateByX();
        }
        
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlusCharacterOrTask();
        } else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MinusCharacterOrTask();
        }


        _totalWorkNum = workDataMgr.getLeftWork();

        //Debug.Log("AssignWork: 현재 state: " + _state + " 현재 선택 캐릭: " + _chosenCharacterNum + " 현재 선택 업무: " + _chosenWorkNum);
        //Debug.Log("AssignWork: _copyCharacterDataLoc: " + string.Join(", ", _copyCharacterDataLoc));

    }
}
