using Audio;
using Objects;
using Haunts;
using System.Collections;
using UnityEngine;

public class FakeFather : MonoBehaviour
{
    [field: SerializeField] public GameObject father {get; set;}
    private bool _isActive;
    private float _timer;

    void Start()
    {
        _isActive = false;
        father.SetActive(_isActive);
    }
    void Update()
    {
        if (_isActive) _timer -= Time.deltaTime;
        if (_isActive && _timer <= 0 && HauntManager.Instance.FatherInstance == null)
        {
            _timer = HauntManager.Instance.HauntTimer;
            Deactivate();
            HauntManager.Instance.ForceSpawnFather(default);
        }
    }
    public void Activate()
    {
        _isActive = true;
        HauntManager.Instance.HauntActive = true;
        _timer = HauntManager.Instance.HauntTimer;
        father.SetActive(_isActive);
        AudioManager.PlayOneShot(FMODEvents.Instance.RandomMusic, transform.position);
    }
    public void Deactivate()
    {
        _isActive = false;
        HauntManager.Instance.HauntActive = false;
        _timer = HauntManager.Instance.HauntTimer;
        father.SetActive(_isActive);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Deactivate();
    }
}
