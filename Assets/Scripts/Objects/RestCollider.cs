using Controllers;
using Haunts;
using UnityEngine;

public class RestCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !HauntManager.Instance.HauntActive) PlayerController.Instance.CanBlink = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) PlayerController.Instance.CanBlink = false;
    }
}
