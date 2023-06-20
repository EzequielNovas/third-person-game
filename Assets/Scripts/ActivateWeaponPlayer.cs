using UnityEngine;

public class ActivateWeaponPlayer : MonoBehaviour
{
    public PickUpWeapon pickUpWeapon;
    public int weapon;
    private void Start() => pickUpWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<PickUpWeapon>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pickUpWeapon.ActivateWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
