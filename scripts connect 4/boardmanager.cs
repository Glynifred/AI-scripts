using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardmanager : MonoBehaviour
{
    //initialises variables
    //2d array represents collumns,rows
    public int[,] board = new int[7,6];
    //array of gameobjects pooled to decrease framedrops from intialisation, works better than chaning colour as a low static amount of tokens are needed
    public GameObject[] redtoken;
    public GameObject[] yellowtoken;
    bool p1turn = true;
    bool reset;
    public int ans;
    public int turn = 0;
    public int four = 1;
    int temp = 0;
    int count;
    int round;
    int p1win;
    int p2win;
    public p1control p1control;
    public p2control p2control;
    // Start is called before the first frame update
    void Start()
    {
        //loads all game objects with matching tags into each array
        redtoken = GameObject.FindGameObjectsWithTag("redtoken");
        yellowtoken = GameObject.FindGameObjectsWithTag("yellowtoken");
        p1control = GameObject.Find("p1empty").GetComponent<p1control>();
        p2control= GameObject.Find("p2empty").GetComponent<p2control>();

    }

    // Update is called once per frame
    void Update()
    {
        // gets user input then checks if the top space is free, if not, token cannot be placed in that column for user input
       /*if(Input.GetKeyDown("1"))
       {
        if(board[0,5] == 0)
        {
                //calls calulate function passing in the column
            calcualte(0);
        }
       }
       else if(Input.GetKeyDown("2"))
       {
        if(board[1,5] == 0)
        {
            calcualte(1);
        }
       }
       else if(Input.GetKeyDown("3"))
       {
        if(board[2,5] == 0)
        {
            calcualte(2);
        }
       }
       else if(Input.GetKeyDown("4"))
       {
        if(board[3,5] == 0)
        {
            calcualte(3);
        }
       }
       else if(Input.GetKeyDown("5"))
       {
        if(board[4,5] == 0)
        {
            calcualte(4);
        }
       }
       else if(Input.GetKeyDown("6"))
       {
        if(board[5,5] == 0)
        {
            calcualte(5);
        }
       }
       else if(Input.GetKeyDown("7"))
       {
        if(board[6,5] == 0)
        {
            calcualte(6);
        }
       }*/
     
        if (p1turn == true)
       {
        p1control.RequestDecision();
       }
       else
       {
        p2control.RequestDecision();
       }
    }



    public void calcualte(int column)
    {   //resets who's turn it is at start of the game
        if(reset == true)
        {
            p1turn = true;
            reset = false;
        }
        int y = 0;
        //gets the first empty position in the column
        while(board[column,y] != 0)
        {
            y++;
        }
        //changes which token is moved based on which persons turn
        if (p1turn == true)
        {
            //sets the board array accourding to whos turn it is
        board[column,y] = 1;
            //moves the token to position of the slot
        redtoken[turn].transform.position = new Vector3((column * 2.5f)+2f,(y*1.75f)+1f,-1f);
            //calls scan function passing the column and y
        scan(column,y);
            //changes whos turn it is
        p1turn = false; 
        }
        else
        {     
            //same as above but for values correlated to player 2
        board[column,y] = 2;
        yellowtoken[turn].transform.position = new Vector3((column * 2.5f)+2f,(y*1.75f)+1f,-1f);
        scan(column,y);
        p1turn = true;
        turn++;
            //tests if the game has ended in a draw
        if (turn == 21)
        {
                //calls win that just resets the board
            win();
            p1control.AddReward(.1f);
            p2control.AddReward(.1f);
            p1control.EndEpisode();
            p2control.EndEpisode();
        }
        }
    }
    //scan used to check if four in a row has been made
    void scan(int x,int y)
    {
        //sets which token to look for based on player turn, 1 being red,2 being yellow.
        if (p1turn == true)
        {
           ans = 1;
        }
        else
        {
            ans = 2;
        }
        //differnt directions of scannning split for easier readability and debugging
        scanvert(x,y);
        scanhor(x,y);
        scandia1(x,y);
        scandia2(x,y);

    }

    //scans vertialy down as a token cannot be above the last placed token 
    void scanvert(int x, int y)
    {
        //creates a tempory y to track where the alorithm last looked
        int tempy = y;
        bool check = false;
        //checks unitl a different token is found or no token
         while(check == false)
        {
            //decreaes tempy to check the poition below 
            tempy--;
            //tests that tempy hasnt gone below the board
            if( tempy >= 0)
            {
                //looks if the token at the poistion matches the players colour
                if (board[x,tempy] == ans)
                {
                    //if so adds to the four count
                    four++;
                }
                else
                {
                    //if something else either blank or a different colour make check = true as no more tokens can add to the four count
                    check = true;
                }
            }
            else
            {   //sets check = true if the position is below the board
                check = true;
            }  
        }
         //tests if a four has been made and if so calls win
        if(four >= 4)
        {
            if(p1turn == true)
            {
                p1control.AddReward(1f);
                p1win++;
            }
            else
            {
                p2control.AddReward(1f);
                p2win++;
            }
            p1control.EndEpisode();
            p2control.EndEpisode();
            //win resets the board
            win();
            
        }
        four = 1;
    }

    //scans horizontally left and right as now the placed token can be found in the middle of a four and not at the end.
    void scanhor(int x, int y)
    {//creates a temp x to track the last positon checked
        int tempx = x;
        bool check = false;
        //works same as vertical but changes the x instead
         while(check == false)
            {
                //checks poistions to the left of the placed token
                tempx--;
                if( tempx >= 0)
                {
                   if (board[tempx,y] == ans)
                    {
                        four++;
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }  
            }
         //resets check and temp x as now we have to check to the right
            check = false;
            tempx = x;
            while(check == false)
            {
                //checks positions to the right of the placed token
                tempx++;
                if(tempx <= 6)
                {
                    if (board[tempx,y] == ans)
                    {
                        four++;
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }
                
            }
            //checks if a four in a row has been created
            if(four >= 4)
            {
                if(p1turn == true)
            {
                p1control.AddReward(1f);
                p1win++;
            }
            else
            {
                p2control.AddReward(1f);
                p2win++;
            }
            p1control.EndEpisode();
            p2control.EndEpisode();
            //if so calls win to reset the board
                win();
            }
            four = 1;
    }

    //scans upper left and bottom right
     void scandia1(int x, int y)
    {
        //creates a temp x and y to track the last positon checked
        int tempx = x;
        int tempy = y;
        bool check = false;
            while(check == false)
            {
                //moves the checked position left and up
                tempx--;
                tempy++;
                //makes sure the checked position is below the top of the board and within the left boarder
                if( tempx >= 0 && tempy <= 5)
                {
                   if (board[tempx,tempy] == ans)
                    {
                        four++;
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }  
            }
            //resets temp x and y and check
            check = false;
            tempx = x;
            tempy = y;
            while(check == false)
            {
            //moves the checked position down and right
                tempx++;
                tempy--;
            //makes sure the checked position is above the bottom of the board and to the left of right border
                if(tempx <= 6 && tempy >=0)
                {
                    if (board[tempx,tempy] == ans)
                    {
                        four++;
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }
                
            }
        //checks if a four in a row has been created
        if (four >= 4)
        {
            if(p1turn == true)
            {
                p1control.AddReward(1f);
                p1win++;
            }
            else
            {
                p2control.AddReward(1f);
                p2win++;
            }
            p1control.EndEpisode();
            p2control.EndEpisode();
            //if so calls win to reset the board
            win();
        }
        four = 1;
    }
    
    //scans bottom left and top right working exatcly the same as scandia1 with different direction
    void scandia2(int x, int y)
    {
        int tempx = x;
        int tempy = y;
        bool check = false;
            while(check == false)
            {
                //moves checked position down and left
                tempx--;
                tempy--;
            //makes sure position is above the bottom and greater than the left border
                if( tempx >= 0 && tempy >= 0)
                {
                   if (board[tempx,tempy] == ans)
                    {
                        four++;
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }  
            }
            check = false;
            tempx = x;
            tempy = y;
            while(check == false)
            {
            //moves checked position up and right
                tempx++;
                tempy++;
            //makes sure checked position is lower than the top and smaller than the right border
                if(tempx <= 6 && tempy <=5)
                {
                    if (board[tempx,tempy] == ans)
                    {
                        four++;
                    }
                    else
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }
                
            }
            if(four >= 4)
            {
                if(p1turn == true)
            {
                p1control.AddReward(1f);
                p1win++;
            }
            else
            {
                p2control.AddReward(1f);
                p2win++;
            }
            p1control.EndEpisode();
            p2control.EndEpisode();
                win();

            }
            four = 1;
    }

    void win()
    {
        //resets board state
        board = new int[7,6];
        //reset values
        reset = true;
        turn = 0;
        four = 1;
        //moves all tokens off the board
        foreach(GameObject token in redtoken)
       {
        token.transform.position = new Vector3(-3f,-3f,-1f);
       } 
        foreach(GameObject token in yellowtoken)
       {
        token.transform.position = new Vector3(-3f,-3f,-1f);
       } 
       count++;
       if (count == 5)
                {
                    round = round + 5;
                    count = 0;
                    Debug.Log("round " + round + " wins " + p1win + " to " + p2win);
                }
    }
}
