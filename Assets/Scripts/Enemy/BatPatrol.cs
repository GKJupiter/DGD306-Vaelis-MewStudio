using UnityEngine;

public class BatPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform currentPoint;

    void Start()
    {
        currentPoint = pointB;
    }

    void Update()
    {
        // Move toward currentPoint
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);

        // When close enough, switch direction
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            currentPoint = (currentPoint == pointA) ? pointB : pointA;
        }
    }
}
