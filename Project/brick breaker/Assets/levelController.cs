using UnityEngine;
using Button = UnityEngine.UI.Button;
using UnityEngine.SceneManagement;

//enum levelNumber {a1,b1,c1,d1,a2,b2,c2,d2,a3,b3,c3,d3,a4,b4,c4,d4 };

public class levelController : MonoBehaviour
{

    int levelToLoad = -1;
    int fromGameLevel = -1;
    GameObject BlackScreenOverlay;
    Vector3 overlayHiddenSize = Vector3.up * 30.0f;// (0,30,0)
    int levelIntroTimer = 0;

    [SerializeField] protected Vector3[] LevelSelectUIOffsets;

    static bool init = false;

    /// called by the UI buttons
    public void SetLevel(int level)
    {
        levelToLoad = level; 

        if (levelToLoad == 16)
            fromGameLevel = (SceneManager.GetActiveScene().buildIndex + 1) / 4;
        else
            fromGameLevel = -1;
    }

    private void Awake()
    {
        if (!init)
        {
            DontDestroyOnLoad(gameObject);
            init = true;
        }
        Debug.Log("lvlCont. Awake called");
    }

    private void Start()
    {
        HighscoreManager.Load();
        ApplyLevelUnlocks();
        BlackScreenOverlay = gameObject;
        BlackScreenOverlay.transform.localScale = overlayHiddenSize;

        if (fromGameLevel >= 0 && SceneManager.GetActiveScene().buildIndex == 16)
        {
            Debug.Log("lvlController - re-enabling animator");
            GameObject.Find("Canvas").GetComponent<Animator>().enabled = true;
            fromGameLevel = -1;
        }
        //Debug.Log("lvlCont. Start called");
        SceneManager.sceneLoaded += OnLevelLoaded; // start listening for a sceneLoad event.
    }

    private void Update()
    {
        if (levelToLoad < 0 && BlackScreenOverlay.transform.localScale.x <= 0)
        {// mid-game
            return;
        }
        else if (levelToLoad < 0)// just finished loading a level
        {
            

            if (levelIntroTimer > 0)
            {
                levelIntroTimer--;
                if(levelIntroTimer > 1)
                    return; // keep going
                else
                {   // black wipe is close to finishing - finish now.
                    GameObject.Find("Panel").GetComponent<GUItext>().SwitchToInGameDisplay();
                    levelIntroTimer = 0;
                }
            }
            if (BlackScreenOverlay.transform.localScale.x > 1) // level just loaded
            {
                BlackScreenOverlay.transform.localScale += Vector3.left; // shrink horizontally
                //Debug.Log("Shrinking");
            }
            else
            {
                BlackScreenOverlay.transform.localScale = overlayHiddenSize;
                //Debug.Log("Shrunk");
            }
        }
        else if (levelToLoad < 0){}
        else if (BlackScreenOverlay.transform.localScale.x > 40)
        {
            SceneManager.LoadScene(levelToLoad); 

            levelToLoad = -1;
        }
        else
        {
            BlackScreenOverlay.transform.localScale += Vector3.right; // grow horizontally
        }
    }

    void OnEnable() {  } 
    void OnDisable() { SceneManager.sceneLoaded -= OnLevelLoaded; } // stop listening for a sceneLoad event. (unsubscribe)
    
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnLevelLoaded: " + scene.name + " " + scene.buildIndex);
        if (scene.buildIndex != 16)
        {
            Debug.Log("New Level: " + scene.name + " latest Score: " + HighscoreManager.data.scores[scene.buildIndex]);
            levelIntroTimer = 240;
            GameObject.Find("Panel").GetComponent<GUItext>().ShowLevelIntro(scene.buildIndex);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("from game level: " + fromGameLevel);
            if (fromGameLevel >= 0 && fromGameLevel < 4 )
            { 
                var UI = GameObject.Find("Canvas"); 
                if (UI == null) return;
                var posLock = UI.GetComponent<Animator>();
                
                Debug.Log("Moving to -: " + LevelSelectUIOffsets[fromGameLevel]);
                posLock.enabled = false;
                UI.transform.position = LevelSelectUIOffsets[fromGameLevel];
                UI.GetComponent<RectTransform>().position = LevelSelectUIOffsets[fromGameLevel];
                UI.GetComponent<RectTransform>().anchoredPosition = LevelSelectUIOffsets[fromGameLevel];
            }
        }
        BlackScreenOverlay.transform.localScale = overlayHiddenSize + (Vector3.right * 40);// cover full screen black
    }

    void ApplyLevelUnlocks()
    {
        if(SceneManager.GetActiveScene().name == SceneManager.GetSceneByBuildIndex(16).name)
        {
            var levelButtons = GameObject.FindGameObjectsWithTag("level_button");
            for (int lvl = 0; lvl <16; lvl++)
            {
                var button = levelButtons[lvl].GetComponent<Button>();
                GameObject nextLevel; 
                switch (lvl)
                {
                    case 4 : nextLevel = GameObject.Find("level 1 to 2 button");
                        break;
                    case 8 : nextLevel = GameObject.Find("level 2 to 3 button");
                        break;
                    case 12 : nextLevel = GameObject.Find("level 4 button");
                        break;
                    default : nextLevel = null;
                        break;
                };
                int scoreThreshold = lvl * 1000;
                if (HighscoreManager.data.scores[lvl>0?lvl-1:0] >= scoreThreshold)
                {
                    Debug.Log($"Button lvl {lvl} - score {scoreThreshold} passed: unlocked");
                    button.interactable = true;
                    if (nextLevel) nextLevel.SetActive(true);
                }
                else
                {
                    //Debug.Log($"Button lvl {lvl} - score {scoreThreshold}: locked");
                    button.interactable = false;
                    if (nextLevel) nextLevel.SetActive(false);
                }
            }
        }
    }

    public static bool IsLevelUnlocked(int lvl)
    {
        if (lvl <= 0) return true;
        if (lvl > HighscoreManager.data.scores.Length) return false;

        int scoreThreshold = lvl * 1000;
        Debug.Log("isUnlocked? score " + HighscoreManager.data.scores[lvl - 1]);
        return HighscoreManager.data.scores[lvl - 1] >= scoreThreshold;
    }
    
}
