using UnityEngine;
using System.Collections;

public class ImageStored : MonoBehaviour {

    public static ImageStored Instance { get; private set; }


    public Texture defaultTexture;
    private Texture puzzleTexture;
    private float camAngle;


    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        puzzleTexture = defaultTexture;
        DontDestroyOnLoad(gameObject);
    }

    public void SetTexture(Texture tex, float angle) 
    {
        puzzleTexture = tex;
        camAngle = angle;
    }

    public Texture GetTexture() 
    {
        return puzzleTexture;
    }

    public float GetRotation() 
    {
        return camAngle;
    }


}


