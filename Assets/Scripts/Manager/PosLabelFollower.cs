using UnityEngine;
using TMPro;

public class PosLabelFollower : MonoBehaviour
{
    [SerializeField] private TextMeshPro _labelPrefab;
    [SerializeField] private string _labelsRootName = "LabelsRoot";
    [SerializeField] private bool _billboardToCamera = true;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2f, 0);
    [SerializeField] private float _fontSizeOverride = -1f;
    private Transform _target;
    private TextMeshPro _text;
    private Camera _cam;
    private static Transform s_labelsRoot;

    public void Bind(Transform target)
    {
        _target = target;

        EnsureLabel();
        UpdateNow();
    }

    public void Bind(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;

        EnsureLabel();
        UpdateNow();
    }
    private void Awake()
    {
        _cam = Camera.main;
    }
    private void LateUpdate()
    {
        if (_target == null)
        {
            CleanupLabel();
            return;
        }

        EnsureLabel();
        UpdateNow();
    }
    private void EnsureLabel()
    {
        if (_text != null)
        {
            return;
        }

        if (_labelPrefab == null)
        {
            Debug.LogError(nameof(PosLabelFollower) + " en " + name + ": falta asignar _labelPrefab.");
            return;
        }

        Transform root = GetOrCreateLabelsRoot();
        _text = Instantiate(_labelPrefab, root);
        _text.name = "PosLabel";
        _text.raycastTarget = false;

        if (_fontSizeOverride > 0f)
        {
            _text.fontSize = _fontSizeOverride;
        }
    }
    private void UpdateNow()
    {
        Transform t = _text.transform;

        t.position = _target.position + _offset;

        Vector2 p = _target.position;
        _text.text = "<" + p.x.ToString("0.00") + ", " + p.y.ToString("0.00") + ">";

        if (_billboardToCamera)
        {
            if (_cam == null)
            {
                _cam = Camera.main;
            }

            if (_cam != null)
            {
                t.rotation = _cam.transform.rotation;
            }
        }
    }
    private static Transform GetOrCreateLabelsRoot()
    {
        if (s_labelsRoot != null)
        {
            return s_labelsRoot;
        }

        GameObject existing = GameObject.Find("LabelsRoot");
        if (existing == null)
        {
            existing = GameObject.Find("PosLabelsRoot");
        }
        if (existing == null)
        {
            existing = GameObject.Find("PosLabelRoot");
        }
        if (existing != null)
        {
            s_labelsRoot = existing.transform;
            return s_labelsRoot;
        }

        GameObject root = new GameObject("LabelsRoot");
        s_labelsRoot = root.transform;
        return s_labelsRoot;
    }
    private void CleanupLabel()
    {
        if (_text != null)
        {
            Destroy(_text.gameObject);
            _text = null;
        }
    }
    private void OnDestroy()
    {
        CleanupLabel();
    }
}