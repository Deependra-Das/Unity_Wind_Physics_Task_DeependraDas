using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolService
{
    private readonly EnemyProjectile _enemyProjectilePrefab;
    private readonly DroneProjectile _droneProjectilePrefab;

    private readonly Transform _enemyProjectileParent;
    private readonly Transform _droneProjectileParent;

    private readonly Queue<EnemyProjectile> _enemyProjectilePool = new();
    private readonly Queue<DroneProjectile> _droneProjectilePool = new();

    public ProjectilePoolService(Projectile_SO projectile_SO, Transform enemyProjectileParent, Transform droneProjectileParent)
    {
        _enemyProjectilePrefab = projectile_SO.enemyProjectilePrefab;
        _droneProjectilePrefab = projectile_SO.droneProjectilePrefab;

        _enemyProjectileParent = enemyProjectileParent;
        _droneProjectileParent = droneProjectileParent;

        InitializeEnemyProjectilePool(projectile_SO.enemyProjectilePoolSize);
        InitializeDroneProjectilePool(projectile_SO.droneProjectilePoolSize);
    }

    private void InitializeEnemyProjectilePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            EnemyProjectile projectile = CreateEnemyProjectile();

            if (projectile != null)
            {
                _enemyProjectilePool.Enqueue(projectile);
            }
        }
    }

    private void InitializeDroneProjectilePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            DroneProjectile projectile = CreateDroneProjectile();

            if (projectile != null)
            {
                _droneProjectilePool.Enqueue(projectile);
            }
        }
    }

    private EnemyProjectile CreateEnemyProjectile()
    {
        EnemyProjectile projectile = Object.Instantiate(_enemyProjectilePrefab, _enemyProjectileParent);

        projectile.gameObject.SetActive(false);
        return projectile;
    }

    private DroneProjectile CreateDroneProjectile()
    {
        DroneProjectile projectile = Object.Instantiate(_droneProjectilePrefab, _droneProjectileParent);

        projectile.gameObject.SetActive(false);
        return projectile;
    }

    public EnemyProjectile SpawnEnemyProjectile(Vector3 position, Quaternion rotation)
    {
        EnemyProjectile projectile;

        if (_enemyProjectilePool.Count > 0)
        {
            projectile = _enemyProjectilePool.Dequeue();
        }
        else
        {
            projectile = CreateEnemyProjectile();
        }

        if (projectile == null)
            return null;

        projectile.transform.SetPositionAndRotation(position, rotation);
        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void ReturnEnemyProjectile(EnemyProjectile projectile)
    {
        if (projectile == null)
            return;

        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(_enemyProjectileParent);

        _enemyProjectilePool.Enqueue(projectile);
    }

    public DroneProjectile SpawnDroneProjectile(Vector3 position, Quaternion rotation)
    {
        DroneProjectile projectile;

        if (_droneProjectilePool.Count > 0)
        {
            projectile = _droneProjectilePool.Dequeue();
        }
        else
        {
            projectile = CreateDroneProjectile();
        }

        if (projectile == null)
            return null;

        projectile.transform.SetPositionAndRotation(position, rotation);

        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void ReturnDroneProjectile(DroneProjectile projectile)
    {
        if (projectile == null)
            return;

        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(_droneProjectileParent);

        _droneProjectilePool.Enqueue(projectile);
    }
}