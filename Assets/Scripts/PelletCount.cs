using UnityEngine;

public class PelletCount : MonoBehaviour
{
    void Start()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.RegisterPellet();
        }
    }
}
