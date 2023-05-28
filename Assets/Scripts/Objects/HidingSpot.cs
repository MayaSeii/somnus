using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Controllers;
using General;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

public class HidingSpot : Interactable
{
    [field: SerializeField] public Transform HidingPosition { get; private set; }
    [field: SerializeField] public Transform HidingExit { get; private set; }

    private Animator _animator;
    private bool _isOpen;
    private string _closeDirection;
    private bool _isActive;

    private new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }
    
    public override void Interact()
    {
        if (!InteractionCircle.enabled) return;
        
        _isActive = !_isActive;
        StartCoroutine(_isActive ? EnterHidingSpot() : LeaveHidingSpot());
    }

    private IEnumerator EnterHidingSpot()
    {
        var player = GameManager.Instance.Player;
        
        player.Crouch(new InputAction.CallbackContext());
        
        _animator.Play("Stand Door Open", 0, 0);
        AudioManager.PlayOneShot(FMODEvents.Instance.StandDoorOpen, transform.position);
        
        yield return new WaitForSeconds(.25f);

        StartCoroutine(CloseDoorAfterHiding());

        while (player.transform.rotation != HidingPosition.rotation)
        {
            player.ForceMovement(Vector3.Lerp(player.transform.position, HidingPosition.position, .2f));
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, HidingPosition.rotation, .2f);
            player.CameraTransform.eulerAngles = Quaternion.identity.eulerAngles;
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator LeaveHidingSpot()
    {
        var player = GameManager.Instance.Player;

        _animator.Play("Stand Door Open", 0, 0);
        AudioManager.PlayOneShot(FMODEvents.Instance.StandDoorClose, transform.position);

        yield return new WaitForSeconds(.25f);

        while (player.transform.rotation != Quaternion.identity)
        {
            player.ForceMovement(Vector3.Lerp(player.transform.position, HidingExit.position, .2f));
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, HidingExit.rotation, .2f);
            player.CameraTransform.eulerAngles = Quaternion.identity.eulerAngles;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.25f);

        player.PrepareStand(new InputAction.CallbackContext());
        StartCoroutine(CloseDoorAfterHiding());

        _isActive = false;
    }

    private IEnumerator CloseDoorAfterHiding()
    {
        yield return new WaitForSeconds(.15f);
        
        _animator.Play("Stand Door Close", 0, 0);
        AudioManager.PlayOneShot(FMODEvents.Instance.StandDoorClose, transform.position);
    }
}
