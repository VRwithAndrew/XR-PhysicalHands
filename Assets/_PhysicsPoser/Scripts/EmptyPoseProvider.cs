using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.SpatialTracking;

public class EmptyPoseProvider : BasePoseProvider
{
    public override PoseDataFlags GetPoseFromProvider(out Pose output)
    {
        output.position = Vector3.zero;
        output.rotation = Quaternion.identity;

        return PoseDataFlags.NoData;
    }
}
