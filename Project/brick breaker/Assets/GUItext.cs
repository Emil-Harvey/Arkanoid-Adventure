using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum winLose { won,playing,lost,intro };

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

        if(allObjects.Length <= 1 || Input.GetKeyDown(KeyCode.BackQuote))
        {// all bricks broken

            foreach (GameObject obj in allObjects)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ball"))
                Destroy(obj);

            int FinalScore = paddleScript.score * (1 + paddleScript.lives);

            UIpanel.transform.position = new Vector3(0, 0, 0);
            UIpanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 4.8f);//UIpanel.GetComponent<VerticalLayoutGroup>().padding.bottom = 3;
            scoreText.color = new Color(0.25f, 0.90f, 0.1f);
            scoreText.text = " Well Done! level complete \n" +
                " SCORE  " + FinalScore +
                "\n press enter to continue ";

            gameState = winLose.won;

            HighscoreManager.AddScoreToProfile(FinalScore, SceneManager.GetActiveScene().buildIndex);
            Debug.Log("saved Score to profile: " + FinalScore + ", level: " + SceneManager.GetActiveScene().buildIndex);

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
                Destroy(obj);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ball"))
                Destroy(obj);
            

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

    public void ShowLevelIntro(int LevelNum)// level name??
    {
        gameState = winLose.intro;
        scoreText.color = new Color(0.5f, 0.6f, 1.0f);//Pale blue
        UIpanel.transform.position = new Vector3(0, 0, 0);
        UIpanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 4.8f);//<VerticalLayoutGroup>().padding.bottom = 3;

        scoreText.text = " Level " +LevelNum + " \n"+
            " HI SCORE " + HighscoreManager.data.scores[LevelNum] + " \t";
    }
    public void SwitchToInGameDisplay() // mid game
    {
        UIpanel.transform.position = new Vector3(0, 12, 0);// at top of level
        UIpanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 1.6f);
        scoreText.text = "SCORE\t" + paddleScript.score + " LIVES  " + paddleScript.lives;
        scoreText.color = new Color(1f, 1f, 1f);

        gameState = winLose.playing;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = UIpanel.transform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>();
        //gameState = winLose.playing;

        //SwitchToInGameHUD();
        audio = GetComponent<AudioSource>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (gameState == winLose.intro) { return; }

        if (gameState == winLose.playing)
        {
            scoreText.text = "SCORE  " + paddleScript.score + " LIVES  " + paddleScript.lives;
            CheckWin();
            CheckLose();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("LevelTransition").GetComponent<levelController>().SetLevel(16); //SceneManager.LoadScene(16);//Application.LoadLevel(0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))// game is over, enter to return to menu
        {
            GameObject.Find("LevelTransition").GetComponent<levelController>().SetLevel(16); //Application.LoadLevel(0);
        }
#if UNITY_WEBGL
        else if (Input.GetMouseButtonDown(0)) // let web/mobile users click screen to continue
        {
            GameObject.Find("LevelTransition").GetComponent<levelController>().SetLevel(16);
        }
#endif //UNITY_WEBGL

    }
}
