using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class AudioOcclusion : MonoBehaviour
    {
        #region - VAR Audio -
        
        [field: Header("Audio"), SerializeField] public EventReference EventRef { get; set; }
        
        private EventInstance _eventInstance;
        private StudioListener _listener;
        private float _maxDistance;
        
        #endregion
        
        #region - VAR Occlusion -
        
        [field: Header("Occlusion")]
        [field: SerializeField, Range(0f, 10f)] public float SoundOcclusionWidening { get; set; }
        [field: SerializeField, Range(0f, 10f)] public float PlayerOcclusionWidening { get; set; }
        [field: SerializeField] public LayerMask OcclusionLayerMask { get; set; }
        
        private int _hitCount;
        
        #endregion
        
        #region - UNITY Start -

        private void Start()
        {
            _eventInstance = RuntimeManager.CreateInstance(EventRef);
            RuntimeManager.AttachInstanceToGameObject(_eventInstance, transform, GetComponent<Rigidbody>());
            _eventInstance.start();
            _eventInstance.release();
            
            RuntimeManager.GetEventDescription(EventRef).getMinMaxDistance(out _, out _maxDistance);
            _listener = FindObjectOfType<StudioListener>();
        }
        
        #endregion
        
        #region - UNITY Updates -

        private void FixedUpdate()
        {
            var distance = Vector3.Distance(transform.position, _listener.transform.position);
            if (distance <= _maxDistance) OccludeBetween(transform.position, _listener.transform.position);
            _hitCount = 0;
        }
        
        #endregion
        
        #region - Occlusion -

        private void OccludeBetween(Vector3 sound, Vector3 listener)
        {
            var soundLeft = CalculatePoint(sound, listener, SoundOcclusionWidening, true);
            var soundRight = CalculatePoint(sound, listener, SoundOcclusionWidening, false);

            var soundAbove = new Vector3(sound.x, sound.y + SoundOcclusionWidening, sound.z);
            var soundBelow = new Vector3(sound.x, sound.y - SoundOcclusionWidening, sound.z);
            
            var listenerLeft = CalculatePoint(listener, sound, PlayerOcclusionWidening, true);
            var listenerRight = CalculatePoint(listener, sound, PlayerOcclusionWidening, false);

            var listenerAbove = new Vector3(listener.x, listener.y + PlayerOcclusionWidening * .5f, listener.z);
            var listenerBelow = new Vector3(listener.x, listener.y - PlayerOcclusionWidening * .5f, listener.z);

            CastRay(soundLeft, listenerLeft);
            CastRay(soundLeft, listener);
            CastRay(soundLeft, listenerRight);
            
            CastRay(sound, listenerLeft);
            CastRay(sound, listener);
            CastRay(sound, listenerRight);
            
            CastRay(soundRight, listenerLeft);
            CastRay(soundRight, listener);
            CastRay(soundRight, listenerRight);
            
            CastRay(soundAbove, listenerAbove);
            CastRay(soundBelow, listenerBelow);
            
            SetParameter();
        }

        private void CastRay(Vector3 start, Vector3 end)
        {
            Physics.Linecast(start, end, out var hit, OcclusionLayerMask);
            if (hit.collider) _hitCount++;
        }

        private void SetParameter()
        {
            _eventInstance.setParameterByName("occlusion", _hitCount / 11f);
        }
        
        #endregion
        
        #region - Helper Methods -

        private static Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool negate)
        {
            float x, z;
            var n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
            var mn = m / n;

            if (negate)
            {
                x = a.x + mn * (a.z - b.z);
                z = a.z - mn * (a.x - b.x);
            }
            else
            {
                x = a.x - mn * (a.z - b.z);
                z = a.z + mn * (a.x - b.x);
            }

            return new Vector3(x, a.y, z);
        }
        
        #endregion
    }
}
