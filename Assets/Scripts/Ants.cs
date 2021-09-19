using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ants : MonoBehaviour
{
    public int modelMov;
    public float speed = 1;
    public float limitI;
    public float limitF;
    public Transform pos;


    void Update()
    {
        int num = modelMov;
        switch (num)
        {
            case 0:
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                if ((transform.position.x > limitF && speed > 0) || (transform.position.x < limitI && speed < 0)){
                    var scale = transform.localScale;
                    scale.z *= -1;
                    transform.localScale = scale;
                    speed *= -1;
                }
                break;
            case 1:
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
                if ((transform.position.x < limitF && speed > 0) || (transform.position.x > limitI && speed < 0)){
                    var scale = transform.localScale;
                    scale.z *= -1;
                    transform.localScale = scale;
                    speed *= -1;
                }
                break;
            case 2:
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
                if ((transform.position.z > limitF && speed > 0) || (transform.position.z < limitI && speed < 0)){
                    var scale = transform.localScale;
                    scale.z *= -1;
                    transform.localScale = scale;
                    speed *= -1;
                }
                break;
            case 3:
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
                if ((transform.position.z < limitF && speed > 0) || (transform.position.z > limitI && speed < 0)){
                    var scale = transform.localScale;
                    scale.z *= -1;
                    transform.localScale = scale;
                    speed *= -1;
                }
                break;
        }
    }
}



