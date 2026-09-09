using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float lifetime = 2.5f;

    private Rigidbody _projectileRB;

    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float speed)
    {
        if (_projectileRB == null)
            return;

        _projectileRB.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider target)
    {
        DroneController drone = target.GetComponentInParent<DroneController>();

        if (drone!=null)
        {
            Debug.Log("Hit Drone");
            Destroy(gameObject);
            return;
        }
    }
}
