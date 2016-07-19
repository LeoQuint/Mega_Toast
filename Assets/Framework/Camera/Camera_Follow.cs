using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_Follow : MonoBehaviour {

    public Transform target;
    public float delayFinalPhase = 1.5f;


    Rigidbody playerRb;

    Quaternion startingRot = new Quaternion(0f,0f,0f,1f);
    Quaternion overHeadRot = new Quaternion(0f,0.7f,-0.7f,0f);
    Vector3 overHeadPos = new Vector3(0f, 0f, 4f);

    Vector3 endPos;

    public float offset = 5f;
    private float camOffset;
    

    float stepStartTime;
    float flipDuration = 0.25f;

    delegate void mDelegate();
    mDelegate mDel;


    public bool overHeadFollow = true;
    bool asFliped = false;

    //public references:
    public Player pScript;

    private Vector3 startingPos;
    private Quaternion initialRotation;

    void Start() 
    {
        Debug.Log(transform.position);

        startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        initialRotation = transform.rotation;
        playerRb = target.GetComponent<Rigidbody>();
        camOffset = offset;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        if (pScript.playerStatus == PlayerStatus.LANDED || pScript.playerStatus == PlayerStatus.DEAD)
        {
            return;
        }
        if (mDel != null)
        {
            mDel();
        }
        if (target != null)
        {
            if (pScript.playerStatus == PlayerStatus.GOINGDOWN && transform.position.y < 15f)
            {

                if (!asFliped)
                {
                   
                    asFliped = true;
                    mDel += Flip;
                    
                    stepStartTime = Time.time;
                    endPos = transform.position + overHeadPos;
                 
                    playerRb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }

                return;
            }
            else if (pScript.playerStatus == PlayerStatus.OVERHEAD)
            {
               
                return;
            }


            
            if (playerRb.velocity.y > 0f)
            {
                camOffset = offset;
            }
            else
            {
                camOffset = -offset;
            }



            Vector3 adjustedPos = new Vector3(0f, transform.position.y, -12f);
            Vector3 adjustedPosTarget = new Vector3(0f, target.position.y + camOffset, -12f);
            transform.position = Vector3.Lerp(adjustedPos, adjustedPosTarget, Mathf.Abs( playerRb.velocity.y) * Time.deltaTime);
            
        }
        
	}

    void Flip()
    {
        
        float step = (Time.time - stepStartTime) / flipDuration;
        transform.rotation = Quaternion.Lerp(startingRot, overHeadRot, step);

        transform.position = Vector3.Lerp(transform.position, endPos, step);


        if (step >= 1f)
        {
            mDel -= Flip;
            if (overHeadFollow)
            {
                mDel += OverHeadFollow;
                
                StartCoroutine(FinalPhaseDelay());
            }
        }
    }
    IEnumerator FinalPhaseDelay()
    {
        yield return new WaitForSeconds(0.6f);
        Time.timeScale = 0f;
        StartCoroutine(FinalViewDelay());
    }
    IEnumerator FinalViewDelay()
    {
        yield return new WaitForSecondsRealtime(delayFinalPhase);
        Time.timeScale = 1f;
    }

    void OverHeadFollow() 
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0f,3f, 1.1f), Mathf.Abs(playerRb.velocity.y) * Time.deltaTime);
    }
    public void ResetValues()
    {
        asFliped = false;
        transform.rotation = initialRotation;
        transform.position = startingPos;
        camOffset = offset;
    }

}
