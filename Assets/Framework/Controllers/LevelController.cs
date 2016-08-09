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
    public List<Toppings> selectedToppings = new List<Toppings>();
    public List<int> quantityToppings = new List<int>();
    public List<Condiments> selectedCondiments = new List<Condiments>();
    public List<int> quantityCondiments = new List<int>();

    //Rate of spawn for objects and bonuses.
    public float objectSpawnRate = 0.5f;
    public float bonusesSpawnRate = 0.1f;

    public bool isPlaying = false;

    public Transform startPOS;

    public GameObject[] toppings;
    public GameObject[] condiments;
    public GameObject[] bonuses;

    public Image li_1;
    public Image li_2;
    public Image li_3;
    public Sprite[] cSprites;
    public Sprite[] tSprites;

    public Transform targetPlayer;
    public Transform startingPosition;

    public Camera_Follow camScript;
    ///Private/////////////////////////////////////////////////////////////////////
    ///                                                                         ///
    ///                                                                         ///
    ///////////////////////////////////////////////////////////////////////////////   
    
    public bool isGoingDown = false;

    //Lists holding references to each spawned gameobject going up / down.
    List<GameObject> upSpawned = new List<GameObject>();
    List<GameObject> downSpawned = new List<GameObject>();

    float spawnedHeight;
    int downSpawnedHeight;

    void Awake () 
    {
        
        //Instance gets removed by any new instance created.
        if (instance != null)
        {
            Destroy(instance);
        }
        //Set our current level to this levelController.
        instance = this;

        spawnedHeight = 0f;
        isGoingDown = false;


#if UNITY_ANDROID
        if (!GameCenterLoading.instance.isConnected)
        {
            GameCenterLoading.instance.RetryConnection();
        }
#endif
    }
    void Start() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Gather all resources needed in this level.
        BuildLevel();

    }
    void Update() 
    {
        if (targetPlayer.position.y > spawnedHeight - 10f)
        {
            SpawnToppings(20f, false);
        }
        if (isGoingDown && targetPlayer.position.y < downSpawnedHeight + 5f)
        {
            SpawnCondiments(20);
        }
       
    }
    void BuildLevel() 
    {
        CreateSandwich(true);
        SpawnToppings(50);
    }
    //Creates the sandwich the player is trying to build. If isRandom = true we create a random sandwich.
    //Size = the number of condiments and toppings to be randomed.
    public void ClearForLanding()
    {
        foreach (GameObject g in downSpawned)
        {
            Destroy(g);
        }
    }
    void CreateSandwich(bool isRandom = false, int size = 3) 
    {
        selectedToppings.Clear();
        quantityToppings.Clear();
        selectedCondiments.Clear();
        quantityCondiments.Clear();
        if (isRandom)
        {
            for (int index = 0; index < 3; index++)
            {
                bool duplicated = false;
                int rngTop = 0;
                int rngCond = 0;
               
                do
                {
                    duplicated = false;
                    rngTop = Random.Range(0, (int)Toppings.COUNT);
                    for (int i = 0; i < selectedToppings.Count; i++)
                    {
                        if (selectedToppings[i] == (Toppings)rngTop)
                        {
                            
                            duplicated = true;
                            break;
                        }
                        
                    }
              
                } while (duplicated);

                selectedToppings.Add((Toppings)rngTop);

                do
                {
                    duplicated = false;
                    rngCond = Random.Range(0, (int)Condiments.COUNT);
                   
                    for (int j = 0; j < selectedCondiments.Count; j++)
                    {
                        if (selectedCondiments[j] == (Condiments)rngCond)
                        {
                            duplicated = true;
                            
                            break;
                        }
                       
                    }
                   
                } while (duplicated);

                selectedCondiments.Add((Condiments)rngCond);
            }

        }

        for (int j = 0 ; j < 3; j++)
        {
            quantityToppings.Add(Random.Range(1,4));
            quantityCondiments.Add(Random.Range(1, 4));
        }

        li_1.sprite = tSprites[(int)selectedToppings[0]];
        li_2.sprite = tSprites[(int)selectedToppings[1]];
        li_3.sprite = tSprites[(int)selectedToppings[2]];


        li_1.transform.FindChild("Text").GetComponent<Text>().text = quantityToppings[0].ToString();
        li_2.transform.FindChild("Text").GetComponent<Text>().text = quantityToppings[1].ToString();
        li_3.transform.FindChild("Text").GetComponent<Text>().text = quantityToppings[2].ToString();

    }
    void SpawnToppings(float amount, bool initial = true) 
    {
        
        float xPOS;
        float yPOS;

        foreach (GameObject g in downSpawned)
        {
           Destroy(g);          
        }
        


        if (initial)
        {
            foreach (GameObject g in upSpawned)
            {
                Destroy(g);
            }
            upSpawned.Clear();
            spawnedHeight = startPOS.position.y + 5f;
        }
        

        for (int i = 0; i < amount; i++)
        {
            if (Random.Range(0f, 1f) < bonusesSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = i + spawnedHeight;
                GameObject newlySpawned = Instantiate(bonuses[0], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                upSpawned.Add(newlySpawned);
            }
            else if (Random.Range(0f, 1f) < objectSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = i + spawnedHeight;
                GameObject newlySpawned = Instantiate(toppings[Random.Range(0, (int)Toppings.COUNT)], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                upSpawned.Add(newlySpawned);
            }

        }

        spawnedHeight += amount;
    }
    public void SpawnCondimentsTurn() 
    {
        
        li_1.sprite = cSprites[(int)selectedCondiments[0]];
        li_2.sprite = cSprites[(int)selectedCondiments[1]];
        li_3.sprite = cSprites[(int)selectedCondiments[2]];

        li_1.transform.FindChild("Text").GetComponent<Text>().text = quantityCondiments[0].ToString();
        li_2.transform.FindChild("Text").GetComponent<Text>().text = quantityCondiments[1].ToString();
        li_3.transform.FindChild("Text").GetComponent<Text>().text = quantityCondiments[2].ToString();

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

        int height = ((int)targetPlayer.position.y) - 3;

        for (int i = 0; i < 15; i++)
        {
            if (Random.Range(0f, 1f) < objectSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = height - i;
                GameObject newlySpawned = Instantiate(condiments[Random.Range(0, (int)Condiments.COUNT)], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                downSpawned.Add(newlySpawned);
            }

        }
        isGoingDown = true;
        downSpawnedHeight = height - 15;
        Debug.Log("Height:" + downSpawnedHeight);
    }
    void SpawnCondiments(int num)
    {
        Debug.Log("Height:"  + downSpawnedHeight);
        float xPOS;
        float yPOS;

        int height = downSpawnedHeight -1;

        for (int i = 0; i < num; i++)
        {
            if (Random.Range(0f, 1f) < objectSpawnRate)
            {
                xPOS = Random.Range(-1.2f, 1.2f);
                yPOS = height - i;
                GameObject newlySpawned = Instantiate(condiments[Random.Range(0, (int)Condiments.COUNT)], new Vector3(xPOS, yPOS, -7.497f), Quaternion.identity) as GameObject;
                downSpawned.Add(newlySpawned);
            }

        }
        downSpawnedHeight -= num;
       
    }
    public void SetCheckMarks(int c)
    {
        SoundController.instance.PlayClip(Random.Range(5,7));
        switch (c)
        {
            case 1:
                li_1.transform.FindChild("Image").gameObject.SetActive(true);
                li_1.transform.FindChild("Text").gameObject.SetActive(false);
                break;
            case 2:
                li_2.transform.FindChild("Image").gameObject.SetActive(true);
                li_2.transform.FindChild("Text").gameObject.SetActive(false);
                break;
            case 3:
                li_3.transform.FindChild("Image").gameObject.SetActive(true);
                li_3.transform.FindChild("Text").gameObject.SetActive(false);
                break;
            case 999:
                li_1.transform.FindChild("Image").gameObject.SetActive(false);
                li_2.transform.FindChild("Image").gameObject.SetActive(false);
                li_3.transform.FindChild("Image").gameObject.SetActive(false);
                li_1.transform.FindChild("Text").gameObject.SetActive(true);
                li_2.transform.FindChild("Text").gameObject.SetActive(true);
                li_3.transform.FindChild("Text").gameObject.SetActive(true);
                break;
        }
    }
    public void Replay()
    {
        //SceneManager.LoadScene(1);
        targetPlayer.position = startingPosition.position;
        targetPlayer.rotation = startingPosition.rotation;

        

        foreach (Transform child in targetPlayer.transform.FindChild("GatherLocation"))
        {
            GameObject.Destroy(child.gameObject);
        }

        isGoingDown = false;
        downSpawnedHeight = 0;
        BuildLevel();
        SetCheckMarks(999);
        Player.instance.ResetValues();
        camScript.ResetValues();
    }
    public void LoadHome()
    {
        GameManager.instance.Load(1);
    }
}
