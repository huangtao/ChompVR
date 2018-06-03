using UnityEngine;
using System.Collections;

public class TopDown : MonoBehaviour
{

    public GameObject Camera;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.transform.rotation.eulerAngles.x < 39 && Camera.transform.rotation.eulerAngles.x > 0)
            transform.rotation = Quaternion.Euler(-Camera.transform.rotation.eulerAngles.x * 2f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
