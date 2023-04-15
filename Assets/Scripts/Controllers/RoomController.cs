using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace Controllers
{
    public class RoomController : MonoBehaviour
    {
        private static List<RoomController> _instances;
        
        [field: SerializeField] public string RoomName { get; private set; }
        [field: SerializeField] public List<Collider> Areas { get; private set; }
        [field: SerializeField] public List<Light> CeilingLamps { get; private set; }
        
        public float PresenceTimer { get; private set; }

        private bool _playerInRoom;

        private void Awake()
        {
            _instances ??= new List<RoomController>();
            _instances.Add(this);
        }

        private void Update()
        {
            if (_playerInRoom)
            {
                PresenceTimer = Mathf.Min(PresenceTimer + Time.deltaTime, 200f);
                DebugManager.Instance.UpdateCurrentRoom($"{RoomName} ({PresenceTimer:0.00})");
            }
            else PresenceTimer = Mathf.Max(PresenceTimer - Time.deltaTime / 1.5f, 0f);
        }

        public void EnterRoom()
        {
            _instances.ForEach(i => i.ExitRoom());
            _playerInRoom = true;
        }

        private void ExitRoom()
        {
            _playerInRoom = false;
        }
    }
}
