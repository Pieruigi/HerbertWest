using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    [System.Serializable]
    public class MessageTrackBehaviour : PlayableBehaviour
    {
        [SerializeField]
        int messageId;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // Get the object attached to the track
            Text textField = playerData as Text;

            // Get the message from the id

            string message =  TextFactory.Instance.GetText(TextFactory.Type.InGameMessage, messageId);
            textField.text = message;

    
        }
    }
}
