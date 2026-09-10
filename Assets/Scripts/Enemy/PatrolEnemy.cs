using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _pointReachDistance = 0.2f;

    [Header("Combat")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float aimRotationSpeed = 180f;

    private Transform[] _patrolPoints;
    private int _currentPointIndex;
    private Transform _target;
    private float _nextFireTime;
    private bool _isInCombat => _target != null;
    private bool _isInitialized = false;

    private ProjectilePoolService _projectilePoolServiceObj;

    public void Initialize(ProjectilePoolService projectilePoolServiceObj, Transform[] patrolPoints)
    {
        _projectilePoolServiceObj = projectilePoolServiceObj;
        _patrolPoints = patrolPoints;
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized)
        {
            if (_isInCombat)
            {
                Combat();
            }
            else
            {
                Patrol();
            }
        }
    }

    private void Patrol()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0) return;

        Transform targetPoint = _patrolPoints[_currentPointIndex];
        Vector3 direction = targetPoint.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= _pointReachDistance * _pointReachDistance)
        {
            GoToNextPoint();
            return;
        }

        MoveTowards(direction);
        RotateTowards(direction);
    }

    private void MoveTowards(Vector3 direction)
    {
        Vector3 movement = direction.normalized * _moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void GoToNextPoint()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _patrolPoints.Length)
        {
            _currentPointIndex = 0;
        }
    }

    private void Combat()
    {
        if (_target == null)
            return;

        RotateTowardsTarget();
        AimFirePointAtTarget();
        TryShoot();
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = _target.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, aimRotationSpeed * Time.deltaTime);
    }

    private void AimFirePointAtTarget()
    {
        Vector3 direction = _target.position - _firePoint.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        _firePoint.rotation = Quaternion.RotateTowards(_firePoint.rotation, targetRotation, aimRotationSpeed * Time.deltaTime);
    }

    private void TryShoot()
    {
        if (Time.time < _nextFireTime)
            return;

        if (!IsFirePointAimed())
            return;

        Shoot();

        _nextFireTime = Time.time + (1f / fireRate);
    }

    private bool IsFirePointAimed()
    {
        Vector3 direction = _target.position - _firePoint.position;

        if (direction.sqrMagnitude < 0.001f)
            return false;

        float angle = Vector3.Angle(_firePoint.forward, direction.normalized);

        return angle <= 5f;
    }

    private void Shoot()
    {
        EnemyProjectile projectile = _projectilePoolServiceObj.SpawnEnemyProjectile(_firePoint.position, _firePoint.rotation);
        if (projectile == null) return;
        AudioManager.Instance.PlaySFX(AudioTypeEnum.EnemyProjectile);
        projectile.Initialize();
        projectile.Launch(_firePoint.forward, projectileSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
        _nextFireTime = Time.time;
    }

    public void ClearTarget()
    {
        _target = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            if (_patrolPoints[i] == null)
                continue;

            Gizmos.DrawSphere(_patrolPoints[i].position, 0.25f);

            int nextIndex = (i + 1) % _patrolPoints.Length;

            if (_patrolPoints[nextIndex] != null)
            {
                Gizmos.DrawLine(_patrolPoints[i].position, _patrolPoints[nextIndex].position);
            }
        }
    }
}
