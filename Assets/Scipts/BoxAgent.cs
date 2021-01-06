using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;

public class BoxAgent : Agent
{
    [System.NonSerialized] public int Count = 0;

    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Text CounterText;

    private Rigidbody agentRB;
    private Vector3 startingPos = Vector3.zero;    

    // Start is called before the first frame update
    void Start()
    {
        agentRB = GetComponent<Rigidbody>(); 
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent position and clear all balls
        transform.localPosition = startingPos;
        Count = 0;

        ballSpawner.BeginEpisode();
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observations x,y,x
        sensor.AddObservation(transform.localPosition);

        // 1 observation
        sensor.AddObservation(agentRB.velocity.x);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveAmount = 0;

        if (actions.DiscreteActions[0] == 1)
        {
            moveAmount = -moveSpeed;
        }

        else if (actions.DiscreteActions[0] == 2)
        {
            moveAmount = moveSpeed;
        }

        agentRB.AddForce(Vector3.right * moveAmount, ForceMode.VelocityChange);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();

        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            UpdateReward(1);
            other.gameObject.SetActive(false);
        }        
    }

    public void UpdateReward(int rewardAmount)
    {
        AddReward(rewardAmount);
        Count += rewardAmount;        

        // We'll only be setting text on the first instance of the environment
        if (CounterText)
        {
            CounterText.text = "Count : " + Count;
        }
    }

    public void MissedBall()
    {
        AddReward(-1);
    }
}
