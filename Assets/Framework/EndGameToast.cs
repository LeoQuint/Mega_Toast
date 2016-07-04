using UnityEngine;
using System.Collections;

public class EndGameToast : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
            float offset = ((float)(other.gameObject.transform.FindChild("GatherLocation").childCount)) / 60f;
            other.transform.position = new Vector3(other.transform.position.x, 0.7f + offset, other.transform.position.z);
            Player.instance.playerStatus = PlayerStatus.LANDED;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY;

            float distance = Mathf.Abs(transform.position.x - other.transform.position.x);

            Player.instance.MultiplyScore(distance);
            GameCenterLoading.instance.UnlockAchievement("CgkI09G1lLUQEAIQAg");
        }

       
    }
}
