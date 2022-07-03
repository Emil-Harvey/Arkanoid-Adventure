using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum winLose { won,playing,lost};

public class GUItext : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject UIpanel;

    private winLose gameState;

    AudioSource audio;
    public AudioClip winSfx;
    public AudioClip failSfx;
    
    void CheckWin() 
    {

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("perish");

        if(allObjects.Length < 3 || Input.GetKeyDown(KeyCode.BackQuote))
        {// all bricks broken

            UIpanel.transform.position = new Vector3(0, 0, 0);
            UIpanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 4.8f);//UIpanel.GetComponent<VerticalLayoutGroup>().padding.bottom = 3;
            scoreText.color = new Color(0.25f, 0.90f, 0.1f);
            scoreText.text = " Well Done! level complete \n" +
                " SCORE  " + paddleScript.score +
                "\n press enter to continue ";

            gameState = winLose.won;

            var bgMusic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
            bgMusic.mute = true;//.Stop(); // stop bg music
            bgMusic.Pause(); // stop bg music
            bgMusic.Stop(); // stop bg music
            audio.PlayOneShot(winSfx);
        }

    }
    void CheckLose()
    {
        if (paddleScript.lives < 0 && gameState == winLose.playing)// gaem over
        {
            Debug.Log("game over");
            
            // kill all bricks & paddles
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("perish");
            foreach (GameObject obj in allObjects)
            {
                Destroy(obj);
            }

            //  show end screen
            scoreText.color = new Color(0.5f, 0.0f, 0.0f);
            UIpanel.transform.position = new Vector3(0, 0, 0);
            UIpanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 4.8f);//<VerticalLayoutGroup>().padding.bottom = 3;

            scoreText.text = " Game Over \n" +
                " SCORE " + paddleScript.score +
                "\n press enter to continue ";
            paddleScript.score = 0;
            gameState = winLose.lost;


            var bgMusic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
            bgMusic.mute = true;//.Stop(); // stop bg music
            bgMusic.Pause(); // stop bg music
            bgMusic.Stop(); // stop bg music
            audio.PlayOneShot(failSfx);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = UIpanel.transform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>();
        gameState = winLose.playing;
        scoreText.text = "SCORE\t" + paddleScript.score + " LIVES  " + paddleScript.lives;
        scoreText.color = new Color(1f, 1f, 1f);

        audio = GetComponent<AudioSource>();
    }
    
    

    // Update is called once per frame
    void Update()
    {

        if (gameState == winLose.playing)
        {
            scoreText.text = "SCORE  " + paddleScript.score + " LIVES  " + paddleScript.lives;
            CheckWin();
            CheckLose();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);//Application.LoadLevel(0);
            }

        }
        else if(Input.GetKeyDown(KeyCode.Return))// game is over, enter to return to menu
        {
            SceneManager.LoadScene(0); //Application.LoadLevel(0);
        }

    }
}
