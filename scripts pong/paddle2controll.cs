using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class paddle2controll : Agent
{
    float move;
    public GameObject ball;
    public GameObject paddle;
    public Ballmove ballmove;
    private void Start()
    {
        ball = GameObject.Find("Ball");
        paddle = GameObject.Find("paddle1");
        ballmove = ball.GetComponent<Ballmove>();

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        move = ((actions.DiscreteActions[0]) - 1) / 10f;
        transform.position = new Vector3(transform.position.x, transform.position.y + move, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(ball.transform.position);
        sensor.AddObservation(ballmove.direction);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        AddReward(1f);
    }


}
