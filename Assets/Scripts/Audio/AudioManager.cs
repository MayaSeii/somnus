using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using General;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }
        public float AmbienceVolume { get; set; }
        public float InterfaceVolume { get; set; }

        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _soundBus;
        private Bus _ambienceBus;
        private Bus _interfaceBus;

        public EventInstance RandomMusicInstance { get; private set; }
        private List<EventInstance> _eventInstances;

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Audio Manager in the scene.");
            Instance = this;

            _eventInstances = new List<EventInstance>();

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _soundBus = RuntimeManager.GetBus("bus:/SFX");
            _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
            _interfaceBus = RuntimeManager.GetBus("bus:/Interface");

            MasterVolume = .9f;
            MusicVolume = .9f;
            SoundVolume = .9f;
            AmbienceVolume = .9f;
            InterfaceVolume = .9f;
        }

        private void Start()
        {
            //InitialiseAmbience(FMODEvents.Instance.LightHum);
            InitialiseAmbience(FMODEvents.Instance.DeepHum);
            
            InitialiseAmbience(FMODEvents.Instance.GameStart);
            RandomMusicInstance = InitialiseAmbience(FMODEvents.Instance.RandomMusic);
        }

        private void Update()
        {
            _masterBus.setVolume(MasterVolume);
            _musicBus.setVolume(MusicVolume);
            _soundBus.setVolume(SoundVolume);
            _ambienceBus.setVolume(AmbienceVolume);
            _interfaceBus.setVolume(InterfaceVolume);
        }

        private EventInstance InitialiseAmbience(EventReference sound)
        {
            var instance = CreateEventInstance(sound);
            RuntimeManager.AttachInstanceToGameObject(instance, GameManager.Instance.Player.transform);
            instance.start();
            
            return instance;
        }

        public static void ChangeParameter(EventInstance ei, string parameterName, float parameterValue)
        {
            ei.setParameterByName(parameterName, parameterValue);
        }

        public static void PlayOneShot(EventReference sound, Vector3 worldPos)
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
}
