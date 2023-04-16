using Objects;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "TVOnHaunt", menuName = "Haunts/TV On")]
    public class TVOnHaunt : Haunt
    {
        private Television _tv;

        public override void Execute()
        {
            _tv = FindObjectOfType<Television>();
            _tv.TurnOn();
        }
    }
}
