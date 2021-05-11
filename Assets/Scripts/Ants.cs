using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ants : MonoBehaviour
{
    public int modelMov;
    public float speed = 0.75f;
    public float limitI;
    public float limitF;
    public Vector3 rot;
    public Transform pos;

    private void Start()
    {

    }

    void Update()
    {
        int num = modelMov;
        switch (num)
        {
            case 0:
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                if ((transform.position.x > limitF && speed < 0) || (transform.position.x < limitI && speed < 0))
                {
                    transform.Rotate(rot);
                }
                break;
            case 1:
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                if ((transform.position.x < limitF && speed < 0) || (transform.position.x > limitI && speed < 0))
                {
                    transform.Rotate(rot);
                }
                break;
            case 2:
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                if ((transform.position.z > limitF && speed < 0) || (transform.position.z < limitI && speed < 0))
                {
                    transform.Rotate(rot);
                }
                break;
            case 3:
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                if ((transform.position.z < limitF && speed < 0) || (transform.position.z > limitI && speed < 0))
                {
                    transform.Rotate(rot);
                }
                break;
            case 4:
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                if ((transform.position.z > limitF && speed > 0) || (transform.position.z < limitI && speed > 0))
                {
                    transform.Rotate(rot);
                }
                break;
        }
    }

}

