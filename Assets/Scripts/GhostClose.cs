using UnityEngine;

public class GhostClose : MonoBehaviour
{
    private Ghost ghost;
    private PlayerController player;


    void Start()
    {
        ghost = GetComponentInParent<Ghost>();
        player = PlayerController.Instance;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ghost.State != GhostState.Vulnerable)
        {
            MusicManager.Instance.PlayOneShot(Sound.GhostScream);
        }
    }
}
