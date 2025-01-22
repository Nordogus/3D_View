using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 7f;
    [SerializeField]
    float rotSpeed = 50f;
    bool moveBack = false;
    [SerializeField]
    Vector2 maxPos = Vector2.one;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
        UpdatePosition();
    }

    void UpdateRotation()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += Time.deltaTime * rotSpeed;
        transform.rotation = Quaternion.Euler(rot);
    }

    void UpdatePosition()
    {
        if (!moveBack)
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            if (transform.position.x > maxPos.x) moveBack = true;
        }
        else
        {
            transform.position -= Vector3.right * Time.deltaTime * moveSpeed;
            if (transform.position.x < -maxPos.x) moveBack = false;
        }

    }
}
