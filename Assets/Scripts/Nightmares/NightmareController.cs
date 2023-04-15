using System;
using Objects;
using UnityEngine;

namespace Nightmares
{
    public class NightmareController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Room Area"))
            {
                other.transform.parent.GetComponentInChildren<LightSwitch>().ForceOff();
            }
        }
    }
}
