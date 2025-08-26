using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int _workSpeed; //random between 30-60
    [SerializeField] private int _uniqueNum;  
    private int _completedWork = 0;
    private string _workName;
    private int _workAmount;
    private bool _isWork;
    private float _timer = 0f;

    public void setAssignment(string workName, int workAmount)
    {
        _workAmount = workAmount;
        _workName = workName;
        _isWork = true;
        _timer = 0f;

    }

    public int getWorkAmount()
    {
        return _workAmount;
    }

    public string getWorkName()
    {
        return _workName;
    }

    public int getUniqueNum()
    {
        return _uniqueNum;
    }

    void Start()
    {
        if (string.IsNullOrEmpty(_name) || _workSpeed == 0 || _uniqueNum == 0)
        {
            Debug.Log("CharacterBase: _name 또는 _workSpeed 또는 _uniqueNum가 설정되지 않음. 임시로 값을 저장");
            _name = "temp";
            _workSpeed = 50;
            _uniqueNum = Random.Range(1000, 2000);
        }

        if (_uniqueNum <= 0)
        {
            Debug.Log($"CharacterBase: _uniqueNum은 양수여야 합니다. 현재값: {_uniqueNum} ");
            _uniqueNum = Random.Range(1000, 2000);
        }
    }


    void Update()
    {
        if (_isWork)
        {
            if (_workAmount > 0 && _workSpeed > 0)
            {
                _timer += Time.deltaTime;
                if (_timer >= 1f)
                {
                    _workAmount -= _workSpeed / 10;
                    _timer = 0f;
                }
            }
            else
            {
                _isWork = false;
                _workAmount = 0;
                _completedWork += 1;
            }


        }

        //Debug.Log(_workAmount);
        //Debug.Log("끝낸일: " + _completedWork);
    }
}
