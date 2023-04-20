using Audio;
using Objects;
using UnityEngine;

public class WallPhone : Interactable
{
    [field: SerializeField] public Light Light { get; set; }
    private bool _isRinging;
    
    public override void Interact()
    {
        if (_isRinging) StopRinging();
    }

    public void Ring()
    {
        _isRinging = true;
        AudioManager.Instance.InitialiseAmbience("Phone Ring", FMODEvents.Instance.KitchenPhoneRing, transform);
        Light.enabled = true;
    }

    public void StopRinging()
    {
        _isRinging = false;
        AudioManager.Instance.StopAmbience("Phone Ring");
        AudioManager.PlayOneShot(FMODEvents.Instance.KitchenPhoneHangUp, transform.position);
        Light.enabled = false;
    }
}
