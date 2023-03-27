using Audio;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "ClockChimeHaunt", menuName = "Haunts/Clock Chime")]
    public class ClockChimeHaunt : Haunt
    {
        private Vector3 _clock;
        
        public override void Execute()
        {
            _clock = GameObject.FindWithTag("Clock").transform.position;
            AudioManager.PlayOneShot(FMODEvents.Instance.ClockChime, _clock);
        }
    }
}
