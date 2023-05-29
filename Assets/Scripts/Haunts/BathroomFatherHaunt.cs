using Objects;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "BathroomFatherHaunt", menuName = "Haunts/Bathroom Father")]
    public class BathroomFatherHaunt : Haunt
    {
        public override void Execute()
        {
            FindObjectOfType<FakeFather>().Activate();
        }
    }
}
