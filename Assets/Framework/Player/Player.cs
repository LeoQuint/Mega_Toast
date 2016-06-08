using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Status 
{
    INTRO,
    CHARGING,
    GOINGUP,
    GOINGDOWN,
    OVERHEAD,
    LANDED,
    MISSED
}

public class Player : MonoBehaviour {

    //newer CODE

    public GameObject player;

    //Delegate used to controls/movements.
    delegate void mDelegate();
    mDelegate mDel;

    //Player Stats. Holds the modifier and stats of a particular type of bread or player.
    public PlayerData pStats;

    [SerializeField]
    float flipDelay = 0.5f;
  
    public Status playerStatus;

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

    public Button replayBtn;

    public Image listDisplay;
    //Table
    public GameObject toaster;
    public GameObject plateAndBread;


    private List<bool> toppingCollected = new List<bool>();
    private List<bool> condimentCollected = new List<bool>();

    public bool enableTiltControls = false;

    bool hasStarted = false;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeModel("toast");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeModel("bagel");
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
                break;

            case "bagel":
                player.transform.FindChild("Mesh").GetComponent<MeshFilter>().mesh = meshes[1];
                player.transform.FindChild("Mesh").GetComponent<MeshRenderer>().material = materials[1];
                
                player.transform.FindChild("Mesh").rotation = new Quaternion(0f, 0f, -1f, -0.3f);
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

        playerStatus = Status.INTRO;
        rb.isKinematic = false;

        StartCoroutine(DelayBreadLoading());
    }

    IEnumerator DelayBreadLoading()
    {
        

        yield return new WaitForSeconds(2f);
        playerStatus = Status.CHARGING;
        powerBar.gameObject.SetActive(true);
        listDisplay.gameObject.SetActive(true);
    }


    public void Jump()
    {
        if (playerStatus == Status.CHARGING)
        {
            playerStatus = Status.GOINGUP;
            powerBar.GetComponent<PowerBar>().hasLaunched = true;
            StartCoroutine(DisplayPower());
            //speedTracker.gameObject.SetActive(true);
            //heightTracker.gameObject.SetActive(true);
            scoreTracker.gameObject.SetActive(true);
            float forceAdded = 1f * Mathf.Pow((0.5f), (1f - powerBar.value));

            GetComponent<Rigidbody>().AddForce(new Vector3(0f, forceAdded, 0f));
            stepEndRotation = upRotation;
            StartCoroutine(Wait(1f));
            launchBtn.gameObject.SetActive(false);
        }
    }
    void Flip()
    {

        float step = (Time.time - stepStartTime) / flipDuration;
        transform.rotation = Quaternion.Lerp(stepStartRotation, stepEndRotation, step);

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
        stepStartRotation = transform.rotation;
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
    /*


   

	// Update is called once per frame
	void Update () 
    {
        if (!LevelController.Instance.isPlaying)
        {
            return;
        }
        else if(!hasStarted)
        {
            rb.isKinematic = false;
            StartCoroutine(DelayToastLoading());
            hasStarted = true;
        }


        if (playerStatus == Status.LANDED || playerStatus == Status.DEAD)
        {
            replayBtn.gameObject.SetActive(true);
            return;
        }

        if (playerStatus == Status.INTRO)
        {
            return;
        }*/

    //speedTracker.text = rb.velocity.y.ToString("F1") + " Kmph";
    /*if (maxHeightAchived < transform.position.y)
    {
        maxHeightAchived = transform.position.y;
        heightTracker.text = "Max height: " + maxHeightAchived.ToString("F1") + " Meters";
    }*/
    /*
        if (mDel != null)
        {
            mDel();
        }

        if (transform.position.y < 1f)
        {
            playerStatus = Status.LANDED;
            return;
        }

        if (playerStatus == Status.CHARGING)
        {
            
            if (Input.GetAxis("Jump") > 0)
            {
                playerStatus = Status.GOINGUP;
                powerBar.GetComponent<PowerBar>().hasLaunched = true;
                StartCoroutine(DisplayPower());
                //speedTracker.gameObject.SetActive(true);
                //heightTracker.gameObject.SetActive(true);
                scoreTracker.gameObject.SetActive(true);
                float forceAdded = 1f * Mathf.Pow( (0.5f), (1f - powerBar.value) );

                GetComponent<Rigidbody>().AddForce(new Vector3(0f, forceAdded, 0f));
                stepEndRotation = upRotation;
                StartCoroutine(Wait(1f));
            }
            

        }
       

        if (playerStatus == Status.OVERHEAD)
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

        Vector3 clampedX = new Vector3(Mathf.Clamp(transform.position.x, -1.2f, 1.2f), transform.position.y, transform.position.z);
        transform.position = clampedX;

        if (rb.velocity.y < -2f && playerStatus == Status.GOINGUP)
        {
            playerStatus = Status.GOINGDOWN;

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

            LevelController.Instance.SpawnDownObjects();
            
            mDel += Flip;
            mDel += MoveList;
        }

        
       
	}


    

    public void ExplosiveDeath() 
    {
        int childCount =  transform.FindChild("GatherLocation").childCount;
        List<Transform> childs = new List<Transform>();
        for (int i = 0; i < childCount; i++)
        {
            transform.FindChild("GatherLocation").GetChild(i).gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)));
            childs.Add(transform.FindChild("GatherLocation").GetChild(i));
        }
        foreach (Transform child in childs)
        {
            child.parent = null;
        }
        playerStatus = Status.DEAD;
    }

 

    

    public void ChangeToOverhead() 
    {
        playerStatus = Status.OVERHEAD;
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

        if (playerStatus == Status.OVERHEAD)
        {
            dir.x = -Input.acceleration.x;
            dir.z = -Input.acceleration.y;
        }
        else 
        {
            dir.x = Input.acceleration.x;
            dir.z = Input.acceleration.y;
        }
        



        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(dir * tiltMovespeed);
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
                for (int i = 0; i < LevelController.Instance.selectedToppings.Length; i++)
                {
                    if (LevelController.Instance.selectedToppings[i] == top)
                    {
                        toppingCollected[i] = true;
                        break;
                    }
                }
            }
            if (con != Condiments.COUNT)
            {
                for (int j = 0; j < LevelController.Instance.selectedCondiments.Length; j++)
                {
                    if (LevelController.Instance.selectedCondiments[j] == con)
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
        

       // score += amount * bonusMultiplier;
       // scoreTracker.text = score + " pts";
    }

    public void MultiplyScore(float distance) 
    {
        if (distance > 10f)
        {
            scoreTracker.text = "You Missed the Plate!";
        }
        else
        {
            int accuracy = (100 - (int)(distance * 100f));

           // score *= accuracy;
           // scoreTracker.text = score + " pts";
        }
    }

    */

}
