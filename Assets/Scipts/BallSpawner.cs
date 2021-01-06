using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<GameObject> balls;

    public float spawnInterval = 1.5f;
    public float spawnRangeX = 12f;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] int numBalls;

    [SerializeField] private float startDelay = 2f;

    private const float SPAWN_POS_Y = 45f;

    // Start is called before the first frame update
    void Start()
    {
        // instantiate all of our balls into an object pool
        balls = new List<GameObject>();
        for (int i = 0; i < numBalls; i++)
        {
            GameObject newBall = (GameObject)Instantiate(ballPrefab);
            newBall.SetActive(false);
            newBall.transform.SetParent(this.transform); // set as children of Spawn Manager
            balls.Add(newBall);
        }
    }

    public void BeginEpisode()
    {
        // Cancel all invoked methods from previous episodes
        CancelInvoke();

        //Disable all balls
        foreach (GameObject ball in balls)
        {
            DeactivateBall(ball);
        }

        InvokeRepeating("DropBall", startDelay, spawnInterval);
    }

    // Enables the next ball and drops it from a random location
    void DropBall()
    {
        GameObject ball = GetPooledObject();
        if (ball)
        {
            ball.transform.position = new Vector3(Random.Range(-15, 15), SPAWN_POS_Y, 0);
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ball.SetActive(true);
        }
    }

    void DeactivateBall(GameObject ball)
    {
        
        ball.SetActive(false);
    }

    public GameObject GetPooledObject()
    {
        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < balls.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!balls[i].activeInHierarchy)
            {
                return balls[i];
            }
        }
        // otherwise, return null   
        return null;
    }
}
