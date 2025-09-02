using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class CharacterData
{
    public string characterName = "";
    public int workSpeed = 0;
    private bool isWork = false;
    private int completedWork = 0;
    private int dataLoc = 0;

    public void setIsWork(bool b) { isWork = b; }
    public void plusCompletedWork() { completedWork += 1; }
    public void setDataLoc(int n) { dataLoc = n; }
    public int getDataLoc() { return dataLoc; }
}

public class WorkingCharacterData
{
    private string characterName = "";
    private int workSpeed = 0;
    private string workName = "";
    private int workAmount = 0;
    private int dataLoc = 0;

    public void setCharacterName(string s) { characterName = s; }
    public string getCharacterName() {  return characterName; }

    public void setWorkName(string s) { workName = s; }

    public void setWorkSpeed(int n) { workSpeed = n; }
    public int getWorkSpeed() { return workSpeed; }

    public void setWorkAmount(int n) { workAmount = n; }
    public int getWorkAmount() { return workAmount; }

    public void setDataLoc(int n) { dataLoc = n; }
    public int getDataLoc() { return dataLoc; }

    public void minusAmountWithSpeed() { workAmount -= workSpeed; }
}

public class CharacterDataMgr : MonoBehaviour
{
    [SerializeField] private AssignWork assignWork;

    [SerializeField] private CharacterSelectionUi characterSelectionUi;

    private float _timer = 0f;

    [SerializeField] private List<CharacterData> _characters;
    private List<WorkingCharacterData> _workingCharacters = new List<WorkingCharacterData>();

    public List<CharacterData> getCharacterData()
    {
        return _characters;
    }

    public List<WorkingCharacterData> getWorkingCharacterData()
    {
        return _workingCharacters;
    }

    public int getTotalCharacterNum()
    {
        return _characters.Count;
    }
    
    public void setAssignment(int characterNum, string workName, int workAmount)
    {
        if (characterNum < 0 || characterNum > _characters.Count - 1)
        {
            Debug.Log($"CharacterControl: 잘못된 characterNum이 주어짐. 현재 주어진 값: {characterNum}");
            return;
        }

        _characters[characterNum].setIsWork(true);
        WorkingCharacterData newChar = new WorkingCharacterData();
        newChar.setCharacterName(_characters[characterNum].characterName);
        newChar.setWorkSpeed(_characters[characterNum].workSpeed / 10);
        newChar.setWorkName(workName);
        newChar.setWorkAmount(workAmount);
        newChar.setDataLoc(_characters[characterNum].getDataLoc());

        _workingCharacters.Add(newChar);
    }

    

    void Start()
    {
       
        

        for (int i = 0; i < _characters.Count; i++)
        {
            if (_characters[i].workSpeed <= 0 || _characters[i].workSpeed > 100)
            {
                Debug.Log($"CharacterControl: 캐릭터의 workSpeed 값은 0 초과 100 이하여야 합니다.현재 주어진 값: {_characters[i].workSpeed}");
                _characters[i].workSpeed = 50;
            }

            if (string.IsNullOrEmpty(_characters[i].characterName))
            {
                Debug.Log($"CharacterControl: 캐릭터의 characterName 값이 주어지지 않았습니다. 임시로 값 저장함");
                _characters[i].characterName = "temp";
            }

            _characters[i].setDataLoc(i);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_workingCharacters.Count > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1f)
            {
                for (int i = 0; i < _workingCharacters.Count; i++)
                {
                    if (_workingCharacters[i].getWorkAmount() > 0 && _workingCharacters[i].getWorkSpeed() > 0)
                    {
                        _workingCharacters[i].minusAmountWithSpeed();
                        _timer = 0f;
                        //Debug.Log($"CharacterControl: 현재 일 중인 캐릭터: {_workingCharacters[i].getCharacterName()}, 총 업무량: {_workingCharacters[i].getWorkAmount()}, 스피드: {_workingCharacters[i].getWorkSpeed()}");
                    }
                    else
                    {

                        _characters[_workingCharacters[i].getDataLoc()].setIsWork(true);
                        _characters[_workingCharacters[i].getDataLoc()].plusCompletedWork();
                        assignWork.addCompletedCharacter(_workingCharacters[i].getDataLoc());
                        characterSelectionUi.aintWorkingCharacterUI(_workingCharacters[i].getDataLoc());
                        _workingCharacters.RemoveAt(i);
                    }
                }
            }
        } else
        {
            _timer = 0f;
        }

        //Debug.Log(_workingCharacters.Count);

    }
}
