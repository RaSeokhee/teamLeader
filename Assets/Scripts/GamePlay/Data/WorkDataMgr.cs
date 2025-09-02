using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WorkData
{
    public string workName = "";
    public int workAmount = 0;

}

public class WorkDataMgr : MonoBehaviour
{
    
    [SerializeField] private List<WorkData> _works;

    

    public int getLeftWork()
    {
        return _works.Count;
    }


    public List<WorkData> getWorkData()
    {
        return _works;
    }

    public WorkData popWork(int n)
    {
        if (n >= _works.Count || n < 0)
        {
            Debug.Log($"WorkControl: 리스트의 크기보다 작거나 큰 값이 주어졌습니다. 주어진 값: {n}");

            return null;
        }

        if (_works[n].workName != "" && _works[n].workAmount > 0)
        {
            WorkData result = new WorkData();
            result.workName = _works[n].workName;
            result.workAmount = _works[n].workAmount;

            _works.RemoveAt(n);
            return result;
        }
        else
        {
            Debug.Log($"WorkControl: {n}번째에 해당하는 배열이 없습니다.");
            return null;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _works.Count; i++)
        {
            if (_works[i].workAmount <= 0 || _works[i].workAmount > 100)
            {
                Debug.Log($"WorkControl: workAount의 값은 0 초과 100 이하여야 합니다.현재 주어진 값: {_works[i].workAmount}");
                _works[i].workAmount = 50;
            }
        }


        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
