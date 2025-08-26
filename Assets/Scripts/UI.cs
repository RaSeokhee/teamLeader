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
    //TO DO: 캐릭터 정보와 스프라이트 개수가 동일하게 맞춰진다는 가정 하. 추후 개수에 따라 랜덤으로 배정하는 스크립트 추가 필요.
    public GameObject[] _characterSprites;

    public void beVisible(int i)
    {
        _characterSprites[i].gameObject.SetActive(true);
    }

    public void aintVisible(int i)
    {
        _characterSprites[i].gameObject.SetActive(false);
    }


    void Start()
    {
        workControl = workControlObject.GetComponent<WorkControl>();
        characterControl = characterControlObject.GetComponent<CharacterControl>();

        if (_characterSprites.Length != characterControl.getTotalCharacterNum())
        {
            Debug.LogWarning("UI: CharacterControl 정보 개수와 캐릭터 스프라이트 개수가 일치하지 않습니다.");
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
        } else
        {
            
            for (int i = 0;i < _works.Count; i++)
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


    }
}
