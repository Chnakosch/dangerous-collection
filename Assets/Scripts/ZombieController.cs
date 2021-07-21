using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject player;
    public float zombieSpeed;
    public float damage;
    public float health = 100;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if(health<=0)
            Destroy(gameObject);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        GetComponent<CharacterController>().SimpleMove(transform.forward * zombieSpeed);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "Player") {
            hit.gameObject.GetComponent<PlayerController>().health -= damage * Time.deltaTime;
        }    
    }
}
