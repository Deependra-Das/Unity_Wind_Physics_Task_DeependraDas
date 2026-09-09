using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    private PatrolEnemy _patrolEnemy;

    public void Initialize(PatrolEnemy patrolEnemy)
    {
        _patrolEnemy = patrolEnemy;
    }

    private void OnTriggerEnter(Collider target)
    {
        DroneController drone = target.gameObject.GetComponent<DroneController>();

        if (drone == null)
        {
            return;
        }    

        _patrolEnemy.SetTarget(drone.transform);
    }

    private void OnTriggerExit(Collider target)
    {
        DroneController drone = target.gameObject.GetComponent<DroneController>();

        if (drone == null)
        {
            return;
        }

        _patrolEnemy.ClearTarget();
    }
}
