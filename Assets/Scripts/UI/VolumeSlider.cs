using Audio;
using Settings;
using UnityEngine;

namespace UI
{
    public class VolumeSlider : MonoBehaviour
    {
        [field: SerializeField] public VolumeType VolumeType { get; private set; }

        public void OnSliderValueChanged(float v)
        {
            SettingsManager.ChangeVolume(VolumeType, v / 10f);
        }
    }
}
