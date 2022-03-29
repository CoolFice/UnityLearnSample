using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Transform myTransform;
    public Vector3 target;
    public float speed;

    void Start()
    {
        myTransform = transform;
    }

    [ContextMenu("Отправить в небытие")]
    public void Use()
    {
        Vector3 start = myTransform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            myTransform.position = Vector3.Lerp(start, target, t);
        }
    }
}
