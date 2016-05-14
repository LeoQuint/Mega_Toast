using UnityEngine;
using System.Collections;

public class Camera_Follow : MonoBehaviour {

    public Transform target;
    
    Rigidbody playerRb;

    Quaternion startingRot = new Quaternion(0f,0f,0f,1f);
    Quaternion overHeadRot = new Quaternion(0f,0.7f,-0.7f,0f);
    Vector3 overHeadPos = new Vector3(0f, 0f, 4f);

    Vector3 endPos;

    public float offset = 5f;
    private float camOffset;
    Player pScript;

    float stepStartTime;
    float flipDuration = 0.5f;

    delegate void mDelegate();
    mDelegate mDel;


    public bool overHeadFollow = true;
    bool asFliped = false;

    void Start() 
    {
        playerRb = target.GetComponent<Rigidbody>();
        pScript = target.GetComponent<Player>();
        camOffset = offset;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (!LevelController.Instance.isPlaying)
        {
            return;
        }
        if (pScript.playerStatus == Status.LANDED || pScript.playerStatus == Status.DEAD)
        {
            return;
        }
        if (mDel != null)
        {
            mDel();
        }
        if (target != null)
        {
            if (pScript.playerStatus == Status.GOINGDOWN && transform.position.y < 15f)
            {

                if (!asFliped)
                {
                    asFliped = true;
                    mDel += Flip;
                    stepStartTime = Time.time;
                    endPos = transform.position + overHeadPos;
                    pScript.ChangeToOverhead();
                    playerRb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }

                return;
            }
            else if (pScript.playerStatus == Status.OVERHEAD)
            {
                return;
            }



            if (playerRb.velocity.y > 0f)
            {
                camOffset = offset;
            }
            else
            {
                camOffset = offset * -1f;
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
            }
        }
    }
    void OverHeadFollow() 
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0f,3f,-0.5f), Mathf.Abs(playerRb.velocity.y) * Time.deltaTime);
    }

}
