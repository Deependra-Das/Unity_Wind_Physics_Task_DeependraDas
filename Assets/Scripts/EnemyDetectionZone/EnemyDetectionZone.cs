using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    [SerializeField] private PatrolEnemy _patrolEnemy;

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
