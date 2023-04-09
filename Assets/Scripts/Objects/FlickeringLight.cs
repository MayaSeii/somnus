using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
   private Light _light;
   
   private float _minIntensity;
   private float _maxIntensity;
   private int _smoothing;
   
   private Queue<float> _smoothQueue;
   private float _lastSum;

   private void Awake()
   {
      _light = GetComponent<Light>();
      _minIntensity = 0.5f;
      _maxIntensity = 1f;
      _smoothing = 5;
   }
   
   public void Reset()
   {
      _smoothQueue.Clear();
      _lastSum = 0;
   }

   private void Start()
   {
      _smoothQueue = new Queue<float>(_smoothing);
      if (_light == null) _light = GetComponent<Light>();
   }

   private void Update()
   {
      if (!_light) return;
   
      while (_smoothQueue.Count >= _smoothing) _lastSum -= _smoothQueue.Dequeue();
      
      var newVal = Random.Range(_minIntensity, _maxIntensity);
      _smoothQueue.Enqueue(newVal);
      _lastSum += newVal;
      
      _light.intensity = _lastSum / _smoothQueue.Count;
   }
}
