using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingAgent : Agent
{
    protected const float StoppedVelocity = .0005f; // Velocity threshold for MA to be considered stopped and told to move on to next destination in queue

    // Health
    protected float _health; // MA's current health level
    protected float _maxHealth; // MA's max possible health

    // Navigation
    private NavMeshAgent _navMeshAgent;
    private Queue<Vector3> _destinations;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        // Navigation
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _destinations = new Queue<Vector3>();
        SetDestination(transform.position);
    }

    // Update is called once per frame
    protected void Update()
    {
        // Navigation
        Vector3 currentDestination = transform.position;

        if (_destinations.Count > 0)
        {
            // Progress to next destination, if current destination has been reached. 
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        // Deque reached destination. Move on to the next destination or stop if there are no more. 
                        Vector3 stopPoint = _destinations.Dequeue();

                        if (_destinations.Count > 0)
                            currentDestination = _destinations.Peek();// Set navmesh dest to front of _destinations queue
                        else
                            _navMeshAgent.isStopped = true;
                    }
                }
            }
            // Assign destination to current queue
            else
            {
                currentDestination = _destinations.Peek();
            }
            _navMeshAgent.destination = currentDestination;
        }
    }

    // Clears destintion queue and adds passed-in destination as first destination in queue. 
    public void SetDestination(Vector3 dest)
    {
        ClearDestinationQueue();
        EnqueueDestination(dest);
    }

    // Adds destinationo to end of destination queue. 
    public void EnqueueDestination(Vector3 dest)
    {
        _destinations.Enqueue(dest);
        LogDestinationsQueue();
    }

    // Resets MA's health to max
    protected void ResetHealth()
    {
        _health = _maxHealth;
    }


    // Clears destination queue. 
    private void ClearDestinationQueue()
    {
        _destinations.Clear();
    }

    // TODO remove
    private void LogDestinationsQueue()
    {
        string print = "";
        foreach (Vector3 v in _destinations)
        {
            print += v;
            print += ", ";
        }
        Debug.Log(print);
    }
}