using Audio;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "KitchenPhoneHaunt", menuName = "Haunts/Kitchen Phone")]
    public class KitchenPhoneHaunt : Haunt
    {
        public override void Execute()
        {
            float timer = 20f;
            FindObjectOfType<WallPhone>().Ring();
            timer -= Time.deltaTime;
        }
    }
}
