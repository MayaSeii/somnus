using Audio;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "FrontDoorHaunt", menuName = "Haunts/Front Door")]
    public class FrontDoorHaunt : Haunt
    {
        public override void Execute()
        {
            FindObjectOfType<FrontDoor>().Knock();
        }
    }
}
