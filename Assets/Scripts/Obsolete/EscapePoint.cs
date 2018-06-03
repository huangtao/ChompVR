using UnityEngine;
using System.Collections;
using System.Linq;


// FRED - I do not see this used anywhere.  
//        If it is obsolete, let's remove it completely.  
//        Delete this comment if it is found to be used somewhere.

public class EscapePoint : MonoBehaviour
{
    public GameObject player;
    public Vector3[] corners;


    void Start()
    {
        StartCoroutine(RefreshEscapeRoute());
    }


    IEnumerator RefreshEscapeRoute()
    {
        while (true)
        {
            transform.position = corners.OrderByDescending(x => Vector3.Distance(x, player.transform.position)).FirstOrDefault();
            yield return new WaitForSeconds(1);
        }
    }
}
