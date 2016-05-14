using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum Toppings 
{
    Tomato,
    Lettuce,
    Bacon,
    Swiss,
    Ham,
    Onions,
    Pickles,
    Egg,
    COUNT
}
public enum Condiments
{
    Mustard,
    Ketchup,
    Salt,
    Peper,
    COUNT
}

public class LevelController : MonoBehaviour {

    public static LevelController Instance;

    public bool isPlaying = false;

    public GameObject[] toppings = new GameObject[8];
    public Sprite[] tSprites = new Sprite[8];

    public GameObject[] condiments = new GameObject[4];
    public Sprite[] cSprites = new Sprite[4];

    public Toppings[] selectedToppings = new Toppings[3];
    public Condiments[] selectedCondiments = new Condiments[3];


    public GameObject pepperBoost;
    public Transform targetPlayer;

    public float objectSpawnRate = 0.5f;
    public float pepperSpawnRate = 0.1f;

    List<GameObject> upSpawned = new List<GameObject>();
    List<GameObject> downSpawned = new List<GameObject>();



    public Image li_1;
    public Image li_2;
    public Image li_3;


    float spawnedHeight = 0f;

	// Use this for initialization
	void Awake () 
    {
       
        Instance = this;
        

        spawnedHeight = 0f;
        bool duplicated = false;
        do
        {
            duplicated = false;
            List<int> randomedT = new List<int>();
            List<int> randomedC = new List<int>();
            for (int i = 0; i < selectedToppings.Length; i++)
            {
                selectedToppings[i] = (Toppings)Random.Range(0, (int)Toppings.COUNT);
                foreach (int num in randomedT)
                {
                    if (num == (int)selectedToppings[i])
                    {
                        duplicated = true;
                    }
                }
                randomedT.Add((int)selectedToppings[i]);
            }

            for (int i = 0; i < selectedCondiments.Length; i++)
            {
                selectedCondiments[i] = (Condiments)Random.Range(0, (int)Condiments.COUNT);
                foreach (int num in randomedC)
                {
                    if (num == (int)selectedCondiments[i])
                    {
                        duplicated = true;
                    }
                }
                randomedC.Add((int)selectedCondiments[i]);
            }
        }
        while (duplicated);

        li_1.sprite = tSprites[(int)selectedToppings[0]];
        li_2.sprite = tSprites[(int)selectedToppings[1]];
        li_3.sprite = tSprites[(int)selectedToppings[2]];

        SpawnUpObjects(100f);
	}


    void Update() 
    {
        if (!isPlaying)
        {
            return;
        }
        if (targetPlayer.GetComponent<Player>().playerStatus == Status.LANDED)
        {
            return;
        }
        if (targetPlayer.GetComponent<Player>().playerStatus == Status.DEAD)
        {
            return;
        }
        

        if (targetPlayer.position.y > spawnedHeight - 10f)
        {
            SpawnUpObjects(20f);
        }
    }

    public void SetCheckMarks(int i)
    {
        switch (i)
        {
            case 0:
                li_1.transform.FindChild("Image").gameObject.SetActive(true);
                break;
            case 1:
                li_2.transform.FindChild("Image").gameObject.SetActive(true);
                break;
            case 2:
                li_3.transform.FindChild("Image").gameObject.SetActive(true);
                break;
            case 999:
                li_1.transform.FindChild("Image").gameObject.SetActive(false);
                li_2.transform.FindChild("Image").gameObject.SetActive(false);
                li_3.transform.FindChild("Image").gameObject.SetActive(false);
                break;
        }
    }
    

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


    }

    public void Replay() 
    {
        SceneManager.LoadScene(0);
    }
}
