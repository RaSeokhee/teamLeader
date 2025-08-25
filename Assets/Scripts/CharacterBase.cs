using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int _workSpeed; //random between 30-60
    private int _completedWork = 0;
    private int _assignment;
    private bool _isWork;
    private float _timer = 0f;

    public void setAssignment(int assignment)
    {
        _assignment = assignment;
        _isWork = true;
        _timer = 0f;

    }

    public int getAssignment()
    {
        return _assignment;
    }


    void Start()
    {
        if (string.IsNullOrEmpty(_name) || _workSpeed == 0)
        {
            Debug.Log("CharacterBase: _name 또는 _workSpeed가 설정되지 않음. 임시로 값을 저장");
            _name = "temp";
            _workSpeed = 50;
        }
    }


    void Update()
    {
        if (_isWork)
        {
            if (_assignment > 0 && _workSpeed > 0)
            {
                _timer += Time.deltaTime;
                if (_timer >= 1f)
                {
                    _assignment -= _workSpeed / 10;
                    _timer = 0f;
                }
            }
            else
            {
                _isWork = false;
                _assignment = 0;
                _completedWork += 1;
            }


        }

        //Debug.Log(_assignment);
        //Debug.Log("끝낸일: " + _completedWork);
    }
}
