using Audio;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public void PlaySound()
    {
        AudioManager.PlayOneShot(FMODEvents.Instance.SettingsButton, transform.position);
    }
}
