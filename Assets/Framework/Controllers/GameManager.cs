using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Represents the status of the application.
public enum AppStatus 
{
    LOADING,    //Application status during loading.
    MENU,       //Application status while navigating menues.
    SHOP,       //Application status while browsing the shop.
    PLAYING,    //Application status during gameplay.
    EXIT        //Application status when closing.
}
///Game Manager is loaded first and closed last. 
///Holds the user's data and unlocked bonuses.
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;              //Static instance of GameManager.

    //Current Status of our application. Public getter and private setter. Set is done inside the class.
    public AppStatus _appStatus { get; private set; }
    //Current loaded LevelController. Public getter and private setter. Set is done inside the class.
    public LevelController _levelController { get; private set; }
    
    
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



   
}
