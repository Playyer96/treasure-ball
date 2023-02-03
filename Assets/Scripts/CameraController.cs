using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float distance = 10f;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float targetZ = target.position.z - distance;
        float targetX = target.position.x;
        float targetY = target.position.y + distance;

        Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);
        float speed = (targetPosition - transform.position).magnitude * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);

        transform.LookAt(target);
    }

    public void AdjustDistance(float newDistance)
    {
        distance = newDistance;
    }
}