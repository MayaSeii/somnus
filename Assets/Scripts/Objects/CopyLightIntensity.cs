using UnityEngine;
using VolumetricLights;

public class CopyLightIntensity : MonoBehaviour
{
    [field: SerializeField] public Light Light { get; private set; }
    private Light _thisLight;
    private VolumetricLight _volLight;

    private void Awake()
    {
        _thisLight = GetComponent<Light>();
        _volLight = GetComponent<VolumetricLight>();
    }

    private void Update()
    {
        var intensity = Light.intensity;
        
        _thisLight.intensity = intensity;
        _volLight.dustBrightness = intensity;
    }
}
