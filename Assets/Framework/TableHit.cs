﻿using UnityEngine;
using System.Collections;

public class TableHit : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            float offset = ((float)(other.gameObject.transform.FindChild("GatherLocation").childCount)) / 60f;
            other.transform.position = new Vector3(other.transform.position.x,  offset, other.transform.position.z);
            other.GetComponent<Player>().playerStatus = Status.LANDED;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY;

         

           // other.GetComponent<Player>().MultiplyScore(2f);
        }
    }
}
