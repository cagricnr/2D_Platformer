using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    Rigidbody2D rigidBody;

    [SerializeField]
    Animator anim;

    [SerializeField]
    GameObject health1, health2, health3, startingPos;

    [SerializeField]
    float speed = 5f;
    bool doesRun = false;
    int score = 0;
    int bestscore = 0;
    int health = 3;


    [SerializeField]
    Text scoreText, scorePanelText, bestscoreText, bestscorePanelText;

    [SerializeField]
    GameObject RestartPanel, mainMenuPanel;
    public static bool gameStarded = false;
    [SerializeField]
    AudioSource coinAudio;




    // Start is called before the first frame update
    void Start()
    {

        if (PlayAgain.isRestart)
        {
            mainMenuPanel.SetActive(false);
            gameStarded = true;
        }
        checkScore();
        checkBestScore();

    }

    private void FixedUpdate()
    {

        if (!gameStarded)
        {
            return;
        }
        float mySpeedX = Input.GetAxis("Horizontal");
        move(mySpeedX);
        myAnimation(mySpeedX);
        charDirection(mySpeedX);
    }





    void checkBestScore()
    {
        bestscore = PlayerPrefs.GetInt("Highscore");
        bestscoreText.text = bestscore.ToString();
    }

    void checkScore()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                PlayerPrefs.DeleteKey("Score");
            }
            else
            {
                score = PlayerPrefs.GetInt("Score");


            }
        }
        scoreText.text = score.ToString();
    }

    // Update is called once per frame


    public void playGame()
    {
        gameStarded = true;
        mainMenuPanel.SetActive(false);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        implementTag(collision);

    }


    void implementTag(Collider2D collision)
    {
        if (collision.CompareTag("Pineapple"))
        {
            scoreCalc(collision, 1);

        }


        else if (collision.CompareTag("Enemy"))
        {
            fail();
        }
        else if (collision.CompareTag("Death"))
        {
            fail();
        }
        else if (collision.CompareTag("Hitbox"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, speed);
            StartCoroutine(scaleDown(collision));



        }
        else if (collision.CompareTag("End"))
        {
            if (SceneManager.GetActiveScene().buildIndex < 4)
            {
                saveScore();
                bestscoreCalc();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene("Level1");
            }

        }
    }
    IEnumerator scaleDown(Collider2D collision)
    {

        collision.transform.parent.localScale = new Vector3(1, 0.8f, 1);
        yield return new WaitForSeconds(0.2f);
        collision.transform.parent.localScale = new Vector3(1, 0.6f, 1);
        yield return new WaitForSeconds(0.2f);
        collision.transform.parent.localScale = new Vector3(1, 0.4f, 1);
        yield return new WaitForSeconds(0.2f);
        collision.transform.parent.localScale = new Vector3(1, 0.2f, 1);
        yield return new WaitForSeconds(0.2f);
        Destroy(collision.transform.parent.gameObject);
        scoreCalc(collision, 5);
    }

    void scoreCalc(Collider2D collision, int kazanc)
    {
        score += kazanc;
        scoreText.text = score.ToString();
        Destroy(collision.gameObject);
        coinAudio.Play();
    }
    void saveScore()
    {
        PlayerPrefs.SetInt("Score", score);
    }

    void bestscoreCalc()
    {
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
           PlayerPrefs.SetInt("Highscore", score);
        }
    }

    void death()
    {
        Destroy(this.gameObject);
        RestartPanel.SetActive(true);
        saveScore();
        bestscoreCalc();
        checkBestScore();
        scorePanelText.text = "Skor: " + score.ToString();
        bestscorePanelText.text = "Best Skor: " + bestscore.ToString();
    }
    void fail()
    {

        health--;
        if (health ==2)
        {
            health1.SetActive(false);
            transform.position = startingPos.transform.position;
        }
        else if (health ==1)
        {
            health2.SetActive(false);
            transform.position = startingPos.transform.position;
        }
        else if (health == 0)
        {
            health3.SetActive(false);
            death();
        }
    }

    void move(float h)
    {

        rigidBody.velocity = new Vector2(h * speed, rigidBody.velocity.y);
    }
    void myAnimation(float h)
    {
        if (h != 0)//Animasyon ??lemleri: E?er mySpeedX = 0 ise idle, de?ilse run
        {
            doesRun = true;

        }
        else
        {
            doesRun = false;
        }
        anim.SetBool("isRunning", doesRun);
    }

    void charDirection(float h) //dönme i?lemleri
    {
        if (h > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }






}
