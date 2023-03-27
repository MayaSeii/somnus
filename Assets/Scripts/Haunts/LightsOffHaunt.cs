using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;

namespace Haunts
{
    [CreateAssetMenu(fileName = "LightsOffHaunt", menuName = "Haunts/Lights Off")]
    public class LightsOffHaunt : Haunt
    {
        private List<LightSwitch> _lightSwitches;

        public override void Execute()
        {
            _lightSwitches = FindObjectsOfType<LightSwitch>().ToList();
            _lightSwitches.ForEach(l => l.ForceOff());
        }
    }
}
