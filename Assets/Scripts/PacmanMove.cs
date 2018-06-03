using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour {

    Vector3 origPosition;

    void Start() {
        origPosition = transform.position;
    }

    bool move = false;
     void Update() {
        if(move) {
            float step = 3 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 20, transform.position.y, transform.position.z), step);
        }

    }
    
    void Move() {
        if((transform.position.x <= origPosition.x-10)) {
            move = true;
        } else {
            move = false;
        }

    }

}
