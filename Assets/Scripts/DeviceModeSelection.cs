using UnityEngine;
using UnityEngine.SceneManagement;

public class DeviceModeSelection : MonoBehaviour
{
    public void SetDeviceMode(int mode)
    {
        ApplicationModel.DeviceMode = (DeviceMode)mode;
        SceneManager.LoadScene(1);
    }
}
