using UnityEngine;
public class ActionMapper
{
    public static bool GetClick() {
        return Input.GetMouseButtonDown(0) || Google.XR.Cardboard.Api.IsTriggerPressed || GetOculusTrigger();
    }
    private static bool GetOculusTrigger() {
        bool value = false;
        if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand).TryGetFeatureValue(new ("TriggerTouch"), out value)) {
            return value;
        }
        return value;
    }
}
