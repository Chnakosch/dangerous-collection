using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject FadeIn, HUD;
    public float cameraSpeed;

    public float cameraHeight;
    public float cameraZDifference;

    public bool devMode; // MAYBE REMOVE LATER
    
    void Start()
    {
        FadeIn.SetActive(true);
        Invoke("HUDActive",1);

        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + cameraHeight,
            player.transform.position.z - cameraZDifference
        );
        transform.LookAt(player.transform);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, player.transform.position.x, cameraSpeed),
            Mathf.Lerp(transform.position.y, player.transform.position.y + cameraHeight, cameraSpeed),
            Mathf.Lerp(transform.position.z, player.transform.position.z - cameraZDifference, cameraSpeed)
        );

        // Dev Shit:
        if (devMode) {
            transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + cameraHeight,
                player.transform.position.z - cameraZDifference
            );
            transform.LookAt(player.transform);
        }
    }
    void HUDActive()
    {
        HUD.SetActive(true);
    }
}
