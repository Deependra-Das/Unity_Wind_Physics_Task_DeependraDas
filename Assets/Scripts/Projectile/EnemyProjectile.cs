using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float lifetime = 2.5f;

    private Rigidbody _projectileRB;
    private ProjectilePoolService _projectilePoolServiceObj;

    private Coroutine _returnCoroutine;

    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        _projectilePoolServiceObj = GameManager.Instance.Services.Get<ProjectilePoolService>();
    }

    public void Launch(Vector3 direction, float speed)
    {
        if (_projectileRB == null)
            return;

        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }

        _projectileRB.linearVelocity = direction.normalized * speed;

        _returnCoroutine = StartCoroutine(ReturnAfterLifetime());
    }

    private IEnumerator ReturnAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        ReturnToPool();
    }
    private void ReturnToPool()
    {
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }

        if (_projectilePoolServiceObj != null)
        {
            _projectilePoolServiceObj.ReturnEnemyProjectile(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (_projectileRB != null)
        {
            _projectileRB.linearVelocity = Vector3.zero;
            _projectileRB.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        DroneController drone = target.GetComponentInParent<DroneController>();

        if (drone!=null)
        {
            Debug.Log("Hit Drone");
            ReturnToPool();
        }
    }
}
