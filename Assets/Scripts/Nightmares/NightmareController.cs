using System;
using System.Collections;
using Audio;
using General;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Nightmares
{
    public class NightmareController : MonoBehaviour
    {
        [field: SerializeField] public NightmareType Type { get; set; }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Room Area"))
            {
                other.transform.parent.GetComponentInChildren<LightSwitch>().ForceOff();
            }
        }

        private void Update()
        {
            var parameter = Type == NightmareType.Father ? "distanceToFather" : "distanceToMother";

            var distance = Mathf.Clamp(Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position), 0, 20f);
            
            AudioManager.ChangeParameter(AudioManager.Instance.EventInstances["Father Music"], parameter, distance);
        }

        public void Jumpscare()
        {
            AudioManager.PlayOneShot(FMODEvents.Instance.Jumpscare, transform.position);
            AudioManager.Instance.StopAmbience("Father Music");

            GetComponent<BaseStateMachine>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            
            GameManager.Instance.MainCamera.transform.position = transform.GetChild(1).position + transform.GetChild(1).forward * .5f;
            GameManager.Instance.MainCamera.transform.LookAt(transform.GetChild(1));

            StartCoroutine(UIManager.FadeOut());
            StartCoroutine(Shake());
        }
        
        private static IEnumerator Shake()
        {
            var elapsed = 0.0f;
            
            var cam = GameManager.Instance.MainCamera.transform;
            var originalCamPos = cam.position;
            
            var percentComplete = elapsed / 5f;
            var damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
            
            while (elapsed < 5f)
            {
                elapsed += Time.deltaTime;
                cam.position = originalCamPos + Random.insideUnitSphere * (.02f * damper);
                yield return null;
            }
        
            cam.position = originalCamPos;
        }
    }
}
