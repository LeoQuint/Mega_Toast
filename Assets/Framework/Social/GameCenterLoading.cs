using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameCenterLoading : MonoBehaviour
{

    public static GameCenterLoading instance = null;

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
   
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.A))
        Social.ShowAchievementsUI();
        if (Input.GetKeyDown(KeyCode.L))
            Social.ShowLeaderboardUI();
    }



    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    void ProcessAuthentication(bool success)
    {
        if (success)
        {
            Debug.Log("Authenticated, checking achievements");

            // Request loaded achievements, and register a callback for processing them
            Social.LoadAchievements(ProcessLoadedAchievements);
        }
        else
            Debug.Log("Failed to authenticate");
    }

    // This function gets called when the LoadAchievement call completes
    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
            Debug.Log("Got " + achievements.Length + " achievements");
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

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }
    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }


}
