using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    private float rotationSpeedMultiplier = 5f;
    public float acceleratingSpeed = 4f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void MoveTowardsTarget(Transform target)
    {
        Vector3 movingDirection = (target.position - transform.position).normalized;
        rb.velocity = movingDirection * (movementSpeed);
    }
    public void AccelerateTowardsTarget(Vector3 target)
    {
        Vector3 movingDirection = (target - transform.position).normalized;
        rb.velocity = movingDirection * (acceleratingSpeed);
    }
    public void RotateTowardsTarget(Transform target, float rotationSpeed)
    {
        Vector3 movingDirection = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(movingDirection.x, 0, movingDirection.z)) * Quaternion.Euler(0, 90, 0); ;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier * rotationSpeed);
    }
}
