using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameCenterLoading : MonoBehaviour
{

    public static GameCenterLoading instance = null;
    public bool isConnected = false;


    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        
#if UNITY_ANDROID
        // PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
        Social.localUser.Authenticate(ProcessAuthentication);
#endif
#if UNITY_EDITOR
        LoadMainScene();
#endif

    }


    public void RetryConnection()
    {
        StartCoroutine(WaitForRetry());
    }

    IEnumerator WaitForRetry()
    {
        yield return new WaitForSeconds(1f);
        if (!isConnected)
        {
            Social.localUser.Authenticate(ProcessAuthentication);
        }
    }


    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    void ProcessAuthentication(bool success)
    {
        Scene scene = SceneManager.GetActiveScene();
        if (success)
        {
            isConnected = true;
            Debug.Log("Authenticated, checking achievements");
            // Request loaded achievements, and register a callback for processing them
            Social.LoadAchievements(ProcessLoadedAchievements);
            UpdateCoins();
            if (scene.name == "loading")
            {
                LoadMainScene();
            }       
        }
        else
        {
            isConnected = false;
            Debug.Log("Failed to authenticate");
            if (scene.name == "loading")
            {
                LoadMainScene();
            }
        }

    }

    void LoadMainScene()
    {
        
        SceneManager.LoadScene(1);
    }

    // This function gets called when the LoadAchievement call completes
    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
        {
            Debug.Log("achievements loaded");
        }
           
    }

    public void UnlockAchievement(string achievementID)
    {
        Social.ReportProgress(achievementID, 100.0f, (bool success) => {
            // handle success or failure
        });
    }
    public void RevealAchievement(string achievementID)
    {
        Social.ReportProgress(achievementID, 0.0f, (bool success) => {
            // handle success or failure
        });
    }
    public void AddProgressToCompletedSand()
    {
        UnlockAchievement("CgkIm8DKqdILEAIQAg");
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkIm8DKqdILEAIQBA", 1, (bool success) => {
            // handle success or failure
        });
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkIm8DKqdILEAIQBQ", 1, (bool success) => {
            // handle success or failure
        });
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkIm8DKqdILEAIQBg", 1, (bool success) => {
            // handle success or failure
        });
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkIm8DKqdILEAIQBw", 1, (bool success) => {
            // handle success or failure
        });
    }

    public void AddProgressToPerfectLaunch()
    {
        UnlockAchievement("CgkIm8DKqdILEAIQEg");
 
    }

    public void AddCoins(int coinCount)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(
       "CgkIm8DKqdILEAIQFQ", coinCount, (bool success) => {
           //UpdateCoins();
        });
        
    }

    public void AddProgressToPerfectLanding()
    {
        UnlockAchievement("CgkI09G1lLUQEAIQFw");
        RevealAchievement("CgkI09G1lLUQEAIQGA");
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkI09G1lLUQEAIQGA", 1, (bool success) => {
            // handle success or failure
        });
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkI09G1lLUQEAIQGQ", 1, (bool success) => {
            // handle success or failure
        });
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkI09G1lLUQEAIQGg", 1, (bool success) => {
            // handle success or failure
        });
        PlayGamesPlatform.Instance.IncrementAchievement(
        "CgkI09G1lLUQEAIQGw", 1, (bool success) => {
            // handle success or failure
        });
    }


    public void ShowAchievements()
    {
        if (isConnected)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            Social.localUser.Authenticate(ProcessAuthentication);
        }
    }
    public void ShowLeaderboard()
    {
        if (isConnected)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            Social.localUser.Authenticate(ProcessAuthentication);
        }
    }
    public void PostToLeaderboard(int score)
    {
      
        Social.ReportScore(score, "CgkIm8DKqdILEAIQFA", (bool success) => {
            // handle success or failure
        });
    }

    public void UpdateCoins()
    {
        string coinTotal = "";
        Social.LoadAchievements(achievements => {
            if (achievements.Length > 0)
            {
                
                foreach (IAchievement achievement in achievements)
                {
                    if (achievement.id == "CgkIm8DKqdILEAIQFQ")
                    {
                        coinTotal = (10000 * achievement.percentCompleted).ToString();
                        GameObject.FindGameObjectWithTag("coin").GetComponent<Text>().text = coinTotal;
                    }
                       
                }
                
            }
            else
                Debug.Log("No achievements returned");
        });
        
    }


}
