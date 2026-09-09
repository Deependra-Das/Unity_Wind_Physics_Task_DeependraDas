using Unity.Cinemachine;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [Header("Drone")]
    [SerializeField] private DroneController _dronePrefab;
    [SerializeField] private Transform _droneSpawnPoint;
    [SerializeField] private CinemachineCamera _droneFollowCamera;

    [Header("Enemy Detection")]
    [SerializeField] private EnemyDetectionZone _enemyDetectionZonePrefab;
    [SerializeField] private Transform _enemyDetectionZoneSpawnPoint;

    [Header("Enemy")]
    [SerializeField] private PatrolEnemy _patrolEnemyPrefab;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private Transform[] _patrolPoints;

    private PatrolEnemy _enemy;
    private EnemyDetectionZone _enemyDetectionZone;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Initialize(ProjectilePoolService projectilePoolService)
    {
        SpawnDrone();
        PatrolEnemy enemy = SpawnEnemy(projectilePoolService);
        SpawnEnemyDetectionZone(enemy);
    }

    public DroneController SpawnDrone()
    {
        DroneController drone = Instantiate(_dronePrefab, _droneSpawnPoint.position, _droneSpawnPoint.rotation);
        _droneFollowCamera.LookAt = drone.CameraTarget;
        _droneFollowCamera.Follow = drone.CameraTarget;
        return drone;
    }

    public PatrolEnemy SpawnEnemy(ProjectilePoolService projectilePoolService)
    {
        PatrolEnemy enemy = Instantiate(_patrolEnemyPrefab, _enemySpawnPoint.position, _enemySpawnPoint.rotation);
        enemy.Initialize(projectilePoolService, _patrolPoints);
        return enemy;
    }

    private void SpawnEnemyDetectionZone(PatrolEnemy enemy)
    {
        _enemyDetectionZone = Instantiate(_enemyDetectionZonePrefab, _enemyDetectionZoneSpawnPoint.position, _enemyDetectionZoneSpawnPoint.rotation);
        _enemyDetectionZone.Initialize(enemy);
    }

    public PatrolEnemy GetEnemy()
    {
        return _enemy;
    }
}
