using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class CannonManager : MonoBehaviour
{
    private static readonly int PathAmountId = Shader.PropertyToID("_PathAmount");

    [SerializeField] private Slider _cannonDegreeSliderValue;
    [SerializeField] private Slider _shotPowerSliderValue;
    [SerializeField] private Transform _shotOrigin;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _shotPrefab;
    private int _steps = 250;
    private float _timeInterval = 0.02f;
    private List<Vector3> _shotPathPoints = new List<Vector3>();
    private MaterialPropertyBlock _lineRendererPropertyBlock;
    private float _shotPathLength;
    private InputAction _fireAction;
    private void Awake()
    {
        _fireAction = new InputAction("Fire", binding: "<Mouse>/leftButton");
        _fireAction.performed += ctx => OnFireButtonPressed();
        _fireAction.Enable();
    }
    void Start()
    {
        _lineRendererPropertyBlock = new MaterialPropertyBlock();
    }
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
        _shotPathLength = 0f;

        float shotPower = HandleShotPower();
        float degree = GetCurrentCannonDegree();

        float Vx = Mathf.Cos(degree * Mathf.Deg2Rad) * shotPower;
        float Vy = Mathf.Sin(degree * Mathf.Deg2Rad) * shotPower;

        Vector3 previousPoint = Vector3.zero;
        bool hasPreviousPoint = false;

        for(int i = 0; i < _steps; i++)
        {
            float t = i * _timeInterval;
            float x = Vx * t + _shotOrigin.position.x;
            float y = Vy * t - (0.5f * 9.81f * t * t) + _shotOrigin.position.y;
            Vector3 currentPoint = new Vector3(x, y, 0);
            if(!IsPlaceFree(currentPoint))
            {
                break;
            }
            _shotPathPoints.Add(currentPoint);

            if(hasPreviousPoint)
            {
                _shotPathLength += Vector3.Distance(previousPoint, currentPoint);
            }

            previousPoint = currentPoint;
            hasPreviousPoint = true;
        }
    }
    private void OnFireButtonPressed()
    {
        float shotPower = HandleShotPower();
        float degree = GetCurrentCannonDegree();

        Vector3 shotDirection = new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad), 0);
        GameObject shot = Instantiate(_shotPrefab, _shotOrigin.position, Quaternion.identity);
        Rigidbody2D shotRigidbody = shot.GetComponent<Rigidbody2D>();
        shotRigidbody.linearVelocity = shotDirection * shotPower;
    }
    private float HandleShotPower()
    {
        return _shotPowerSliderValue.value;
    }
    private float GetCurrentCannonDegree()
    {
        return transform.rotation.eulerAngles.z;
    }
    private bool IsPlaceFree(Vector3 position)
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(position, 0.1f);
        return hitCollider == null;
    }
    private void RenderPath()
    {
        if(_lineRenderer == null)
        {
            return;
        }
        _lineRenderer.positionCount = _shotPathPoints.Count;
        _lineRenderer.SetPositions(_shotPathPoints.ToArray());

        if(_lineRendererPropertyBlock == null)
        {
            _lineRendererPropertyBlock = new MaterialPropertyBlock();
        }

        _lineRenderer.GetPropertyBlock(_lineRendererPropertyBlock);
        _lineRendererPropertyBlock.SetFloat(PathAmountId, Mathf.Max(1f, _shotPathLength));
        _lineRenderer.SetPropertyBlock(_lineRendererPropertyBlock);
    }
}
