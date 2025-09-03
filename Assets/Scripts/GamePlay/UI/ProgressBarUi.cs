using UnityEngine;
using System.Collections.Generic;

public class ProgressBarUi : MonoBehaviour
{
    private GameObject _outBar;
    private GameObject _inBar;

    private int _workAmount;
    private int _workSpeed;
    private int _totalFillCount;
    private float _basicScaleX = 1.3f;
    private float _basicScaleY = 0.1f;
    private float _inBarScaleX = 0f;
    private float _basicXpos = 12.5f;
    private float _basicYpos = 2.5f;

    private float _timer = 0f;

    public void setWorkAmountAndSpeed(int workAmount, int workSpeed)
    {
        if (workAmount < 1 || workSpeed < 1) 
        { 
            Debug.LogWarning($"ProgressUi: setWorkAmountAndSpeed함수에 잘못된 값이 주어졌습니다. workAmount: {workAmount}, workSpeed: {workSpeed}"); 
        } else
        {
            _workAmount = workAmount;
            _workSpeed = workSpeed;
            _totalFillCount = _workAmount / _workSpeed;
            _inBar.transform.localScale = new Vector3(0f, _basicScaleY, 0f);
        }

    }

    public void setOutBarPosX(float x)
    {
        Vector3 position = _outBar.transform.position;
        position.x = x;
        _outBar.transform.position = position;
    }

    private void moveInBarTowardOutbar()
    {
        Vector3 position = _inBar.transform.position;
        position.x = _outBar.transform.position.x;
        _inBar.transform.position = position;
    }

    private void moveBarOutsideScreen()
    {
        Vector3 position = new Vector3(_basicXpos, _basicYpos, 0f);
        _outBar.transform.position = position;
        _inBar.transform.position = position;
    }
    
    void Awake()
    {
        Transform child = transform.Find("OutBar");
        if (child == null) { Debug.LogWarning($"ProgressUi: OutBar 오브젝트를 찾지 못함"); return; }
        _outBar = child.gameObject;
        _outBar.transform.localScale = new Vector3(_basicScaleX + 0.1f, _basicScaleY + 0.1f, 0f);


        child = transform.Find("InBar");
        if (child == null) { Debug.LogWarning($"ProgressUi: InBar 오브젝트를 찾지 못함"); return; }
        _inBar = child.gameObject;
        _inBar.transform.localScale = new Vector3(0f, _basicScaleY, 0f);

        moveBarOutsideScreen();

    }

    void Start()
    {

    }

   
    void Update()
    {
        //todo: outbar location manipulate, 

        if (_workSpeed > 0)
        {
            Vector3 scale = _inBar.transform.localScale;
            scale.x = _inBarScaleX;        
            _inBar.transform.localScale = scale;

            Vector3 position = _inBar.transform.position;
            position.x = _outBar.transform.position.x - (_basicScaleX + 0.1f) / 2 + 0.05f + _inBarScaleX / 2;
            _inBar.transform.position = position;

            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _workAmount -= _workSpeed; 
                if (_workAmount <= 0)
                {
                    scale = _inBar.transform.localScale;
                    scale.x = _basicScaleX; 
                    _inBar.transform.localScale = scale;

                    position = _inBar.transform.position;
                    position.x = _outBar.transform.position.x;
                    _inBar.transform.position = position;

                    _workSpeed = 0;
                    _workAmount = 0;
                    _timer = 0f;
                    _totalFillCount = 0;

                } else
                {
                    _inBarScaleX += (float)(1.3 / _totalFillCount);
                    _timer = 0f;
                }
                
            }

        } else
        {
            _timer = 0f;

            moveInBarTowardOutbar();
        }
    }
}
