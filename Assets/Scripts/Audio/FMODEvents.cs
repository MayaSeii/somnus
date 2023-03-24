using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set; }
    
    [field: Header("Player")]
    [field: SerializeField] public EventReference Footsteps { get; private set; }
    
    [field: Header("Light Switch")]
    [field: SerializeField] public EventReference LightSwitchOn { get; private set; }
    [field: SerializeField] public EventReference LightSwitchOff { get; private set; }
    
    [field: Header("Door")]
    [field: SerializeField] public EventReference DoorOpen { get; private set; }
    [field: SerializeField] public EventReference DoorClose { get; private set; }

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Found more than one FMOD Events instance in the scene.");
        Instance = this;
    }
}
