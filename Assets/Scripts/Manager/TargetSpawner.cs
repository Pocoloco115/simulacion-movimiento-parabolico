using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner Instance { get; private set; }

    [SerializeField] private TargetController _targetPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public TargetController SpawnTarget()
    {
        if (_targetPrefab == null)
        {
            return null;
        }

        Vector3 spawnPosition = TargetController.ActiveTarget != null
            ? TargetController.ActiveTarget.transform.position
            : TargetController.LastKnownPosition;

        TargetController spawnedTarget = Instantiate(_targetPrefab, spawnPosition, Quaternion.identity);
        return spawnedTarget;
    }
}