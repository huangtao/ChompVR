using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostManager : Singleton<GhostManager>
{
    [SerializeField]
    private List<Ghost> ghosts;

    [SerializeField]
    private List<Transform> randomTargets;


    private int firstGhostIndex = -1;
    private int previousFirstGhostIndex = -1;


    private void Start()
    {
        SetRandomTargets();
        SetPlayerTarget();
    }


    /// <summary>
    /// Rotates the targets so that each target is now at a new position
    /// </summary>
    public void SetRandomTargets()
    {
        // For simplicity, rotate the targets rather than randomize them
        firstGhostIndex = GetIndex(previousFirstGhostIndex + 1);
        int index = firstGhostIndex;

        foreach (var ghost in ghosts.Where(g => g.TargetType == TargetType.Random))
        {
            ghost.ChangeTarget(GetTarget(ref index));
            index++;
        }

        previousFirstGhostIndex = firstGhostIndex;
    }


    private void SetPlayerTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        foreach (var chaser in ghosts.Where(g => g.TargetType == TargetType.Player))
        {
            chaser.ChangeTarget(player.transform);
        }
    }


    private int GetIndex(int index)
    {
        if (index >= randomTargets.Count)
            return 0;
        else
            return index;
    }


    /// <summary>
    /// Finds the next target in the list of random targets
    /// </summary>
    private Transform GetTarget(ref int index)
    {
        index = GetIndex(index);
        return randomTargets[index].transform;
    }


    public void KillGhost(Ghost ghost)
    {
        ghost.Die();
        MusicManager.Instance.PlayOneShot(Sound.GhostEaten);
        GameController.Instance.EatGhost();
    }


    /// <summary>
    /// Starts ghosts being vulnerable
    /// </summary>
    public void StartVulnerability()
    {
        ghosts.ForEach(ghost => ghost.StartVulnerable());
    }


    /// <summary>
    /// Stops ghosts from being vulnerable
    /// </summary>
    public void EndVulnerability()
    {
        ghosts.ForEach(ghost => ghost.EndVulnerable());
    }


    public void RespawnGhosts()
    {
        ghosts.ForEach(ghost => ghost.ResetToStartPosition(1));
    }
}
