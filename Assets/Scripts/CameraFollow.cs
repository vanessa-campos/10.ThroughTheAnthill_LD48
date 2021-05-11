using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followDelay = 4;
    PlayerController player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        // Vector3 p = player.transform.position;
        // transform.position = new Vector3(p.x,-9,-p.z);
        offset = transform.position - player.transform.position;
    }

    // Camera Updates should be done in LateUpdate
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * followDelay);
    }
}
