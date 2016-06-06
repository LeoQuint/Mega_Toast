using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


using NS_Level;

public class LevelController : MonoBehaviour {

    ///Static//////////////////////////////////////////////////////////////////////
    ///                                                                         ///
    ///                                                                         ///
    ///////////////////////////////////////////////////////////////////////////////   

    //Only one instance of levelController for each levels.
    public static LevelController instance;

    ///Public//////////////////////////////////////////////////////////////////////
    ///                                                                         ///
    ///                                                                         ///
    ///////////////////////////////////////////////////////////////////////////////  
                                                                      
    //Selected Toppings and condiments for the sandwich built in this level.
    public Toppings[] selectedToppings;
    public Condiments[] selectedCondiments;
    //Lists of available toppings/condiments for this level.
    public Toppings[] availableToppings;
    public Condiments[] availableCondiments;
    //Rate of spawn for objects and bonuses.
    public float objectSpawnRate = 0.5f;
    public float bonusesSpawnRate = 0.1f;

    ///Private/////////////////////////////////////////////////////////////////////
    ///                                                                         ///
    ///                                                                         ///
    ///////////////////////////////////////////////////////////////////////////////   

    //Lists of gameobject toppings, condiments and bonuses available.
    List<GameObject> toppings = new List<GameObject>();
    List<GameObject> condiments = new List<GameObject>();
    List<GameObject> bonuses = new List<GameObject>();
    //Reference to the player position. Used to spawn/despawn objects in the scene.   
    Transform targetPlayer;
    //Lists holding references to each spawned gameobject going up / down.
    List<GameObject> upSpawned = new List<GameObject>();
    List<GameObject> downSpawned = new List<GameObject>();

	void Awake () 
    {
        //Instance gets removed by any new instance created.
        if (instance != null)
        {
            Destroy(instance);
        }
	}

    void Start() 
    {
        //Set our current level to this levelController.
        instance = this;
        //Gather all resources needed in this level.
        

    }
    //
    void BuildLevel() 
    {
        
    }
    //Creates the sandwich the player is trying to build. If isRandom = true we create a random sandwich.
    //Size = the number of condiments and toppings to be randomed.
    void CreateSandwich(bool isRandom = false, int size = 3) 
    {
        if (isRandom)
        { 
            
        }
    }

    void SpawnToppings() 
    {
    
    }

    void SpawnCondiments() 
    {
    
    }


    /*
    void SpawnUpObjects(float amount)
    {
        float xPOS;
        float yPOS;

        for (int i = 0; i < amount; i++)
        {
            if (Random.Range(0f, 1f) < bonusesSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = transform.position.y + 5f + i;
                GameObject newlySpawned = Instantiate(bonuses[0], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                upSpawned.Add(newlySpawned);
            }
            else if (Random.Range(0f, 1f) < objectSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = transform.position.y + 5f + i;
                GameObject newlySpawned = Instantiate(toppings[Random.Range(0, (int)Toppings.COUNT)], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                upSpawned.Add(newlySpawned);
            }

        }

        //spawnedHeight += transform.position.y + amount;

    }*/

    /*

    void SpawnUpObjects(float amount) 
    {
        float xPOS;
        float yPOS;

        for (int i = 0; i < amount; i++)
        {
            if(Random.Range(0f, 1f) < pepperSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = transform.position.y + 5f + i + spawnedHeight;
                GameObject newlySpawned = Instantiate(pepperBoost, new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                upSpawned.Add(newlySpawned);
            }
            else if (Random.Range(0f, 1f) < objectSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = transform.position.y + 5f + i + spawnedHeight;
                GameObject newlySpawned = Instantiate(toppings[Random.Range(0, (int)Toppings.COUNT)], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                upSpawned.Add(newlySpawned);
            }
            
        }

        spawnedHeight += transform.position.y + amount;
       
    }

    public void SpawnDownObjects() 
    {
        li_1.sprite = cSprites[(int)selectedCondiments[0]];
        li_2.sprite = cSprites[(int)selectedCondiments[1]];
        li_3.sprite = cSprites[(int)selectedCondiments[2]];

        SetCheckMarks(999);

        foreach (GameObject g in upSpawned)
        {
            if (g.transform.parent == null)
            {
                
                Destroy(g);
            }
        }

        float xPOS;
        float yPOS;

        int height = ((int)targetPlayer.position.y) - 15;

        for (int i = 0; i < height; i++)
        {
            if (Random.Range(0f, 1f) < objectSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = 15 + i;
                GameObject newlySpawned = Instantiate(condiments[Random.Range(0, (int)Condiments.COUNT)], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                downSpawned.Add(newlySpawned);
            }

        }


    }*/

}
