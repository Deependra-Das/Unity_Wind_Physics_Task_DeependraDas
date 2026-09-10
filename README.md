# Unity_Wind_Physics_Task_DeependraDas
Simulation prototype developed in Unity

-- Script/Component Structure
- GameManager is the root script that initializes and registers services using Service Locator and injects dependencies required.
- GameplayManager controls the spawning logic of drone, patrol enemy, detection zone along with cleanup of enemies & targets.
- DroneController handles player input for drone movement using configurable input asset, movement logic and missile dropping logic.
- PlayerManager handles football spawning, respawning.
- PatrolEnemy handles patrolling movement and and shooting at the drone when target is set by Detection Zone.
- EnemyDetectionZone handles detecting player using a trigger collider and sets the target for the enemies that it references for shooting.
- ProjectilePoolService is pooling projectiles (EnemyProjectile & DroneProjectile) for both PatrolEnemy & Drone then spawns them based on the position provided.
- VfxPoolService is pooling ExplosionVfx and spawns them based on the position provided.
- AudioManager handles playing audio clips based on the AudioTypeEnum value from the AudioData.
- ProjectilePoolService, VfxPoolService & AudioManager are being fed with data using scriptable objects Projectile_SO, Vfx_SO & Audio_SO.

