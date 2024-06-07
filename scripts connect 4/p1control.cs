using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class p1control : Agent
{
   public boardmanager boardmanager;
   public GameObject Gamemanger;
   float temp;
    private void Start()
    {
        Gamemanger = GameObject.Find("Gamemanger");
        boardmanager = Gamemanger.GetComponent<boardmanager>();
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        boardmanager.calcualte(actions.DiscreteActions[0]);
    }

    public override void CollectObservations(VectorSensor sensor)
    {   
        for(int y = 0; y < 6; y++)
        {
            for(int x = 0; x < 7; x++)
            {
        temp = boardmanager.board[x,y];
            sensor.AddObservation(temp);
            }
        }
    }

public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
{   

for(int x = 0; x < 7; x++)
{
    if(boardmanager.board[x,5] != 0)
    {
        actionMask.SetActionEnabled(0, x, false);
    }
    else
    {
        actionMask.SetActionEnabled(0, x, true);
    }
}
    
}

}