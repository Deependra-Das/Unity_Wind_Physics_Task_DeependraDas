using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Projectile_SO _projectile_SO;
    [SerializeField] private Transform _enemyProjectilePoolParent;
    [SerializeField] private Transform _droneProjectilePoolParent;

    public static GameManager Instance { get; private set; }
    public ServiceLocator Services { get; private set; }

    private ProjectilePoolService _projectilePoolService;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeServices();
        RegisterServices();
        GameplayManager.Instance.Initialize(_projectilePoolService);
    }

    private void InitializeServices()
    {
        Services = new ServiceLocator();
        _projectilePoolService = new ProjectilePoolService(_projectile_SO, _enemyProjectilePoolParent, _droneProjectilePoolParent);
    }

    private void RegisterServices()
    {
        Services.Register(_projectilePoolService);
    }
}