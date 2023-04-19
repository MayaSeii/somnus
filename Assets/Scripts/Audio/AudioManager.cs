using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using General;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
        
        public Dictionary<string, EventInstance> EventInstances { get; private set; }

        private void Awake()
        {
            Instance = this;

            EventInstances = new  Dictionary<string, EventInstance>();

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

        private void Update()
        {
            _masterBus.setVolume(MasterVolume);
            _musicBus.setVolume(MusicVolume);
            _soundBus.setVolume(SoundVolume);
            _ambienceBus.setVolume(AmbienceVolume);
            _interfaceBus.setVolume(InterfaceVolume);
        }

        public void InitialiseInMenu()
        {
            InitialiseAmbience("DeepHum", FMODEvents.Instance.DeepHum);
            InitialiseAmbience("MenuAmbience", FMODEvents.Instance.TitlePiano);
        }

        public void InitialiseInGame()
        {
            InitialiseAmbience("GameStart", FMODEvents.Instance.GameStart);
            InitialiseAmbience("RandomMusic", FMODEvents.Instance.RandomMusic);
            InitialiseAmbience("MenuBuzz", FMODEvents.Instance.MenuBuzz, true);
            InitialiseAmbience("Footsteps", FMODEvents.Instance.Footsteps, true);
        }

        private void InitialiseAmbience(string title, EventReference sound, bool stop = false)
        {
            var instance = CreateEventInstance(title, sound);
            RuntimeManager.AttachInstanceToGameObject(instance, GameManager.Instance.MainCamera.transform);
            if (!stop) instance.start();
        }
        
        public void InitialiseAmbience(string title, EventReference sound, Transform tf, bool stop = false)
        {
            var instance = CreateEventInstance(title, sound);
            RuntimeManager.AttachInstanceToGameObject(instance, tf);
            if (!stop) instance.start();
        }
        
        public void StopAmbience(string title, STOP_MODE stopMode = STOP_MODE.IMMEDIATE)
        {
            if (!EventInstances.ContainsKey(title)) return;
            
            EventInstances[title].stop(stopMode);
            EventInstances.Remove(title);
        }

        public static void ChangeParameter(EventInstance ei, string parameterName, float parameterValue)
        {
            ei.setParameterByName(parameterName, parameterValue);
        }

        public static void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        private EventInstance CreateEventInstance(string title, EventReference sound)
        {
            if (EventInstances.ContainsKey(title)) EventInstances.Remove(title);
            
            var eventInstance = RuntimeManager.CreateInstance(sound);
            EventInstances.Add(title, eventInstance);
            return eventInstance;
        }

        private void CleanUp()
        {
            foreach (var ei in EventInstances)
            {
                ei.Value.stop(STOP_MODE.IMMEDIATE);
                ei.Value.release();
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}
