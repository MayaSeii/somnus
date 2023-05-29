using Audio;
using Objects;
using Haunts;
using System.Collections;
using UnityEngine;

public class WallPhone : Interactable
{
    [field: SerializeField] public Light Light { get; set; }
    private bool _isRinging;
    private float _timer;
    
    public override void Interact()
    {
        if (_isRinging) StopRinging();
    }

    public void Ring()
    {
        _timer = HauntManager.Instance.HauntTimer;
        _isRinging = true;
        HauntManager.Instance.HauntActive = true;
        AudioManager.Instance.InitialiseAmbience("Phone Ring", FMODEvents.Instance.KitchenPhoneRing, transform);
        Light.enabled = true;
    }

    public void StopRinging()
    {
        _isRinging = false;
        HauntManager.Instance.HauntActive = false;
        AudioManager.Instance.StopAmbience("Phone Ring");
        AudioManager.PlayOneShot(FMODEvents.Instance.KitchenPhoneHangUp, transform.position);
        Light.enabled = false;
    }
    private void FixedUpdate()
    {
        if (_isRinging) _timer -= Time.deltaTime;
        if (_isRinging && _timer <= 0 && HauntManager.Instance.MotherInstance == null)
        {
            _timer = HauntManager.Instance.HauntTimer;
            StopRinging();
            HauntManager.Instance.ForceSpawnMother(default);
        }
    }
}
