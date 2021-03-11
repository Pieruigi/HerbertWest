using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Zom.Pie
{
    [TrackColor(1, 0, 0)]
    [TrackBindingType(typeof(PlayerManager))]
    [TrackClipType(typeof(PlayerTrackController))]
    public class PlayerTrackAsset : TrackAsset
    {

    }
}
