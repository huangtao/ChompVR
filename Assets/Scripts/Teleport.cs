using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform teleportsTo;

    private Vector3 teleportToPosition;

    private PlayerController player;
    private Autowalk autowalk;
    

    private void Start()
    {
        player = PlayerController.Instance;
        autowalk = player.GetComponent<Autowalk>();

        // Save the position of the location to teleport to.
        // Ignore the y position and use the player's own y position instead, 
        // so that the player doesn't jump up or down when teleporting.
        teleportToPosition = new Vector3(teleportsTo.position.x, player.transform.position.y, teleportsTo.position.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }


    private void TeleportPlayer()
    {
        autowalk.enabled = false;
        player.transform.position = teleportToPosition;
        autowalk.enabled = true;
    }
}
