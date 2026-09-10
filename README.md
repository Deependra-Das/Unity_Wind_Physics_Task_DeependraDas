# Unity_Wind_Physics_Task_DeependraDas
Simulation prototype developed in Unity

## Controls
- Use keyboard (WASD + Q/E to ascend/descend)

## Features implemented
- Drone Movement using input with missile drops
- Patrol Enemy AI using waypoints & shooting at target
- Drone Detection Zone logic that can set target for the enemies that it references (1 for now in prototype)
- Particle Vfx for explosion
- Audio System to play audio based on type

## Known bugs or limitations
- Currently the detection zone can only reference one enemy for prototype but it can be scaled using a list of enemies
- Movement logic is kept simple without any tilt logic for realistic controls.
- Patrol Enemy is just a capsule not a soldier model and shoots only bullets.

## Script/Component Structure
- GameManager is the root script that initializes and registers services using Service Locator and injects dependencies required.
- GameplayManager controls the spawning logic of drone, patrol enemy, detection zone along with cleanup of enemies & targets.
- DroneController handles player input for drone movement using configurable input asset, movement logic and missile dropping logic.
- PlayerManager handles football spawning, respawning.
- PatrolEnemy handles patrolling movement, aiming & shooting at the drone when target is set by Detection Zone.
- EnemyDetectionZone handles detecting player using a trigger collider and sets the target for the enemies that it references for shooting.
- ProjectilePoolService is pooling projectiles (EnemyProjectile & DroneProjectile) for both PatrolEnemy & Drone then spawns them based on the position provided.
- VfxPoolService is pooling ExplosionVfx and spawns them based on the position provided.
- AudioManager handles playing audio clips based on the AudioTypeEnum value from the AudioData.
- ProjectilePoolService, VfxPoolService & AudioManager are being fed with data using scriptable objects Projectile_SO, Vfx_SO & Audio_SO.



