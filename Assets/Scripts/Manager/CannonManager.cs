using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class CannonManager : MonoBehaviour
{
    [SerializeField] private Slider _cannonDegreeSliderValue;
    [SerializeField] private Slider _shotPowerSliderValue;
    [SerializeField] private Transform _shotOrigin;
    [SerializeField] private LineRenderer _lineRenderer;
    private int _steps = 250;
    private float _timeInterval = 0.02f;
    private List<Vector3> _shotPathPoints = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCannonAngle();
        CalculatePath();
        RenderPath();
    }
    private void UpdateCannonAngle()
    {
        transform.rotation = Quaternion.Euler(0, 0, _cannonDegreeSliderValue.value);
    }
    private void CalculatePath()
    {
        _shotPathPoints.Clear();

        float shotPower = HandleShotPower();
        float degree = GetCurrentCannonDegree();

        float Vx = Mathf.Cos(degree * Mathf.Deg2Rad) * shotPower;
        float Vy = Mathf.Sin(degree * Mathf.Deg2Rad) * shotPower;

        for(int i = 0; i < _steps; i++)
        {
            float t = i * _timeInterval;
            float x = Vx * t + _shotOrigin.position.x;
            float y = Vy * t - (0.5f * 9.81f * t * t) + _shotOrigin.position.y;
            _shotPathPoints.Add(new Vector3(x, y, 0));
        }
    }
    private float HandleShotPower()
    {
        return _shotPowerSliderValue.value;
    }
    private float GetCurrentCannonDegree()
    {
        return transform.rotation.eulerAngles.z;
    }
    private void RenderPath()
    {
        if(_lineRenderer == null)
        {
            return;
        }
        _lineRenderer.positionCount = _shotPathPoints.Count;
        _lineRenderer.SetPositions(_shotPathPoints.ToArray());
    }
}
