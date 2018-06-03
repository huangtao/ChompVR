using UnityEngine;

public class DeviceModeEnabled : MonoBehaviour
{
    void Start()
    {
        if (ApplicationModel.DeviceMode == DeviceMode.Mobile)
        {
            var vr = GetComponent<GvrViewer>();
            vr.VRModeEnabled = false;
        }
    }
}
