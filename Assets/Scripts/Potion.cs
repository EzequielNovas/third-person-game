using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public int potions;
    public GameObject UsePotion;
    public Text Text;
  
     void Start()
    {
        potions = 0;
        UsePotion.SetActive(false);
    }
     void Update()
    {
        Text.text = "potions: " + potions;
        if (FindObjectOfType<PlayerLogic>().HP == 100 || potions == 0)
        {
            UsePotion.SetActive(false);
        }
        else if (FindObjectOfType<PlayerLogic>().HP < 100 && potions >= 1)
        {
            UsePotion.SetActive(true);
        }
    }
    public void UsePotions()
    {
        potions--;
        FindObjectOfType<PlayerLogic>().HP += 50;
        if (FindObjectOfType<PlayerLogic>().HP >= 100)
            FindObjectOfType<PlayerLogic>().HP = 100;
    }
}
