using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : PlatformController
{
    public float speed = 5.0f;
    public float waitTime = 0.5f;
    public Transform[] wayPoints;
    public Transform transform;

    private float patrolTimer = 0f;
    private int wayPointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, wayPoints[wayPointIndex].position) < .1f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= waitTime)
            {
                if (wayPointIndex < wayPoints.Length - 1)
                {
                    wayPointIndex++;
                    transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex].position, speed * Time.deltaTime);
                }
                else
                {
                    wayPointIndex = 0;
                    transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex].position, speed * Time.deltaTime);
                }
                patrolTimer = 0f;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex].position, speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
            other.gameObject.transform.SetParent(gameObject.transform, true);
    }

    void OnCollisionExit2D(Collision2D other)
    {
            other.gameObject.transform.parent = null;
    }
}
