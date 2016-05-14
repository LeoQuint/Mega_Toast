using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Status 
{
    RESTING,
    CHARGING,
    GOINGUP,
    GOINGDOWN,
    OVERHEAD,
    LANDED,
    DEAD
}

public class Player : MonoBehaviour {


    [System.NonSerialized]
    public int score;

    public float jumpForce = 10000f;
    public float pepperBonusForce = 1000f;

    public Status playerStatus;

    public float flipDelay = 1f;

    delegate void mDelegate();
    mDelegate mDel;

    Quaternion upRotation = new Quaternion(0f,0f,0f,1f);
    Quaternion downRotation = new Quaternion(-1f,0f,0f,0f);
    Quaternion normalRotation = new Quaternion(-0.7f, 0f,0f,0.7f);

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

    

    float maxHeightAchived;

    GameTouch gameTouch = new GameTouch();

    private List<bool> toppingCollected = new List<bool>();
    private List<bool> condimentCollected = new List<bool>();

    public bool enableTiltControls = true;

    bool hasStarted = false;

	// Use this for initialization
	void Start () 
    {
        score = 0;
        maxHeightAchived = 0f;
        playerStatus = Status.RESTING;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        for (int i = 0; i < LevelController.Instance.selectedToppings.Length; i++)
        {
            toppingCollected.Add(false);
        }
        for(int i = 0; i < LevelController.Instance.selectedCondiments.Length; i++)
        {
            condimentCollected.Add(false);
        }

        
	}

    IEnumerator DelayToastLoading() 
    {
        yield return new WaitForSeconds(2f);
        playerStatus = Status.CHARGING;
        powerBar.gameObject.SetActive(true);
        listDisplay.gameObject.SetActive(true);
    }

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

        if (playerStatus == Status.RESTING)
        {
            return;
        }

        speedTracker.text = rb.velocity.y.ToString("F1") + " Kmph";
        /*if (maxHeightAchived < transform.position.y)
        {
            maxHeightAchived = transform.position.y;
            heightTracker.text = "Max height: " + maxHeightAchived.ToString("F1") + " Meters";
        }*/

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
                float forceAdded = jumpForce * Mathf.Pow( (0.5f), (1f - powerBar.value) );

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
            else
            {
                SwipeCtr(true);
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
            else
            {
                SwipeCtr(true);
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


    IEnumerator Wait(float delay) 
    {
        yield return new WaitForSeconds(delay);
        stepStartTime = Time.time;
        stepStartRotation = transform.rotation;
        //Removes the toaster and adds the plate with bread
        SetTable();
        mDel += Flip;
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

    void MoveList() 
    {
        float step = (Time.time - stepStartTime) / flipDuration;
        listDisplay.transform.position = new Vector3(listDisplay.transform.position.x, (Screen.height * 0.8f) * step, 0f);
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
            float forceAdded = jumpForce * Mathf.Pow((0.5f), (1f - powerBar.value));
            
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, forceAdded, 0f));
            stepEndRotation = upRotation;
            StartCoroutine(Wait(1f));
            launchBtn.gameObject.SetActive(false);
        }
    }

    IEnumerator DisplayPower() 
    {
        yield return new WaitForSeconds(2f);
        powerBar.gameObject.SetActive(false);
    }

    void SwipeCtr(bool isOverHead) 
    {


        if (Input.touchCount > 0)
        {

            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {

                case TouchPhase.Began:
              
                    gameTouch.SetStartTouch(Time.time, touch.position);

                    break;


                case TouchPhase.Ended:
                    
                    gameTouch.SetEndTouch(Time.time, touch.position);
                    
                    break;
            }
            if (isOverHead)
            {
                movementY = gameTouch.TouchVector().y/40f;
                rb.AddForce(Vector3.forward * movementY * movementSpeed * -1f);
                movement = gameTouch.TouchVector().x/40f;
                rb.AddForce(Vector3.right * movement * movementSpeed * -1f);
            }
            else 
            {
                movement = gameTouch.TouchVector().x/40f;
                rb.AddForce(Vector3.right * movement * movementSpeed);
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
        
 
    

    void SetTable() 
    {
        toaster.SetActive(false);
        plateAndBread.SetActive(true);
    }

    public void PepperBonus() 
    {
        rb.AddForce(Vector3.up * pepperBonusForce);
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
        

        score += amount * bonusMultiplier;
        scoreTracker.text = score + " pts";
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

            score *= accuracy;
            scoreTracker.text = score + " pts";
        }
    }

    

}


public class GameTouch
{
    float start;
    float end;

    Vector2 startPosition;
    Vector2 endPosition;


    public void SetStartTouch(float phaseStart, Vector2 phaseStartPosition)
    {
        start = phaseStart;
        startPosition = phaseStartPosition;
       
    }

    public void SetEndTouch(float phaseEnd, Vector2 phaseEndPosition)
    {
        end = phaseEnd;
        endPosition = phaseEndPosition;
    }

    public Vector2 TouchVector()
    {
        return endPosition - startPosition;
    }

    public string Direction()
    {
        Vector2 direction = TouchVector();

        float angle = 0;

        if (direction.y < 0)
        {
            angle = 360 - (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg * -1);
        }
        else
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }


        if (angle < 45 || angle > 315)
        {
            //right
            return "Right";
        }
        else if (angle < 135)
        {
            //Up
            return "Up";
        }
        else if (angle <= 225)
        {
            //Left
            return "Left";
        }
        else if (angle <= 315)
        {
            //Down
            return "Down";
        }
        else
        {
            //error
            Debug.LogError("Error in angle calculation!");
            return "Error";
        }
    }

    public float TouchDuration()
    {
        return end - start;
    }

    public GameTouch()
    {

    }


}