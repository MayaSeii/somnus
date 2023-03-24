using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private List<EventInstance> _eventInstances;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Found more than one Audio Manager in the scene.");
        Instance = this;

        _eventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        InitialiseAmbience(FMODEvents.Instance.LightHum);
        InitialiseAmbience(FMODEvents.Instance.DeepHum);
    }

    private void InitialiseAmbience(EventReference sound)
    {
        var instance = CreateEventInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(instance, GameObject.FindWithTag("Player").transform);
        instance.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference sound)
    {
        var eventInstance = RuntimeManager.CreateInstance(sound);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (var ei in _eventInstances)
        {
            ei.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ei.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
