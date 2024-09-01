using UnityEngine;
using UnityEngine.SceneManagement;

enum winLose { won,playing,lost,intro };

public class GUItext : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject UIPanel;

    private winLose gameState;

    AudioSource audio;
    public AudioClip winSfx;
    public AudioClip failSfx;
    
    void CheckWin()
    {

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("perish");

        if (allObjects.Length > 1 && !Input.GetKeyDown(KeyCode.BackQuote))
            return;
        
        //else: all bricks broken

        foreach (GameObject obj in allObjects)
            Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ball"))
            Destroy(obj);

        int FinalScore = paddleScript.score * (1 + paddleScript.lives);

        int lvl = SceneManager.GetActiveScene().buildIndex;
        bool wasNextLvlUnlocked = levelController.IsLevelUnlocked(lvl + 1);

        HighscoreManager.AddScoreToProfile(FinalScore, lvl);
        HighscoreManager.Save();
        Debug.Log("saved Score to profile: " + FinalScore + ", level: " + lvl);

        bool newLvlUnlocked = (lvl % 4 == 0) && !wasNextLvlUnlocked && levelController.IsLevelUnlocked(lvl + 1);
        string unlockMessage = newLvlUnlocked? "\n NEW LEVEL UNLOCKED" : "";

        UIPanel.transform.position = new Vector3(0, 0, 0);
        UIPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, newLvlUnlocked? 6.8f : 4.8f);//UIpanel.GetComponent<VerticalLayoutGroup>().padding.bottom = 3;
        scoreText.color = new Color(0.25f, 0.90f, 0.1f);
        scoreText.text = " Well Done! level complete " 
                        + "\n SCORE  "  +  FinalScore 
                        +  unlockMessage
                        + "\n press enter to continue ";

        gameState = winLose.won;


        var bgMusic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        bgMusic.mute = true;//.Stop(); // stop bg music
        bgMusic.Pause(); // stop bg music
        bgMusic.Stop(); // stop bg music
        audio.PlayOneShot(winSfx);

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
            UIPanel.transform.position = new Vector3(0, 0, 0);
            UIPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 4.8f);//<VerticalLayoutGroup>().padding.bottom = 3;

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
        UIPanel.transform.position = new Vector3(0, 0, 0);
        UIPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 4.8f);//<VerticalLayoutGroup>().padding.bottom = 3;

        scoreText.text = " Level " +LevelNum + " \n"+
            " HI SCORE " + HighscoreManager.data.scores[LevelNum] + " \t";
    }
    public void SwitchToInGameDisplay() // mid game
    {
        UIPanel.transform.position = new Vector3(0, 12, 0);// at top of level
        UIPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 1.6f);
        scoreText.text = "SCORE\t" + paddleScript.score + " LIVES  " + paddleScript.lives;
        scoreText.color = new Color(1f, 1f, 1f);

        gameState = winLose.playing;
    }

    // Awake is alled when the script instance is being loaded
    private void Awake()
    {
        UIPanel = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        scoreText = UIPanel.transform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>();
        
        if (gameState != winLose.intro && gameState != winLose.playing) // only if started debug
            gameState = winLose.playing;

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
                //GameObject.Find("LevelTransition").GetComponent<levelController>().SetLevel(16); //SceneManager.LoadScene(16);
                paddleScript.lives = -1;
                CheckLose();
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
