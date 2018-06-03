using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour {

    [SerializeField]
    private float tileSize;

    void Start() {
        InvokeRepeating("UpdatePosition", 0, 0.5f);
    }

    void UpdatePosition() {
        transform.position = RoundPosition(transform.position);
    }

    Vector3 RoundPosition(Vector3 pos) {
        float factor = tileSize;

        Vector3 multipliedPosition = pos * factor;
        var roundedPosition = new Vector3((float) System.Math.Round(multipliedPosition.x, System.MidpointRounding.AwayFromZero), (float) System.Math.Round(multipliedPosition.y, System.MidpointRounding.AwayFromZero), (float) System.Math.Round(multipliedPosition.z, System.MidpointRounding.AwayFromZero));
        Vector3 finalPosition = roundedPosition / factor;

        return finalPosition;
    }

}
