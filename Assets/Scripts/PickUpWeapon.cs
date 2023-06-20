using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public GameObject[] weapon;
    public PlayerLogic playerLogic;

    public void ActivateWeapon(int number)
    {
        for(int i = 0; i < weapon.Length; i++)
        {
            weapon[i].SetActive(false);
        }

        weapon[number].SetActive(true);

        playerLogic.sword = true;
    }
}
