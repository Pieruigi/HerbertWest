using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Zom.Pie
{
    [System.Serializable]
    public class PlayerTrackBehaviour : PlayableBehaviour
    {
        [SerializeField]
        bool disabled = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // Get the object attached to the track
            PlayerManager p = playerData as PlayerManager;

            p.SetDisable(disabled);
        
        }
    }
}