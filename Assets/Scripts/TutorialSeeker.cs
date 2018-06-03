using UnityEngine;
using System.Collections;

public class TutorialSeeker : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    //Vector3 startingPosition;

    private NavMeshAgent clyde;


    private void Start()
    {
        //startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        clyde = GameObject.Find("Clyde").GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (ApplicationModel.GameState == GameState.Playing && agent.enabled)
        {
            // Since the target moves, need to refresh the destination based on the target's current location
            agent.SetDestination(target.position);
        }
        else if (agent.hasPath)
        {
            agent.Stop();
        }
    }


    public void ChangeTarget(GameObject newTarget)
    {
        target = newTarget.transform;
        agent.SetDestination(newTarget.transform.position);
        UpdateTarget();
        clyde.Resume();
    }


    public void ChangeTargetPower(GameObject newTarget)
    {
        StartCoroutine(ChangeSpeed());
        ChangeTarget(newTarget);
    }


    public void ChangeTargetPostTeleport(GameObject newTarget)
    {
        ChangeTarget(newTarget);
        StartCoroutine(ToggleStatus(false));
    }


    private IEnumerator ChangeSpeed()
    {
        agent.speed *= 1.5f;
        yield return new WaitForSeconds(5f);
        agent.speed /= 1.5f;
    }


    private IEnumerator ToggleStatus(bool stat)
    {
        enabled = stat;
        yield return new WaitForSeconds(.1f);
        enabled = !stat;
        yield return new WaitForSeconds(.1f);
        enabled = stat;
    }


    public void UpdateTarget()
    {
        agent.enabled = false; //for some reason this is neccessary to reset the agent target. 
        agent.enabled = true;  //
    }


    public void Teleport(GameObject otherTeleport)
    {
        GameObject whereTo = otherTeleport.transform.GetChild(0).gameObject;
        agent.Warp(new Vector3(whereTo.transform.position.x, transform.position.y, whereTo.transform.position.z));
        ChangeTarget(otherTeleport);
        StartCoroutine(ToggleStatus(false));
        clyde.speed /= 1.5f;
    }
}
