using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum winLose { won,playing,lost};

public class GUItext : MonoBehaviour
{
    public Text scoreText;
    public GameObject UIpanel;

    private winLose gameState;

    void CheckWin() 
    {

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("perish");

        if(allObjects.Length < 3)
        {// all bricks broken

            UIpanel.transform.position = new Vector3(0, 0, 0);
            UIpanel.GetComponent<VerticalLayoutGroup>().padding.bottom = 3;
            scoreText.color = new Color(0.25f, 0.90f, 0.1f);
            scoreText.text = "Well Done! level complete" +
                "SCORE  " + ballScript.score +
                "\npress enter to continue";

            gameState = winLose.won;

        }

    }
    void CheckLose()
    {
        if (ballScript.lives < 0 && gameState == winLose.playing)// gaem over
        {
            Debug.Log("game over");
            scoreText.color = new Color (0.5f,0.0f,0.0f);
            // kill all bricks & paddles
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("perish");
            foreach (GameObject obj in allObjects)
            {
                Destroy(obj);
            }
            //  show end screen

            UIpanel.transform.position = new Vector3(0, 0, 0);
            UIpanel.GetComponent<VerticalLayoutGroup>().padding.bottom = 3;

            scoreText.text = "Game Over " +
                "SCORE  " + ballScript.score +
                "\npress enter to continue";
            ballScript.score = 0;
            gameState = winLose.lost;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        gameState = winLose.playing;
        scoreText.text = "SCORE\t" + ballScript.score + " LIVES  " + ballScript.lives;
        scoreText.color = new Color(0.05f, 0.31f, 0.91f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == winLose.playing)
        {
            scoreText.text = "SCORE  " + ballScript.score + " LIVES  " + ballScript.lives;
            CheckWin();
            CheckLose();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.LoadLevel(0);
            }

        }
        else if(Input.GetKeyDown(KeyCode.Return))// game is over, enter to return to menu
        {
            Application.LoadLevel(0);
        }
    }
}
