using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class RoomController : MonoBehaviour
    {
        [field: SerializeField] public string RoomName { get; set; }
        [field: SerializeField] public List<Collider> Areas { get; set; }
        [field: SerializeField] public List<Light> CeilingLamps { get; set; }
    }
}
