using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    //A variable to hold the rigidbody for the player
    private Rigidbody2D rb;
    private Transform playerObject;
    public float defaultSpeed;
    public float speedBoost;
    public float breakForce;
    public float rotationSpeed;

    //Control scheme
    public InputActionAsset playerControls;
    private InputAction forward;
    private InputAction back;
    private InputAction left;
    private InputAction right;
    private InputAction handBreak;
    private InputAction mainMenu;

    //UI Stuff
    public TextMeshProUGUI Timer;
    private int miliSeconds;
    private int seconds;
    private int minutes;
    private bool timerStop = false;
    public GameObject finishScreen;
    public GameObject mainMenuScreen;

    //Finish Line Variable
    public GameObject finishLine;
    public TextMeshProUGUI lapTime;

    //Stop time
    public bool timeTicks = false;
    private bool canCall = true;
    public bool allowMovement = true;

    //Sounds
    public AudioClip hitSound;
    public AudioClip finishAudio;
    public AudioClip engineNoise;

    // Start is called before the first frame update
    void Start()
    {
        //Set's the ridged body for the player
        rb = this.GetComponent<Rigidbody2D>();
        //Get the player object
        playerObject = this.transform;

        ControllEnabled();
    }

    // Update is called once per frame
    void Update()
    {
        MainMenu();
        if(allowMovement)
        {
            MovementMethod();
        }
    }

    private void FixedUpdate()
    {
        TimerMothod();
    }

    public void StopMovement()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void StopandStartTime()
    {
        if(timeTicks)
        {
            UnityEngine.Time.timeScale = 1;
            timerStop = false;
            timeTicks = false;
        }
        else
        {
            UnityEngine.Time.timeScale = 0;
            timerStop = true;
            timeTicks = true;
        }
    }

    //Opens the main menu
    public void MainMenu()
    {
        if (mainMenu.WasPressedThisFrame())
        {
            if(mainMenuScreen.activeSelf == true)
            {
                mainMenuScreen.SetActive(false);
                StopandStartTime();
            }
            else
            {
                mainMenuScreen.SetActive(true);
                StopandStartTime();
            }
        }
    }

    /// <summary>
    /// Keep track of a timer
    /// </summary>
    private void TimerMothod()
    {
        if (timerStop == false)
        {
            //If milisecond is greater then 60 it's a second
            if (miliSeconds > 60)
            {
                miliSeconds = 0;
                seconds += 1;
            }
            //If second is grater then 60 it's a minute
            else if (seconds > 60)
            {
                seconds = 0;
                minutes += 1;
            }
            //Else add to milisecond
            else
            {
                miliSeconds += 1;
            }

            Timer.text = "Timer: ";
            //If minutes is less then 10 add a 0 to the front
            if (minutes < 10)
            {
                Timer.text += "0" + minutes + ":";
                if (seconds < 10)
                {
                    Timer.text += "0" + seconds;
                    //If seconds is less then 10 add a 0 to the front
                    if (miliSeconds < 10)
                    {
                        Timer.text += ":0" + miliSeconds;
                    }
                    //Otherwise don't
                    else
                    {
                        Timer.text += ":" + miliSeconds;
                    }
                }
                //Otherwise don't
                else
                {
                    Timer.text += seconds;
                    //If seconds is less then 10 add a 0 to the front
                    if (miliSeconds < 10)
                    {
                        Timer.text += ":0" + miliSeconds;
                    }
                    //Otherwise don't
                    else
                    {
                        Timer.text += ":" + miliSeconds;
                    }
                }
            }
            //If minute is more then 10 don't add a 0
            else
            {
                Timer.text += minutes + ":";
                if (seconds < 10)
                {
                    Timer.text += "0" + seconds;
                    //If seconds is less then 10 add a 0 to the front
                    if (miliSeconds < 10)
                    {
                        Timer.text += ":0" + miliSeconds;
                    }
                    //Otherwise don't
                    else
                    {
                        Timer.text += ":" + miliSeconds;
                    }
                }
                //Otherwise don't
                else
                {
                    Timer.text += seconds;
                    //If seconds is less then 10 add a 0 to the front
                    if (miliSeconds < 10)
                    {
                        Timer.text += ":0" + miliSeconds;
                    }
                    //Otherwise don't
                    else
                    {
                        Timer.text += ":" + miliSeconds;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method controls player movement
    /// </summary>
    public void MovementMethod()
    {
        if (forward.IsPressed())
        {
            if(engineNoise.loadState == AudioDataLoadState.Unloaded)
            {
                AudioSource.PlayClipAtPoint(engineNoise, playerObject.transform.position);
            }

            //Push Forwards
            rb.AddRelativeForce(new Vector2(0, defaultSpeed + speedBoost));
        }
        if (right.IsPressed())
        {
            if (engineNoise.loadState == AudioDataLoadState.Unloaded)
            {
                AudioSource.PlayClipAtPoint(engineNoise, playerObject.transform.position);
            }

            //Push Right
            rb.SetRotation(rb.rotation - rotationSpeed);
        }
        if (back.IsPressed())
        {
            if (engineNoise.loadState == AudioDataLoadState.Unloaded)
            {
                AudioSource.PlayClipAtPoint(engineNoise, playerObject.transform.position);
            }

            //Push back
            rb.AddRelativeForce(new Vector2(0, -defaultSpeed - speedBoost));
        }
        if (left.IsPressed())
        {
            if (engineNoise.loadState == AudioDataLoadState.Unloaded)
            {
                AudioSource.PlayClipAtPoint(engineNoise, playerObject.transform.position);
            }

            //Push Left
            rb.SetRotation(rb.rotation + rotationSpeed);
        }
        if(handBreak.IsPressed())
        {
            AudioSource.PlayClipAtPoint(hitSound, playerObject.transform.position);
            engineNoise.UnloadAudioData();
            //If stopped make sure we stay stopped no overlapping force
            if (rb.velocity.y == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            if(rb.velocity.x == 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            //If driving left
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(defaultSpeed * breakForce, rb.velocity.y);
            }
            //If driving forwards
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -defaultSpeed * breakForce);
            }
            //If driving backwards
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, defaultSpeed * breakForce);
            }
            //If driving right
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(-defaultSpeed * breakForce,rb.velocity.y);
            }
        }
    }

    /// <summary>
    /// Enables all the controls
    /// </summary>
    public void ControllEnabled()
    {
        //Enable the control scheme
        playerControls.Enable();
        //Enable walk
        forward = playerControls.FindAction("Forward");
        back = playerControls.FindAction("Back");
        left = playerControls.FindAction("Left");
        right = playerControls.FindAction("right");
        handBreak = playerControls.FindAction("Handbreak");
        mainMenu = playerControls.FindAction("MainMenu");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If we collide with a question display it
        if (collision.tag == "Quiz")
        {
            AudioSource.PlayClipAtPoint(hitSound, collision.transform.position);
            playerObject.position = collision.gameObject.transform.position;
            allowMovement = false;
            StopMovement();
            if(canCall)
            {
                GameObject.Find("GameManager").GetComponent<QuizManage>().NextQuestion();
            }
        }
        //If in smoke slow the player
        else if (collision.tag == "Smoke")
        {
            rb.velocity = rb.velocity - new Vector2(rb.velocity.x, rb.velocity.y);
            defaultSpeed = defaultSpeed / 2;
            rotationSpeed = rotationSpeed / 2;
        }

        //When the game is done present the player with a screen to let them know the game is done
        if(collision.tag == "Finish Line")
        {
            AudioSource.PlayClipAtPoint(finishAudio, collision.transform.position);
            finishLine.SetActive(false);
            finishScreen.SetActive(true);
            timerStop = true;
            lapTime.text = Timer.text;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Return player speed when we leave smoke
        if(collision.tag == "Smoke")
        {
            defaultSpeed = defaultSpeed * 2;
            rotationSpeed = rotationSpeed * 2;
        }
        //reset locks put in place during quiz
        if(collision.tag == "Quiz")
        {
            canCall = true;
            Destroy(collision.gameObject);
        }
    }
}
