using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballmove : MonoBehaviour
{
   //declaring varaibles
    public Vector3 direction;
    public float speed = 1f;
    public GameObject paddle1;
    public GameObject paddle2;
    public GameObject leftwall;
    public GameObject rightwall;
    public GameObject topwall;
    public GameObject bottomwall;
    //variables to hold AI script controllers
    public paddle1control p1con;
    public paddle2controll p2con;
    public int scorep1;
    public int scorep2;
    public float RNG1;
    public float temp;
    public float answer;
    bool ready = false;
    private float timer = 1;
    int count;
    int round;
    int p1win;
    int p2win;
    // Start is called before the first frame update
    void Start()
    {
        //gets objects refrences for later use in script, ive done this so I only need to find each object once reducing processing time when running
        paddle1 = GameObject.Find("paddle1");
        paddle2 = GameObject.Find("paddle2");
        leftwall = GameObject.Find("leftwall");
        rightwall = GameObject.Find("rightwall");
        topwall = GameObject.Find("topwall");
        bottomwall = GameObject.Find("bottomwall");
        //gets the scripts attached to paddle1 and psddle2 which hold the AI agents classes used to increment each AI reward function
        p1con = paddle1.GetComponent<paddle1control>();
        p2con = paddle2.GetComponent<paddle2controll>();
        //calls spawan to start the game
        Spawn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //makes it so the ball waits a second before moving
            if (timer <= 0)
            {
                //moves the ball based on the given direction(vector3 e.g. x,y,z) and speed(int) that gets incremented each bounce
                this.transform.position = this.transform.position + (direction * speed);
            }
            else 
            {
                //takes timer down based on time between each frame
                timer = timer - Time.deltaTime;
            }
            
        if(transform.position.y <= 0 || transform.position.y >= 10.8f)
        {
            //ball bounces on top and bottom screen as colliders can break when speed or two colliders happen at once, works by just changing the sign of the directions y value
            direction = new Vector3(direction.x,direction.y * -1f,direction.z);
        }
    }

    //called on every point and start of the game
    void Spawn()
    {
        //resets position of ball
        transform.position = new Vector3(9.6f,5.4f,0f);
        //resets speed of ball
        speed = 1f;
        //generates a random number between 0 and 10000000 and then devides it by 10000000 to get a value of 0-1, used to be 0-1f but this could never give 1, as such using 10000001 and deviding gives the full float array and the number 1
        RNG1 = (Random.Range(0,10000001)/10000000f); 
        //checks if the number is less than .5 and if so adds it to -1,the values created are -1 to -.5 and .5 to 1, meaning the ball will always go towards a paddle at a reasonable angle
        if (RNG1 < 0.5f)
        {
            RNG1 = -1f + RNG1;
        }
        //uses pythag ( A^2 + B^2 = c^2 to work out the componant forces of x and y that result in a overall force of 1 
        //logic behind maths b^2 = c^2 - A^2
        temp = 1f - (RNG1 * RNG1);
        //B = root B^2
        answer = Mathf.Sqrt(temp);
        //since the answer of root b^2 can be both negative and posative uses a random number of 0 or 1 and if 0 makes the B negative 
        temp = (Random.Range(0,2));
        if(temp == 0)
        {
            answer = answer * -1;
        }
        //uses the random number for the x and the calculated number for y which will always result in a force of 1 with a random angle of 45-135 in either direction
        direction = new Vector3(+ (RNG1 /10), + (answer/10),0f);
        //resets position of paddles keeping thier x value
        paddle2.transform.position = new Vector3(paddle2.transform.position.x,5.4f,0);
        paddle1.transform.position = new Vector3(paddle1.transform.position.x,5.4f,0);
        timer = 1;
    }

    //detects when the ball enters another collider and saves the other collider for use
    void OnTriggerEnter2D(Collider2D collider)
    {
        //tests if the collider was attached to paddle 1
        if (collider.gameObject == paddle1)
            {
                //gets the distance between the center of the paddle and ball
                temp = transform.position.y - paddle1.transform.position.y;
                //converts the distance to a ratio of 0 being dead center and 1 being on the edge of the paddle
                temp = (temp/1.5f)/10f;
                //locks max value incase the ball hits the side of paddle instead of face
                if(temp > 0.1f)
                {
                    temp = .1f;
                }
                else if(temp < -0.1f)
                {
                    temp = -.1f;
                }
                //makes it so the angle cannot be too large by testing if the current angle plus the additional angle is too great as this would increase the speed of the ball past the speed limit due to increaing the force behind the ball
                if (direction.y + temp > .1f)
                {
                    //if too possative takes the possative off direction so it cancels out when calculated
                    direction.y = .1f-temp;
                }
                else if (direction.y + temp < -.1f)
                {
                    //if too negative takes the negative off increaing the angle once again cancelling out
                    direction.y = -.1f - temp;
                }
                //adds the new angle to the direction
                direction = new Vector3(direction.x * -1f,direction.y + temp,0f);
                //increase the speed of the ball
                speed = speed + 0.1f;
                //locks the speed of the ball to 10 as over 10, the ball can start passing colliders without colliding
                if(speed > 10)
                {
                    speed = 10;
                }
            }
            //tests if collider was attached to paddle 2 and else if as cannot collide with both
            else if (collider.gameObject == paddle2)
            {
            //gets the distance between the center of the paddle and ball
            temp = transform.position.y - paddle2.transform.position.y;
            //converts the distance to a ratio of 0 being dead center and 1 being on the edge of the paddle
            temp = (temp / 1.5f) / 10f;
            //locks max value incase the ball hits the side of paddle instead of face
            if (temp > 0.1f)
            {
                temp = .1f;
            }
            else if (temp < -0.1f)
            {
                temp = -.1f;
            }
            //makes it so the angle cannot be too large by testing if the angle would be too great
            if (direction.y + temp > .1f)
            {
                //if too possative takes the possative off direction so it cancels out when calculated
                direction.y = .1f - temp;
            }
            else if (direction.y + temp < -.1f)
            {
                //if too negative takes the negative off increaing the angle once again cancelling out
                direction.y = -.1f - temp;
            }
            //adds the new angle to the direction
            direction = new Vector3(direction.x * -1f, direction.y + temp, 0f);
            //increase the speed of the ball
            speed = speed + 0.1f;
            //locks the speed of the ball to 10 as over ball can start passing colliders in frame drops
            if (speed > 10)
            {
                speed = 10;
            }
        }
            //tests if left wall was collided
            if (collider.gameObject == leftwall)
            { 
                //increases player 2 score
                scorep2 ++;
            //tests if player two has reached the score
            if (scorep2 == 1)
            {
                p1con.EndEpisode();
                p2con.EndEpisode();
                scorep1 = 0;
                scorep2 = 0;
                count++;
                p2win++;
                if (count == 5)
                {
                    round = round + 5;
                    count = 0;
                    Debug.Log("round " + round + " wins " + p1win + " to " + p2win);
                }
            }
            //resets round
            Spawn();
            }
            //tests if right wall was collided
            else if (collider.gameObject == rightwall)
            {
            //give a point to player 1
                scorep1 ++;
            //tests if player one has reached the score
            if (scorep1 == 1)
            {
                p1con.EndEpisode();
                p2con.EndEpisode();
                scorep1 = 0;
                scorep2 = 0;
                count++;
                p1win++;
                if (count == 5)
                {
                    round = round + 5;
                    count = 0;
                    Debug.Log("round " + round + " wins " + p1win + " to " + p2win);
                }
            }
            //reset the round
            Spawn();
            }
    }
}
    


