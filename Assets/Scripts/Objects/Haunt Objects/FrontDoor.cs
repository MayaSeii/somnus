using Audio;
using Objects;
using Haunts;
using System.Collections;
using UnityEngine;

public class FrontDoor : MonoBehaviour
{
    [field: SerializeField] public GameObject KnockingCalmSource;
    [field: SerializeField] public GameObject KnockingAngrySource;
    private bool _isKnocking;
    private float _timer;
    private float _countdown;

    private void Awake()
    {
        _countdown = 3f;
        KnockingCalmSource.SetActive(false);
        KnockingCalmSource.SetActive(false);
    }

    public void Knock()
    {
        _timer = HauntManager.Instance.HauntTimer;
        _isKnocking = true;
        HauntManager.Instance.HauntActive = true;
        KnockingCalmSource.SetActive(true);
    }
    public void StopKnocking()
    {
        KnockingCalmSource.SetActive(false);
        KnockingAngrySource.SetActive(true);
        _countdown -= Time.deltaTime;
        if (_countdown <= 0)
        {
            _isKnocking = false;
            HauntManager.Instance.HauntActive = false;
            KnockingAngrySource.SetActive(false);
        }
    }
    private void Update()
    {
        if (_isKnocking) _timer -= Time.deltaTime;
        if (_isKnocking && _timer <= 0 && HauntManager.Instance.MotherInstance == null)
        {
            _timer = HauntManager.Instance.HauntTimer;
            StopKnocking();
            HauntManager.Instance.ForceSpawnMother(default);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _isKnocking) StopKnocking();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _countdown = 3f;
            if (_isKnocking) KnockingCalmSource.SetActive(true);
        }
    }
}
