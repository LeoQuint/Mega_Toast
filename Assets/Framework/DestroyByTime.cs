using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

    public float time = 0.5f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, time);
	}
	
	
}
