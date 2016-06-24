using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using NS_Level;

public enum PlayerStatus 
{
    INTRO,
    CHARGING,
    GOINGUP,
    GOINGDOWN,
    OVERHEAD,
    LANDED,
    DEAD
}

public class Player : MonoBehaviour {

    //newer CODE

    public static Player instance;

    public GameObject player;

    //Delegate used to controls/movements.
    delegate void mDelegate();
    mDelegate mDel;

    //Player Stats. Holds the modifier and stats of a particular type of bread or player.
    public PlayerData pStats;

    [SerializeField]
    float flipDelay = 0.5f;
  
    public PlayerStatus playerStatus;

    Quaternion upRotation = new Quaternion(0f, 0f, 0f, 1f);
    Quaternion downRotation = new Quaternion(-1f, 0f, 0f, 0f);
    Quaternion initialRotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);

    public BitArray playerStats = new BitArray(32);

 

    public Mesh[] meshes;
    public Material[] materials;
   

    /// OLD CODE
    
    

    Quaternion stepStartRotation;
    Quaternion stepEndRotation;

    private float stepStartTime;
    public float flipDuration = 1f;

    public float jumpForce = 500f;
    public float movementSpeed = 20f;
    float movement = 0f;
    float movementY = 0f;
    public float tiltMovespeed = 2000f;

    Rigidbody rb;
    //UI elements
    public Button launchBtn;
    public Slider powerBar;
    public Text speedTracker;
    public Text scoreTracker;
    public Text heightTracker;
    public GameObject multiplierTracker;
    public GameObject picMenu;
    public GameObject GameUI;

    int score;

    public Button replayBtn;

    public Image listDisplay;
    //Table
    public GameObject toaster;
    public GameObject plateAndBread;

    public GameObject endPoint;

    public Camera playerPictureCam;

    private List<bool> toppingCollected = new List<bool>();
    private List<bool> condimentCollected = new List<bool>();

    public bool enableTiltControls = false;

    bool hasStarted = false;

    void Awake()
    {
        //Instance gets removed by any new instance created.
        if (instance != null)
        {
            Destroy(instance);
        }
        //Set our current level to this levelController.
        instance = this;
        score = 0;
    }

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update() 
    {
       
        if (!LevelController.instance.isPlaying)
        {
            return;
        }
        if (!hasStarted)
        {
            return;
        }
        if (playerStatus == PlayerStatus.LANDED || playerStatus == PlayerStatus.DEAD)
        {
            replayBtn.gameObject.SetActive(true);
            return;
        }
        if (playerStatus == PlayerStatus.INTRO)
        {
            return;
        }
        

        //speedTracker.text = rb.velocity.y.ToString("F1") + " Kmph";
       /* if (maxHeightAchived < transform.position.y)
        {
            maxHeightAchived = transform.position.y;
            heightTracker.text = "Max height: " + maxHeightAchived.ToString("F1") + " Meters";
        }*/
    
        if (mDel != null)
        {
            mDel();
        }

        

        if (playerStatus == PlayerStatus.CHARGING)
        {
            
            if (Input.GetAxis("Jump") > 0)
            {
                playerStatus = PlayerStatus.GOINGUP;
                powerBar.GetComponent<PowerBar>().hasLaunched = true;
                StartCoroutine(DisplayPower());
                //speedTracker.gameObject.SetActive(true);
                //heightTracker.gameObject.SetActive(true);
                scoreTracker.gameObject.SetActive(true);
                float forceAdded = jumpForce * Mathf.Pow( (0.5f), (1f - powerBar.value) );
               
                rb.AddForce(new Vector3(0f, forceAdded, 0f));
                stepEndRotation = upRotation;
                StartCoroutine(Wait(1f));
            }
            

        }
       

        if (playerStatus == PlayerStatus.OVERHEAD)
        {
            if (enableTiltControls)
            {
                TiltControls();
            }
         
           
            movementY = Input.GetAxis("Vertical");
            rb.AddForce(Vector3.forward * movementY * movementSpeed * -1f);
            movement = Input.GetAxis("Horizontal");
            rb.AddForce(Vector3.right * movement * movementSpeed * -1f);
        }
        else 
        {
            if (enableTiltControls)
            {
                TiltControls();
            }
           
            movement = Input.GetAxis("Horizontal");
            rb.AddForce(Vector3.right * movement * movementSpeed);
        }


        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -2f, 2f), Mathf.Clamp(rb.velocity.y, -30f, 30f), 0f);

        Vector3 clampedX = new Vector3(Mathf.Clamp(player.transform.position.x, -1.2f, 1.2f), player.transform.position.y, player.transform.position.z);
        player.transform.position = clampedX;

        if (rb.velocity.y < -2f && playerStatus == PlayerStatus.GOINGUP)
        {
            playerStatus = PlayerStatus.GOINGDOWN;

            stepStartTime = Time.time;
            stepStartRotation = upRotation;
            stepEndRotation = downRotation;

            foreach (bool b in toppingCollected)
            {
                if (!b)
                {
                    ExplosiveDeath();
                    return;
                }
            }

            LevelController.instance.SpawnCondiments();
            
            mDel += Flip;
            mDel += MoveList;
        }

    }



    public void ChangeModel(string bread) 
    {
        switch(bread)
        {
            case "toast":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[0];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[0];
               
                player.transform.FindChild("Mesh").rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);

                //end point
                endPoint.transform.rotation = new Quaternion(0f, -1f, 0f, 0f);
                endPoint.GetComponent<MeshFilter>().mesh = meshes[0];
                endPoint.GetComponent<MeshRenderer>().material = materials[0];

                break;

            case "bagel":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];
                
                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp1":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp2":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp3":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp4":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp5":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp6":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp7":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
            case "temp8":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];

                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);

                //end point
                endPoint.transform.rotation = new Quaternion(-0.7f, 0f, 0f, -0.7f);

                endPoint.GetComponent<MeshFilter>().mesh = meshes[1];
                endPoint.GetComponent<MeshRenderer>().material = materials[1];
                break;
        }
           
   
    }

    public void StartGame() 
    {

        for (int i = 0; i < LevelController.instance.selectedToppings.Count; i++)
        {
            toppingCollected.Add(false);
        }
        for (int i = 0; i < LevelController.instance.selectedCondiments.Count; i++)
        {
            condimentCollected.Add(false);
        }

        playerStatus = PlayerStatus.INTRO;
        rb.isKinematic = false;

        StartCoroutine(DelayBreadLoading());
    }

    IEnumerator DelayBreadLoading()
    {
        

        yield return new WaitForSeconds(2f);
        playerStatus = PlayerStatus.CHARGING;
        hasStarted = true;
        powerBar.gameObject.SetActive(true);
        listDisplay.gameObject.SetActive(true);
    }


    public void Jump()
    {
        if (playerStatus != PlayerStatus.CHARGING)
        {
            return;
        }
            playerStatus = PlayerStatus.GOINGUP;
            powerBar.GetComponent<PowerBar>().hasLaunched = true;
            StartCoroutine(DisplayPower());
            //speedTracker.gameObject.SetActive(true);
            //heightTracker.gameObject.SetActive(true);
            scoreTracker.gameObject.SetActive(true);
            float forceAdded = jumpForce * Mathf.Pow((0.5f), (1f - powerBar.value));

            rb.AddForce(new Vector3(0f, forceAdded, 0f));
            stepEndRotation = upRotation;
            StartCoroutine(Wait(1f));
        
    }
    void Flip()
    {

        float step = (Time.time - stepStartTime) / flipDuration;
        player.transform.rotation = Quaternion.Lerp(stepStartRotation, stepEndRotation, step);

        if (step >= 1f)
        {
            mDel -= Flip;
            mDel -= MoveList;
        }
    }

    IEnumerator DisplayPower()
    {
        yield return new WaitForSeconds(2f);
        powerBar.gameObject.SetActive(false);
    }
    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        stepStartTime = Time.time;
        stepStartRotation = player.transform.rotation;
        //Removes the toaster and adds the plate with bread
        SetTable();
        mDel += Flip;
    }

    void SetTable()
    {
        toaster.SetActive(false);
        plateAndBread.SetActive(true);
    }
    void MoveList()
    {
        float step = (Time.time - stepStartTime) / flipDuration;
        listDisplay.transform.position = new Vector3(listDisplay.transform.position.x, (Screen.height * 0.8f) * step, 0f);
    }


    
   

    public void ExplosiveDeath() 
    {
        int childCount =  player.transform.FindChild("GatherLocation").childCount;
        List<Transform> childs = new List<Transform>();
        for (int i = 0; i < childCount; i++)
        {
            player.transform.FindChild("GatherLocation").GetChild(i).gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)));
            childs.Add(player.transform.FindChild("GatherLocation").GetChild(i));
        }
        foreach (Transform child in childs)
        {
            child.parent = null;
        }
        playerStatus = PlayerStatus.DEAD;
    }

 

    

    public void ChangeToOverhead() 
    {
        playerStatus = PlayerStatus.OVERHEAD;
        foreach (bool b in condimentCollected)
        {
            if (!b)
            {
                ExplosiveDeath();
                return;
            }
        }
    }


    void TiltControls() 
    {
        Vector3 dir = Vector3.zero;

        if (playerStatus == PlayerStatus.OVERHEAD)
        {
            dir.x = -Input.acceleration.x;
           // dir.z = -Input.acceleration.y;
        }
        else 
        {
            dir.x = Input.acceleration.x;
           // dir.z = Input.acceleration.y;
        }
        



        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime;
        rb.AddForce(dir * tiltMovespeed);
    }

    public void PepperBonus() 
    {
        rb.AddForce(Vector3.up * 1f);
        //transform.FindChild("Emmiter").gameObject.SetActive(true);
        //StartCoroutine(DisplayEmitter());
    }

    IEnumerator DisplayEmitter() 
    {
        yield return new WaitForSeconds(0.5f);
        transform.FindChild("Emmiter").gameObject.SetActive(false);
    }


    private int bonusMultiplier = 1;

    public void AddScore(int amount, bool multiplier, Toppings top, Condiments con) 
    {
        if (multiplier)
        {
            if (top != Toppings.COUNT)
            {
                for (int i = 0; i < LevelController.instance.selectedToppings.Count; i++)
                {
                    if (LevelController.instance.selectedToppings[i] == top)
                    {
                        toppingCollected[i] = true;
                        break;
                    }
                }
            }
            if (con != Condiments.COUNT)
            {
                for (int j = 0; j < LevelController.instance.selectedCondiments.Count; j++)
                {
                    if (LevelController.instance.selectedCondiments[j] == con)
                    {
                        condimentCollected[j] = true;
                        break;
                    }
                }
            }

            bonusMultiplier++;
            multiplierTracker.transform.FindChild("Multiplier").GetComponent<Text>().text = "X" + bonusMultiplier;
            multiplierTracker.gameObject.SetActive(true);
        }
        else 
        {
            bonusMultiplier = 1;
            multiplierTracker.gameObject.SetActive(false);
        }
        

       score += amount * bonusMultiplier;
       scoreTracker.text = score + " pts";
       CheckScoreAchievement();
    }

    public void MultiplyScore(float distance) 
    {

        if (distance > 10f)
        {
            scoreTracker.text = "You Missed the Plate!";
            //score = 0;
        }
        else
        {
            int accuracy = (100 - (int)(distance * 100f));

           //score *= accuracy;
           scoreTracker.text = score + " pts";
           CheckScoreAchievement();
        }

        //SetCamera();
    }

    void SetCamera()
    {
        picMenu.SetActive(true);
        GameUI.SetActive(false);
 
        playerPictureCam.depth = 6;
        playerPictureCam.gameObject.SetActive(true);
    }

    public void CheckScoreAchievement()
    {
        if (score >= 100)
        {
            GameCenterLoading.instance.UnlockAchievement("CgkI09G1lLUQEAIQCA");
            GameCenterLoading.instance.RevealAchievement("CgkI09G1lLUQEAIQCQ");
        }
        if (score >= 200)
        {
            GameCenterLoading.instance.UnlockAchievement("CgkI09G1lLUQEAIQCQ");
            GameCenterLoading.instance.RevealAchievement("CgkI09G1lLUQEAIQCg");
        }
        if (score >= 300)
        {
            GameCenterLoading.instance.UnlockAchievement("CgkI09G1lLUQEAIQCg");
            GameCenterLoading.instance.RevealAchievement("CgkI09G1lLUQEAIQCw");
        }
        if (score >= 400)
        {
            GameCenterLoading.instance.UnlockAchievement("CgkI09G1lLUQEAIQCw");
            GameCenterLoading.instance.RevealAchievement("CgkI09G1lLUQEAIQDA");
        }
        if (score >= 500)
        {
            GameCenterLoading.instance.UnlockAchievement("CgkI09G1lLUQEAIQDA");
        }
    }

}


