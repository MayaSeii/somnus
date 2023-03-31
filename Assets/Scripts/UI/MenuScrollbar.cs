using Audio;
using UnityEngine;
using UnityEngine.UI;

public class MenuScrollbar : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Scrollbar>().value = 1;
    }

    public void PlaySound()
    {
        AudioManager.PlayOneShot(FMODEvents.Instance.Scrollbar, transform.position);
    }
}
