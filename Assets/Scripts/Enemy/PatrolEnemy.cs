using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pointReachDistance = 0.2f;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Combat")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float aimRotationSpeed = 180f;

    private int currentPointIndex;
    private Transform target;
    private float nextFireTime;
    private bool isInCombat => target != null;

    private void Update()
    {
        if (isInCombat)
        {
            Combat();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = targetPoint.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= pointReachDistance * pointReachDistance)
        {
            GoToNextPoint();
            return;
        }

        MoveTowards(direction);
        RotateTowards(direction);
    }

    private void MoveTowards(Vector3 direction)
    {
        Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void GoToNextPoint()
    {
        currentPointIndex++;

        if (currentPointIndex >= patrolPoints.Length)
        {
            currentPointIndex = 0;
        }
    }

    private void Combat()
    {
        if (target == null)
            return;

        RotateTowardsTarget();
        AimFirePointAtTarget();
        TryShoot();
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, aimRotationSpeed * Time.deltaTime);
    }

    private void AimFirePointAtTarget()
    {
        Vector3 direction = target.position - firePoint.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        firePoint.rotation = Quaternion.RotateTowards(firePoint.rotation, targetRotation, aimRotationSpeed * Time.deltaTime);
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        if (!IsFirePointAimed())
            return;

        Shoot();

        nextFireTime = Time.time + (1f / fireRate);
    }

    private bool IsFirePointAimed()
    {
        Vector3 direction = target.position - firePoint.position;

        if (direction.sqrMagnitude < 0.001f)
            return false;

        float angle = Vector3.Angle(firePoint.forward, direction.normalized);

        return angle <= 5f;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        EnemyProjectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Launch(firePoint.forward, projectileSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        nextFireTime = Time.time;
    }

    public void ClearTarget()
    {
        target = null;
    }


    private void OnDrawGizmosSelected()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null)
                continue;

            Gizmos.DrawSphere(patrolPoints[i].position, 0.25f);

            int nextIndex = (i + 1) % patrolPoints.Length;

            if (patrolPoints[nextIndex] != null)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[nextIndex].position);
            }
        }
    }
}
