using UnityEngine;

[CreateAssetMenu(fileName = "Projectile_SO", menuName = "ScriptableObjects/Projectile_SO")]
public class Projectile_SO : ScriptableObject
{
    public EnemyProjectile enemyProjectilePrefab;
    public int enemyProjectilePoolSize;
    public DroneProjectile droneProjectilePrefab;
    public int droneProjectilePoolSize;
}
