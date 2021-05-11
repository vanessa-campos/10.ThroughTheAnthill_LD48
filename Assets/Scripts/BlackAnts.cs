using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackAnts : Ants
{
    public bool leaf;
    Transform player;
    NavMeshAgent navMeshAgent;
    Vector3 pos1;
    Vector3 pos2;
    Vector3 netxPos;


    public enum State { WALK, CHASE }
    State state;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        state = State.WALK;
        pos1 = transform.position;
        pos1.z = limitI;
        pos2 = transform.position;
        pos2.z = limitF;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 3 && !leaf)
        {
            state = State.CHASE;
        }
        else
        {
            state = State.WALK;
        }

        switch (state)
        {
            case State.WALK:
                transform.LookAt(netxPos);
                if (transform.position.z <= limitI)
                {
                    netxPos = pos2;
                    transform.Rotate(rot);
                    Debug.Log(state);
                }
                if (transform.position.z >= limitF)
                {
                    netxPos = pos1;
                    transform.Rotate(rot);
                    Debug.Log(state);
                }
                Vector3 newPos = Vector3.MoveTowards(transform.position, netxPos, speed * Time.deltaTime);
                transform.position = newPos;
                break;
            case State.CHASE:
                Debug.Log(state);
                // navMeshAgent.SetDestination(player.position);
                netxPos = player.position;
                newPos = Vector3.MoveTowards(transform.position, netxPos, (1.5f * speed) * Time.deltaTime);
                transform.position = newPos;
                transform.LookAt(netxPos);
                break;
        }
    }
}
