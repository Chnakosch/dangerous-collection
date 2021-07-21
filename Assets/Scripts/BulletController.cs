using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage;
    public float bulletSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.down * bulletSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision) 
    {
        Destroy(gameObject);
        if(collision.collider.tag == "zombie")
        {
            collision.collider.gameObject.GetComponent<ZombieController>().health -= damage;
        }
    }
}
