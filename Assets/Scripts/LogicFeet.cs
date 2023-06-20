using UnityEngine;

public class LogicFeet : MonoBehaviour
{
    public PlayerLogic playerLogic; 
    private void OnTriggerStay(Collider other) => playerLogic.Jump = true;
    private void OnTriggerExit(Collider other) => playerLogic.Jump = false;
}
