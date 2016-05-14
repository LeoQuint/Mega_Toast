using UnityEngine;
using System.Collections;

public class Foods : MonoBehaviour {

    public bool pepper;
    public Toppings typeTopping;
    public Condiments typeCondiment;

    public int pointValue = 10;

    void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Player")
        {
            Destroy(GetComponent<RotationScript>());
            Destroy(GetComponent<BoxCollider>());
            gameObject.transform.SetParent(other.gameObject.transform.FindChild("GatherLocation"));
            gameObject.transform.rotation = transform.parent.rotation;
            float offset = ((float)(other.gameObject.transform.FindChild("GatherLocation").childCount)) / 600f;
            
            gameObject.transform.localPosition = new Vector3(0f, offset, 0f);

            if (typeTopping == Toppings.Egg)
            {
                gameObject.transform.localRotation = Quaternion.Euler(180f, Random.Range(0f, 355f), 0f);
            }
            else 
            {
                gameObject.transform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 355f));
            }

            Vector3 center = other.gameObject.GetComponent<BoxCollider>().center;
            Vector3 colliderSize = other.gameObject.GetComponent<BoxCollider>().size;
            other.gameObject.GetComponent<BoxCollider>().center = new Vector3(center.x, center.y + 0.001f, center.z);
            other.gameObject.GetComponent<BoxCollider>().size = new Vector3(colliderSize.x, colliderSize.y + 0.001f, colliderSize.z);
            
        }
        if (pepper &&  other.GetComponent<Player>().playerStatus == Status.GOINGUP)
        {
            other.GetComponent<Player>().PepperBonus();
        }

        bool correctTopping = false;
        
        for (int i = 0; i < LevelController.Instance.selectedToppings.Length; i++)
        {
            if (LevelController.Instance.selectedToppings[i] == typeTopping)
            {
                correctTopping = true;
                LevelController.Instance.SetCheckMarks(i);
            }
        }
        for (int j = 0; j < LevelController.Instance.selectedCondiments.Length; j++)
        {
            if (LevelController.Instance.selectedCondiments[j] == typeCondiment)
            {
                correctTopping = true;
                LevelController.Instance.SetCheckMarks(j);
            }
        }
        if (correctTopping)
        {
            other.GetComponent<Player>().AddScore(pointValue, true, typeTopping, typeCondiment);
        }
        else 
        {
            other.GetComponent<Player>().AddScore(pointValue, false, typeTopping, typeCondiment);
        }


    }


    

}
