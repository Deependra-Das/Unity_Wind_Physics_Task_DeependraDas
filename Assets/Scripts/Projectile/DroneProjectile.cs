using UnityEngine;

public class DroneProjectile : MonoBehaviour
{
    private Rigidbody _projectileRB;
    private ProjectilePoolService _projectilePoolService;

    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
    }

    public void Initialize(ProjectilePoolService projectilePoolService, Vector3 droneLinearVelocity, float dropSpeed)
    {
        _projectilePoolService = projectilePoolService;

        _projectileRB.linearVelocity = droneLinearVelocity;
        _projectileRB.linearVelocity += Vector3.down * dropSpeed;
    }

    private void OnTriggerEnter(Collider target)
    {
        PatrolEnemy enemy = target.GetComponentInParent<PatrolEnemy>();

        if (enemy != null)
        {
            Debug.Log("Drone projectile hit enemy.");
            GameplayManager.Instance.DetroyEnemy(enemy);

            ReturnToPool();
            return;
        }

        if (target.CompareTag("Target"))
        {
            Debug.Log($"Drone projectile hit target: {target.gameObject.name}");
            GameplayManager.Instance.DestroyTarget(target.gameObject);

            ReturnToPool();
            return;
        }

        if (target.CompareTag("Ground"))
        {
            Debug.Log($"Drone projectile hit Groiund: {target.gameObject.name}");

            ReturnToPool();
            return;
        }
    }

    private void ReturnToPool()
    {
        if (_projectilePoolService == null)
        {
            Debug.LogError("DroneProjectile: ProjectilePoolService is null.");
            return;
        }

        _projectilePoolService.ReturnDroneProjectile(this);
    }

    private void OnDisable()
    {
        if (_projectileRB == null)
            return;

        _projectileRB.linearVelocity = Vector3.zero;
        _projectileRB.angularVelocity = Vector3.zero;
    }
}
