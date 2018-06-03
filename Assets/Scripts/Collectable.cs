using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem collectParticles;

    [SerializeField]
    private bool IsPowerup;

    private GameController gameController;


    void Start()
    {
        gameController = GameController.Instance;

        var particles = GameObject.FindGameObjectWithTag("collectParticles");

        if (particles != null)
        {
            collectParticles = particles.GetComponent<ParticleSystem>();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (gameController && other.CompareTag("Player"))
        {
            gameController.CollectObject(tag);
            Collect(tag);
        }
    }


    void Collect(string tag)
    {
        Sound sound = tag == "pellet" ? Sound.PelletEaten : tag == "fruit" ? Sound.FruitEaten : Sound.PowerPelletEaten;
        MusicManager.Instance.PlayOneShot(sound);

        if (collectParticles && tag != "fruit")
        {
            collectParticles.transform.position = transform.position;
            collectParticles.transform.rotation = Quaternion.identity;
            collectParticles.Play();
        }

        Destroy(gameObject);
    }
}
