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

        if (TargetController.ActiveTarget != null)
        {
            return TargetController.ActiveTarget;
        }

        Vector3 spawnPosition = TargetController.HasInitialSpawnPosition
            ? TargetController.InitialSpawnPosition
            : Vector3.zero;

        TargetController spawnedTarget = Instantiate(_targetPrefab, spawnPosition, Quaternion.identity);
        return spawnedTarget;
    }
}