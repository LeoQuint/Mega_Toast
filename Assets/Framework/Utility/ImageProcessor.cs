using UnityEngine;
using System.Collections;

public class ImageProcessor : MonoBehaviour {

    WebCamTexture webcamTexture;

    float angle;
    Vector2 angleVec;
    
    public Texture targetTexture;
	// Use this for initialization
	void Start () 
    {
        webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        //targetTexture = webcamTexture;
        webcamTexture.Play();
        angle = (float)(webcamTexture.videoRotationAngle);

        gameObject.transform.Rotate(Vector3.forward, angle);
	}

    public void ResetCam() 
    {
        webcamTexture.Play();
    }

    public void TakePicture() 
    {
       Texture pic = new Texture();
        pic = webcamTexture;
        webcamTexture.Stop();

        SetImageToStorage(pic);
    }

    void SetImageToStorage(Texture tex) 
    {
        GameObject.FindGameObjectWithTag("ImageStorage").GetComponent<ImageStored>().SetTexture(tex, angle);
    }

 
   
}
