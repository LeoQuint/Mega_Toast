using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCTR : MonoBehaviour {

    public Sprite soundOn;
    public Sprite soundOff;

    public Button soundBtn;

    public GameObject mainMenuUI;
    public GameObject gameUI;
    public GameObject titleUI;
    public GameObject charSelectUI;

    bool isSoundOn = false;

    AudioSource aud;

    bool movingUI = false;
    bool isToasty = true;

    public GameObject player;
    public GameObject plate;

    public Sprite toast;
    public Sprite bagel;

    public GameObject btn_Toast;

    public GameObject mainBg;
    public GameObject mainScene;

    public Transform plateCenter;
    public string[] selectedChar;
    public GameObject[] selectedChar_prefab;
    private int currentCharacterIndex = 0;

    public Camera charSelectCam;
    private GameObject currentCharacter;
  

    void Awake() 
    {
        aud = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
        currentCharacter = Instantiate(selectedChar_prefab[currentCharacterIndex], plateCenter.position, Quaternion.identity) as GameObject;
        charSelectCam.depth = 0;
    }
	
	// Update is called once per frame
	void Update () {

       

	}

    public void ToggleCharSelect(bool t)
    {

        mainMenuUI.SetActive(!t);
        charSelectUI.SetActive(t);
        mainBg.SetActive(!t);
        mainScene.SetActive(t);        
    }
   

    //scroll up or down the list of characters.
    public void Scroll(int s)
    {
        if (currentCharacter)
        {
            Destroy(currentCharacter);
        }
        if (s == 1)
        {
            currentCharacterIndex++;
            if (currentCharacterIndex == selectedChar.Length)
            {
                currentCharacterIndex = 0;
            }
        }
        else if (s == -1)
        {
            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
            {
                currentCharacterIndex = selectedChar.Length - 1;
            }
        }
        currentCharacter = Instantiate(selectedChar_prefab[currentCharacterIndex], plateCenter.position, Quaternion.identity) as GameObject;
        CharSelect(selectedChar[currentCharacterIndex]);
    }

    public void CharSelect(string sel) 
    {
        player.GetComponent<Player>().ChangeModel(sel);
    }

    public void StartGame() 
    {

    }


    #region Settings
    //toggle sound on/off
    public void SoundToggle() 
    {

        
    }
    //Set the volume for soundfx or music in the settings menu.
    public void SetVolume(int type, float vol)
    {
        
    }

    #endregion

}
