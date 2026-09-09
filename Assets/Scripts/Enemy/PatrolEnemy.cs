using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pointReachDistance = 0.2f;
    [SerializeField] private Transform[] patrolPoints;

    private int currentPointIndex;

    private void Update()
    {
        Patrol();
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
