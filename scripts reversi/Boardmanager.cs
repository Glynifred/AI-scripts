using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boardmanager : MonoBehaviour
{
    //declaring variables
    //2d array for board corisponds to column row
    public int[,] board = new int[8,8];
    //2d array that mirrors the board usd to save token locations in a 2d array
    public GameObject[,] tokens = new GameObject[8,8];
    //array of gameobjects used to save objects with circle tag
    public GameObject[] circle;
    bool go = true;
    public int partone = -1;
    public int parttwo = -1;
    bool p1turn = true;
    int ans;
    int turn = 1;
    int p1score;
    int p2score;
    public p1control p1control;
    public p2control p2control;
    int count;
    int round;
    int p1win;
    int p2win;
    // Start is called before the first frame update
    void Start()
    {
        float x;
        float y;
        //gets all objects with circle tag and saves into array
    circle = GameObject.FindGameObjectsWithTag("circle");
        //converts one dimensional array into 2d array that matches the positioning of the board so 1,1 is bottom left and 8,8 is top right
        //circle used to be loaded in oder but updating to 2023 messed it up and so new maths works out where each randomised circle should go in the array based on its position
        for(int i = 0;i < 64;i++)
        {
            x = ((circle[i].transform.localPosition.x - 0.0125f)/1.175f)-1f;
            y = ((circle[i].transform.localPosition.y - 0.0125f)/1.175f)-1f;
            tokens[(int)x,(int)y] = circle[i];
        }
        p1control = GameObject.Find("p1empty").GetComponent<p1control>();
        p2control = GameObject.Find("p2empty").GetComponent<p2control>();    
    }

    // Update is called once per frame
    //update used to get better inputs since game can be run using frames instead of set time to control game speed
    void Update()
    {       //gets input for partone that corrispondes to the column
           /* if(Input.GetKeyDown("a"))
            {
                partone = 0;
            }
             if(Input.GetKeyDown("b"))
            {
                partone = 1;
            }
             if(Input.GetKeyDown("c"))
            {
                partone = 2;
            }
             if(Input.GetKeyDown("d"))
            {
                partone = 3;
            }
             if(Input.GetKeyDown("e"))
            {
                partone = 4;
            }
             if(Input.GetKeyDown("f"))
            {
                partone = 5;
            }
             if(Input.GetKeyDown("g"))
            {
                partone = 6;
            }
             if(Input.GetKeyDown("h"))
            {
                partone = 7;
            }
        //gets inpit for parttwo that corrispondes to the row
        if (Input.GetKeyDown("1"))
            {
                parttwo = 0;
            }
             if(Input.GetKeyDown("2"))
            {
                parttwo = 1;
            }
             if(Input.GetKeyDown("3"))
            {
                parttwo = 2;
            }
             if(Input.GetKeyDown("4"))
            {
                parttwo = 3;
            }
             if(Input.GetKeyDown("5"))
            {
                parttwo = 4;
            }
             if(Input.GetKeyDown("6"))
            {
                parttwo = 5;
            }
             if(Input.GetKeyDown("7"))
            {
                parttwo = 6;
            }
            if(Input.GetKeyDown("8"))
            {
                parttwo = 7;
            }
            //checks that both partone and parttwo have had an input
            if (partone != -1 && parttwo != -1)
            {
                //makes sure the desired move isnt already taken
                if (board[partone,parttwo] == 0)
                {
                    go = false;
                }
            }
            //once a valid command is made takes the players turn
        if (go == false)
        {
            //calls turn that makes the players turn
            taketurn(partone,parttwo);
            partone = -1;
            parttwo = -1;
            go = true;
        }
        
        */
        if (p1turn == true)
        {
           p1control.RequestDecision(); 
        }
        else
        {
            p2control.RequestDecision();
        }
        
    }

    public void taketurn(int tempone,int temptwo)
    {
        partone = tempone;
        parttwo = temptwo;
        //checks which players turn it is 
        if(p1turn == true)
            {
                
                //sets the token at the given position to players colour
                tokens[partone,parttwo].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                //sets the number of the board at the given position to the number that corrispondes to the player
                board[partone,parttwo] = 1;
                //sets the value the scan algorithm looks for to the value of the players token
                ans = 1;
                //calls scan which checks if pieces need to be flipped
                scan();
                //changes the players turn
                p1turn = false;
                partone = -1;
                parttwo = -1;
            }
            else
            {
                //same as above but values corrisponde to player two
                tokens[partone,parttwo].GetComponent<SpriteRenderer>().material.color = Color.red;
                board[partone,parttwo] = 2;
                ans = 2;
                turn++;
                scan();
                p1turn = true;
                partone = -1;
                parttwo = -1;
            }
    }


    void scan()
    {
        //calls each scan diretion split up for easability to read and debbug
        scanup();
        scandown();
        scanleft();
        scanright();
        scanbottomleft();
        scanbottomright();
        scantopleft();
        scantopright();
        if(turn == 33)
        {
            end();
        }
    }

    void scanup()
    {
        int tempy = parttwo;
        int lasty = -1;
        bool check = false;
         while(check == false)
        {
            tempy++;
            if( tempy <= 7)
            {
                if (board[partone,tempy] == 0)
                {
                    check = true;
                }
                else if(board[partone,tempy] == ans)
                {
                    lasty = tempy;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lasty != -1)
            {
                tempy = parttwo;
                if(p1turn == true)
                {
                    while(tempy < lasty)
                    {
                        tempy++;
                        tokens[partone,tempy].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[partone,tempy] = 1;
                    }                 
                }
                else
                {
                    while(tempy < lasty)
                    {
                        tempy++;
                        tokens[partone,tempy].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[partone,tempy] = 2;
                    }  
                }
        }
    }

    void scandown()
    {
        int tempy = parttwo;
        int lasty = -1;
        bool check = false;
         while(check == false)
        {
            tempy--;
            if( tempy >= 0)
            {
                if (board[partone,tempy] == 0)
                {
                    check = true;
                }
                else if(board[partone,tempy] == ans)
                {
                    lasty = tempy;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lasty != -1)
            {
                tempy = parttwo;
                if(p1turn == true)
                {
                    while(tempy > lasty)
                    {
                        tempy--;
                        tokens[partone,tempy].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[partone,tempy] = 1;
                    }                 
                }
                else
                {
                    while(tempy > lasty)
                    {
                        tempy--;
                        tokens[partone,tempy].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[partone,tempy] = 2;
                    }  
                }
        }
    }

    void scanleft()
    {
        int tempx= partone;
        int lastx = -1;
        bool check = false;
         while(check == false)
        {
            tempx--;
            if( tempx >= 0)
            {
                if (board[tempx,parttwo] == 0)
                {
                    check = true;
                }
                else if(board[tempx,parttwo] == ans)
                {
                    lastx = tempx;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lastx != -1)
            {
                tempx = partone;
                if(p1turn == true)
                {
                    while(tempx > lastx)
                    {
                        tempx--;
                        tokens[tempx,parttwo].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[tempx,parttwo] = 1;
                    }                 
                }
                else
                {
                    while(tempx > lastx)
                    {
                        tempx--;
                        tokens[tempx,parttwo].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[tempx,parttwo] = 2;
                    }  
                }
        }
    }

    void scanright()
    {
        int tempx = partone;
        int lastx = -1;
        bool check = false;
         while(check == false)
        {
            tempx++;
            if(tempx <= 7)
            {
                if (board[tempx,parttwo] == 0)
                {
                    check = true;
                }
                else if(board[tempx,parttwo] == ans)
                {
                    lastx = tempx;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lastx != -1)
            {
                tempx = partone;
                if(p1turn == true)
                {
                    while(tempx < lastx)
                    {
                        tempx++;
                        tokens[tempx,parttwo].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[tempx,parttwo] = 1;
                    }                 
                }
                else
                {
                    while(tempx < lastx)
                    {
                        tempx++;
                        tokens[tempx,parttwo].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[tempx,parttwo] = 2;
                    }  
                }
            } 
    }

     void scantopright()
    {
        int tempy = parttwo;
        int lasty = -1;
        int tempx = partone;
        int lastx = -1;
        bool check = false;
         while(check == false)
        {
            tempy++;
            tempx++;
            if( tempy <= 7 && tempx <=7)
            {
                if (board[tempx,tempy] == 0)
                {
                    check = true;
                }
                else if(board[tempx,tempy] == ans)
                {
                    lasty = tempy;
                    lastx = tempx;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lasty != -1 && lastx !=-1)
            {
                tempy = parttwo;
                tempx = partone;
                if(p1turn == true)
                {
                    while(tempy < lasty)
                    {
                        tempy++;
                        tempx++;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[tempx,tempy] = 1;
                    }                 
                }
                else
                {
                    while(tempy < lasty)
                    {
                        tempy++;
                        tempx++;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[tempx,tempy] = 2;
                    }  
                }
        }
    }
    
    void scanbottomleft()
    {
        int tempy = parttwo;
        int lasty = -1;
        int tempx = partone;
        int lastx = -1;
        bool check = false;
        while(check == false)
        {
            tempy--;
            tempx--;
            if( tempy >= 0 && tempx >=0)
            {
                if (board[tempx,tempy] == 0)
                {
                    check = true;
                }
                else if(board[tempx,tempy] == ans)
                {
                    lasty = tempy;
                    lastx = tempx;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lasty != -1 && lastx !=-1)
            {
                tempy = parttwo;
                tempx = partone;
                if(p1turn == true)
                {
                    while(tempy > lasty)
                    {
                        tempy--;
                        tempx--;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[tempx,tempy] = 1;
                    }                 
                }
                else
                {
                    while(tempy > lasty)
                    {
                        tempy--;
                        tempx--;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[tempx,tempy] = 2;
                    }  
                }
        }
    }
    void scanbottomright()
    {
        int tempy = parttwo;
        int lasty = -1;
        int tempx = partone;
        int lastx = -1;
        bool check = false;
        while(check == false)
        {
            tempy--;
            tempx++;
            if( tempy >= 0 && tempx <=7)
            {
                if (board[tempx,tempy] == 0)
                {
                    check = true;
                }
                else if(board[tempx,tempy] == ans)
                {
                    lasty = tempy;
                    lastx = tempx;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lasty != -1 && lastx !=-1)
            {
                tempy = parttwo;
                tempx = partone;
                if(p1turn == true)
                {
                    while(tempy > lasty)
                    {
                        tempy--;
                        tempx++;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[tempx,tempy] = 1;
                    }                 
                }
                else
                {
                    while(tempy > lasty)
                    {
                        tempy--;
                        tempx++;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[tempx,tempy] = 2;
                    }  
                }
        }
    }

     void scantopleft()
    {
        int tempy = parttwo;
        int lasty = -1;
        int tempx = partone;
        int lastx = -1;
        bool check = false;
         while(check == false)
        {
            tempy++;
            tempx--;
            if( tempy <= 7 && tempx >=0)
            {
                if (board[tempx,tempy] == 0)
                {
                    check = true;
                }
                else if(board[tempx,tempy] == ans)
                {
                    lasty = tempy;
                    lastx = tempx;
                }
            }
            else
            {
                check = true;
            } 
        }
            if(lasty != -1 && lastx !=-1)
            {
                tempy = parttwo;
                tempx = partone;
                if(p1turn == true)
                {
                    while(tempy < lasty)
                    {
                        tempy++;
                        tempx--;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.yellow;
                        board[tempx,tempy] = 1;
                    }                 
                }
                else
                {
                    while(tempy < lasty)
                    {
                        tempy++;
                        tempx--;
                        tokens[tempx,tempy].GetComponent<SpriteRenderer>().material.color = Color.red;
                        board[tempx,tempy] = 2;
                    }  
                }
        }
    }

    void end()
    {
        for(int i = 0;i < 8;i++)
        {
            for(int y = 0; y < 8;y++)
            {
                if(board[i,y]== 1)
                {
                    p1score++;
                    board[i,y]= 0;
                    tokens[i,y].GetComponent<SpriteRenderer>().material.color = Color.white;
                }
                else if(board[i,y]== 2)
                {
                    p2score++;
                    board[i,y]= 0;
                    tokens[i,y].GetComponent<SpriteRenderer>().material.color = Color.white;
                }
                else
                {
                  
                }
            }
        }
        if(p1score > p2score)
        {
            p1win++;
        }
        else if (p1score < p2score)
        {
          p2win++;  
        }
        else
        {
        
            
        
        }
        count++;
        if (count == 5)
                {
                    round = round + 5;
                    count = 0;
                    Debug.Log("round " + round + " wins " + p1win + " to " + p2win);
                }
        p1control.AddReward(p1score);
        p2control.AddReward(p2score);
        p1control.EndEpisode();
        p2control.EndEpisode();
        p1score = 0;
        p2score= 0;
        p1turn = true;
        turn = 1;
    }
}
