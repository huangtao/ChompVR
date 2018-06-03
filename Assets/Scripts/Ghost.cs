using System.Collections;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    private Material vulnerableMaterial;

    private Material defaultMaterial;

    [SerializeField]
    private new SkinnedMeshRenderer renderer;

    [SerializeField]
    private int respawnDelay = 5;

    [SerializeField]
    private GameObject spawnPointPrefab;

    [SerializeField]
    private GameObject[] hide;

    [SerializeField]
    private GameObject[] show;

    public TargetType TargetType = TargetType.Random;

    /// <summary>
    /// Whether the ghost is currently vulnerable, dead or otherwise.
    /// </summary>
    public GhostState State;

    [HideInInspector]
    public Transform currentTarget;

    [HideInInspector]
    public GameObject ghostModel;

    private string spawnPointName;
    private Transform startPoint;
    private NavMeshAgent agent;


    /// <summary>
    /// Whether the ghost is currently frozen or not.  
    /// Once the scene has been won or lost, the ghost should become frozen.
    /// </summary>
    private bool isFrozen;


    private void Start()
    {
        State = GhostState.Killer;
        startPoint = CreateRunawayTarget();
        ghostModel = gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Model").gameObject;
        defaultMaterial = renderer.sharedMaterials[0];
        agent = GetComponent<NavMeshAgent>();
    }


    private Transform CreateRunawayTarget()
    {
        spawnPointName = name + "SpawnPoint";

        GameObject go = (GameObject)Instantiate(spawnPointPrefab, transform.position, transform.rotation);
        go.name = spawnPointName;
        return go.transform;
    }


    private void Update()
    {
        if (ApplicationModel.GameState == GameState.Playing && !isFrozen)
        {
            // Since the target moves, need to refresh the destination based on the target's current location
            
            if (State == GhostState.Killer)
                agent.SetDestination(currentTarget.position);
            else
                agent.SetDestination(startPoint.position);
        }
        else if (agent.hasPath)
        {
            agent.Stop();
        }
    }


    public void ChangeTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }


    /// <summary>
    /// Updates the ghost's main material
    /// </summary>
    private void SetMaterial(Material material)
    {
        var array = renderer.materials;
        array[0] = material;
        renderer.materials = array;
    }


    /// <summary>
    /// Makes the ghost vulnerable.
    /// Vulnerable ghosts will flee to the spawn point.
    /// </summary>
    public void StartVulnerable()
    {
        State = GhostState.Vulnerable;

        foreach (var item in hide)
        {
            item.SetActive(false);
        }

        foreach (var item in show)
        {
            item.SetActive(true);
        }

        SetMaterial(vulnerableMaterial);
    }


    /// <summary>
    /// Returns ghosts to being invulnerable and resume normal behavior.
    /// </summary>
    public void EndVulnerable()
    {
        // Do not disable vulnerability until ghost has respawned
        if (State == GhostState.Dead)
            return;

        State = GhostState.Killer;

        foreach (var item in hide)
        {
            item.SetActive(true);
        }

        foreach (var item in show)
        {
            item.SetActive(false);
        }

        SetMaterial(defaultMaterial);
    }


    public void Die()
    {
        State = GhostState.Dead;
        ghostModel.SetActive(false);

        // Ghost dies at respawn point.
        float distance = Vector3.Distance(transform.position, startPoint.position);
        if (distance < .5)
        {
            StartCoroutine(Respawn());
        }
    }


    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        State = GhostState.Killer;
        ghostModel.SetActive(true);

        EndVulnerable();
    }


    public void ResetToStartPosition(float wait = 1f)
    {
        StartCoroutine(ResetPositionAndPause(wait));
    }


    private IEnumerator ResetPositionAndPause(float wait)
    {
        isFrozen = true;
        agent.Stop();   // Maybe this is not necessary but it seems like the ghost gets 
                        // stuck and it might be because of moving while the agent is moving.

        agent.Warp(startPoint.position);
        transform.rotation = startPoint.rotation;

        yield return new WaitForSeconds(wait);
        isFrozen = false;
        agent.Resume();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (State == GhostState.Vulnerable)
            {
                // Player eats the ghost.
                GhostManager.Instance.KillGhost(this);
            }
            else if (State == GhostState.Killer)
            {
                // Ghost kills the player.
                GameController.Instance.KillPlayer();
            }
        }
        else if (State == GhostState.Dead && collider.name == spawnPointName)
        {
            // Dead ghost has reached respawn point.
            StartCoroutine(Respawn());
        }
        else if (collider.gameObject.transform == currentTarget)
        {
            // Ghost has reached target.  Set new targets.
            GhostManager.Instance.SetRandomTargets();
        }
    }
}
