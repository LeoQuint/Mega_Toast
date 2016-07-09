using UnityEngine;
using System.Collections;
using NS_Level;
public class Foods : MonoBehaviour {

    public bool pepper;
    public Toppings typeTopping;
    public Condiments typeCondiment;

    public int pointValue = 1;
    
    void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Player")
        {
            
            Destroy(GetComponent<RotationScript>());
            Destroy(GetComponent<BoxCollider>());


            if (typeCondiment == Condiments.COUNT)
            {
                gameObject.transform.SetParent(other.gameObject.transform.FindChild("GatherLocation"));
                gameObject.transform.rotation = transform.parent.rotation;
                int childCount = other.gameObject.transform.FindChild("GatherLocation").childCount;
                float offset = ((float)(childCount)) / 600f;

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

                Player.instance.CheckToppingAchievement(childCount);
            }
            else
            {
                Destroy(gameObject);
            }
        
            if (pepper &&  Player.instance.playerStatus == PlayerStatus.GOINGUP)
            {
                Player.instance.PepperBonus();
            }

            bool correctTopping = false;
        
            for (int i = 0; i < LevelController.instance.selectedToppings.Count; i++)
            {
                if (LevelController.instance.selectedToppings[i] == typeTopping)
                {
                    correctTopping = true;
                    LevelController.instance.SetCheckMarks(i+1);
                }
            }
            for (int j = 0; j < LevelController.instance.selectedCondiments.Count; j++)
            {
                if (LevelController.instance.selectedCondiments[j] == typeCondiment)
                {
                    correctTopping = true;
                    LevelController.instance.SetCheckMarks(j+1);
                }
            }
            if (correctTopping)
            {
               Player.instance.AddScore(pointValue, true, typeTopping, typeCondiment);
            }
            else 
            {
                Player.instance.AddScore(pointValue, false, typeTopping, typeCondiment);
            }
        }


    }

    
    

}
