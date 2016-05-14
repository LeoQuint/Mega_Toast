using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCTR : MonoBehaviour {

    public Sprite soundOn;
    public Sprite soundOff;

    public Button soundBtn;

    public GameObject menuUI;
    public GameObject gameUI;

    bool isSoundOn = false;

    AudioSource aud;

    bool movingUI = false;
    bool isToasty = true;

    public GameObject player;
    public GameObject plate;

    public Sprite toast;
    public Sprite bagel;

    public GameObject btn_Toast;

    void Awake() 
    {
        aud = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (movingUI)
        {
            menuUI.transform.Translate(0f,15f,0f);
        }

	}

    public void CharSelect() 
    {
        isToasty = !isToasty;
        if (isToasty)
        {
            player.GetComponent<MeshRenderer>().enabled = true;
            player.transform.FindChild("M_bagel").gameObject.SetActive(false);
            plate.transform.FindChild("M_bagel").GetComponent<MeshRenderer>().enabled = false;
            plate.transform.FindChild("M_toast").GetComponent<MeshRenderer>().enabled = true;
            btn_Toast.GetComponent<Image>().sprite = toast;
        }
        else
        {
            player.GetComponent<MeshRenderer>().enabled = false;
            player.transform.FindChild("M_bagel").gameObject.SetActive(true);
            plate.transform.FindChild("M_bagel").GetComponent<MeshRenderer>().enabled = true;
            plate.transform.FindChild("M_toast").GetComponent<MeshRenderer>().enabled = false;
            btn_Toast.GetComponent<Image>().sprite = bagel;
        }
    }

    public void StartGame() 
    {
        SoundToggle();
        LevelController.Instance.isPlaying = true;
        StartCoroutine(StopMoving());
        movingUI = true;
        
    }

    IEnumerator StopMoving() 
    {
        yield return new WaitForSeconds(2f);
        movingUI = false;
        menuUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void SoundToggle() 
    {
        isSoundOn = !isSoundOn;
        if(isSoundOn)
        {
            aud.Play();
            soundBtn.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            aud.Pause();
            soundBtn.GetComponent<Image>().sprite = soundOff;
        }
        
        
    }

}
