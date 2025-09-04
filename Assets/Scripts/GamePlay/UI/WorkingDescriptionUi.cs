using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class WorkingDescriptionUi : MonoBehaviour
{
    public CharacterDataMgr characterDataMgr;
    private List<CharacterData> _characterData;
    private int _charNum = -1;
    private TextMeshPro _tmp;
    private float _basicXpos = 12.5f;
    private float _basicYpos = 1.0f;

    public void setCharNum(int charNum)
    {
        if (characterDataMgr != null)
        {
            _characterData = characterDataMgr.getCharacterData();
        } else
        {
            Debug.LogWarning($"WorkingDescriptionUi: 외부에서 CharacterDataMgr이 주어지지 않았습니다.");
        }

        if (charNum < 0 || charNum > _characterData.Count - 1) 
        {
            Debug.LogWarning($"WorkingDescriptionUi: setCharNum에 잘못된 값이 주어졌습니다. 주어진 값: {charNum}");
        } else
        {
            _charNum = charNum;
        }

        _tmp = GetComponent<TextMeshPro>();
        if (_tmp == null)
        {
            Debug.LogWarning("WorkingDescriptionUi: TextMeshPro 컴포넌트를 찾을 수 없습니다!");
        }

    }

    public void setDescriptionXpos(float xpos)
    {
        Vector3 position = transform.position;
        position.x = xpos;
        transform.position = position;
    }

    private void moveDescriptionOutsideScreen()
    {
        transform.position = new Vector3(_basicXpos, _basicYpos, 0f);
    }



    void Awake()
    {
        //if (characterDataMgr != null) { _characterData = characterDataMgr.getCharacterData(); }
        moveDescriptionOutsideScreen();
    }

    void Update()
    {
        if (_charNum > -1)
        {
            _tmp.text = $"characterName\n: {_characterData[_charNum].getCharacterName()}\nworkSpeed\n: {_characterData[_charNum].getWorkSpeed()}\nworkName\n:{_characterData[_charNum].getWorkName()}\nworkAmount\n:{_characterData[_charNum].getWorkAmount()}";
        }

    }
}
