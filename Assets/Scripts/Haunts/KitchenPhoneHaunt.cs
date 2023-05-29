using Audio;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "KitchenPhoneHaunt", menuName = "Haunts/Kitchen Phone")]
    public class KitchenPhoneHaunt : Haunt
    {
        public override void Execute()
        {
            FindObjectOfType<WallPhone>().Ring();
        }
    }
}
