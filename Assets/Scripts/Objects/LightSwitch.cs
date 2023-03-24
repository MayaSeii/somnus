using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class LightSwitch : Interactable
{
    private List<Light> _associatedLights;
    private bool _state;
    
    private void Start()
    {
        _associatedLights = transform.parent.GetComponent<RoomController>().CeilingLamps;
        _state = true;
    }

    public override void Interact()
    {
        _state = !_state;
        _associatedLights.ForEach(l => l.gameObject.SetActive(_state));
        transform.GetChild(1).rotation = Quaternion.Euler(_state ? -90 : 90, 0, 90);
    }
}
