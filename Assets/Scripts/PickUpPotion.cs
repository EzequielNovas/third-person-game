using UnityEngine;

public class PickUpPotion : MonoBehaviour
{
    public Potion potion;
    public AudioClip potionPick;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundController.instance.PlaySound(potionPick);
            potion.potions++;
            Destroy(gameObject);
        }
    }
}
