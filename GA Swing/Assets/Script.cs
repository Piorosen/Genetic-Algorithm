using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    Rigidbody rigid;
    public Vector3 Velocity;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    float delta = 0.0f;
    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta > 0.5f)
        {
            rigid.AddForce(Vector3.forward * Random.Range(-100, 100));
            rigid.AddForce(Vector3.forward * Random.Range(-100, 100));
            rigid.AddForce(Vector3.forward * Random.Range(-100, 100));
            rigid.AddForce(Vector3.forward * Random.Range(-100, 100));
            rigid.AddForce(Vector3.forward * Random.Range(-100, 100));
            Velocity = rigid.velocity;
            delta -= 0.5f;
        }
    }
}
