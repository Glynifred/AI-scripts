using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class p1control : Agent
{
   public Boardmanager boardmanager;
   public GameObject Gamemanger;
   float temp;
   int tempone;
   int temptwo;
    private void Start()
    {
        Gamemanger = GameObject.Find("Gamemanger");
        boardmanager = Gamemanger.GetComponent<Boardmanager>();
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //casting to int always truncates for you
        temp = (float)actions.DiscreteActions[0];
        tempone = (int)temp/8;
        temptwo = (int)temp - (tempone*8);
        boardmanager.taketurn(tempone,temptwo);
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {   
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
            temp = boardmanager.board[x,y];
            sensor.AddObservation(temp);
            }
        }
    }

public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
{   
 
    for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
            if(boardmanager.board[x,y] != 0)
            {
            actionMask.SetActionEnabled(0, (x*8)+y, false);
            }
            else
            {
            actionMask.SetActionEnabled(0, (x*8)+y, true);
            }
        }
}
    
}

}