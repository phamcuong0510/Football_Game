﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    //variables for swipe input detection
    private Vector3 fp; //First finger position
    private Vector3 lp; //Last finger position
    private float dragDistance;  //Distance needed for a swipe to register

    //variables for determining the shot power and position
    public float power;  //power at which the ball is shot
    private Vector3 footballPos; //initial football position for replacing the ball at the same posiiton
    private Vector3 footballRot;
   

    public bool canShoot = true;  //flag to check if shot can be taken
    public  int scorePlayer = 0;  //score of player
    public int scoreOpponent = 0; //score of oponent
    public int turn = 0;   //0 for striker, 1 for goalie
    public bool isGameOver = false; //flag for game over detection
    Vector3 oppKickDir;   //direction at which the ball is kicked by opponent
    public int shotsTaken = 0;  //number of rounds of penalties taken
    private bool returned = true;  //flag to check if the ball is returned to its initial position
    public bool isKickedPlayer = false; //flag to check if the player has kicked the ball
    public bool isKickedOpponent = false; //flag to check if the opponent has kicked the ball
    public bool triggered = false;
    public bool click = false; // flag to check if clicked the screen
     //Spaw:
    public GameObject target; 
    public Transform[] spawnPoints;  
   
     public GameObject keeper; 
     public Transform[] keepPoints; 

     public bool directionChosen;
     float xball;
     Vector3 ballPos;
     
     //score + highscore:
      public Text scoreText;
     public Text highscoreText;
     public static float highscore;
      public Button[] buttons;

     
     
    void Start()
    {
        Time.timeScale = 1;    //set it to 1 on start so as to overcome the effects of restarting the game by script
        dragDistance = Screen.height * 10 / 100; //20% of the screen should be swiped to shoot
        Physics.gravity = new Vector3(0, -20, 0); //reset the gravity of the ball to 20
        footballPos = transform.position;  //store the initial position of the football
         
            //High Score
          if (PlayerPrefs.GetFloat("HighScore") != null) {
            highscore = PlayerPrefs.GetFloat("HighScore");
            
            
        }
       
          
        
    }

    
    void Update()
    {

        //High Score:
         if (scorePlayer > highscore) {
            highscore = scorePlayer;
            PlayerPrefs.SetFloat("HighScore", highscore);
        }
         highscoreText.text = "Best: " + Mathf.Round(highscore);//Show hight score
         scoreText.text = "" + scorePlayer;// Show score
         if(isGameOver){ // check if is gameover
          foreach (Button button in buttons)
        {   
             
             Time.timeScale = 0; //Stop all
           
            button.gameObject.SetActive(true);//SetActive Buttons
            highscoreText.gameObject.SetActive(true); // Set Active high score text 
            
        }
         }

        if (returned)
        {     //check if the football is in its initial position
            if (turn == 0 && !isGameOver)
            { //if its users turn to shoot and if the game is not over
                playerLogic();   //call the playerLogic fucntion
            }
            else if (turn == 1 && !isGameOver)
            { //if its opponent's turn to shoot
                //opponentLogic(); //call the respective function
            }
           
        }

        lp = Input.mousePosition;
       // Debug.Log(lp);



        //check if the scores differ by a value of 1
if((scorePlayer==scoreOpponent+2) ||(scoreOpponent==scorePlayer+1)){ 
isGameOver = true;
}

 }




 void OnMouseDrag() {
  
   
  if(!click){
      // if clicked screen
       
   if (returned) 
        {     //check if the football is in its initial position
            if (turn == 0 && !isGameOver)
            { //if its users turn to shoot and if the game is not over
    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
               {   //It's a drag

               if (lp.y > fp.y)  //If the movement was up
                       
                            GetComponent<Rigidbody>().AddForce((new Vector3((lp.x-fp.x)/Screen.height*30,15, 15)) * power);
                     //AudioSource sound = gameObject.GetComponent<AudioSource>();
                                 GetComponent<AudioSource>().Play();
                   
               
                    canShoot = false;
                  returned = false;
                isKickedPlayer = true;
               
               StartCoroutine(ReturnBall());
                click = true;
  }}}}

 }
      void playerLogic()
    {
    


  if (Input.GetButtonDown("Fire1"))
           {
           fp = Input.mousePosition;
           
               click = false;
           
             }

    }

    IEnumerator ReturnBall()
    {   
        yield return new WaitForSeconds(1.0f);  //set a delay of 1 seconds before the ball is returned
        GetComponent<Rigidbody>().velocity = Vector3.zero;   //set the velocity of the ball to zero
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;  //set its angular vel to zero
           //repositon it to initial position:
     if(scorePlayer>5){ // score greater than 5 : random position.x
      xball = Random.Range(-0.5f, 0.5f);
      ballPos = new Vector3(xball, footballPos.y, footballPos.z);
      
       transform.position = ballPos;
     }
     if(scorePlayer <=5){ 
         transform.position = footballPos;
     }
        
        scoreOpponent++; //increment the goals tally of opponent
       if(scorePlayer == 3){
     //Spaw:
                 int spawnPointIndex = Random.Range (0, keepPoints.Length);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate (keeper, keepPoints[spawnPointIndex].position, keepPoints[spawnPointIndex].rotation);
          
}
        if (turn == 1)
            turn = 0;
        canShoot = true;
               returned = true;
               isKickedPlayer = false;
    
    }


    //function to check if its a goal
    void OnTriggerEnter(Collider other)
    {
        //check if the football has triggered an object named GoalLine and triggered is not true
        if (other.gameObject.tag == "target" && !triggered)
        {   
             StartCoroutine(Destroy());
            
         
            
             
             scorePlayer++;
            //if the scorer is the player
            if (turn == 1)
            

            triggered = true;       //check triggered to true

        }
    }
     IEnumerator Destroy()
    {   
        yield return new WaitForSeconds(0.4f);
       Destroy (GameObject.FindWithTag("target"));
        //Spaw:
                 int spawnPointIndex = Random.Range (0, spawnPoints.Length);
                 float xTarget = Random.Range (-1.5f, 1.7f);
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate (target, new Vector3(xTarget,spawnPoints[spawnPointIndex].position.y, spawnPoints[spawnPointIndex].position.z ), spawnPoints[spawnPointIndex].rotation);
    }

//Button functions:
public void OutToMain()
    {
        Application.LoadLevel("MenuScene");//Back Main Sence
    }
public void RePlay()
   
    {
		 Application.LoadLevel(Application.loadedLevel);
    }


}

