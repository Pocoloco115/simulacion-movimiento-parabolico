using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class CannonManager : MonoBehaviour
{
    private static readonly int PathAmountId = Shader.PropertyToID("_PathAmount");

    [SerializeField] private Slider _cannonDegreeSliderValue;
    [SerializeField] private Slider _shotPowerSliderValue;
    [SerializeField] private Transform _shotOrigin;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _aimLineRenderer;
    [SerializeField] private float _aimLineLength = 30f;
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private TargetController _target;
    private int _steps = 250;
    private float _timeInterval = 0.02f;
    private List<Vector3> _shotPathPoints = new List<Vector3>();
    private MaterialPropertyBlock _lineRendererPropertyBlock;
    private float _shotPathLength;
    void Start()
    {
        _lineRendererPropertyBlock = new MaterialPropertyBlock();
    }
    void Update()
    {
        UpdateCannonAngle();
        RenderAimLine();
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

    public void Fire()
    {
        float shotPower = HandleShotPower();
        float degree = GetCurrentCannonDegree();

        Vector3 shotDirection = new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad), 0);
        GameObject shot = Instantiate(_shotPrefab, _shotOrigin.position, Quaternion.identity);
        Rigidbody2D shotRigidbody = shot.GetComponent<Rigidbody2D>();
        shotRigidbody.linearVelocity = shotDirection * shotPower;
        _target.StartTargetBehaviour();
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
    private void RenderAimLine()
    {
        if (_aimLineRenderer == null) return;

        Vector3 origin = _shotOrigin.position;

        float degree = GetCurrentCannonDegree();
        Vector2 dir = new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));

        Vector3 end = origin + (Vector3)(dir * _aimLineLength);

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, _aimLineLength);
        if (hit.collider != null)
        {
            end = hit.point;
        }

        _aimLineRenderer.positionCount = 2;
        _aimLineRenderer.SetPosition(0, origin);
        _aimLineRenderer.SetPosition(1, end);
    }
}
