using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarScript : MonoBehaviour
{
    public GameObject neededWheel, neededBattery, neededFuel;
    private List<GameObject> neededStuff = new List<GameObject>();
    public List<GameObject> collectedStuff = new List<GameObject>();

    public GameObject FadeOut, HUD;

    void Start()
    {
        neededStuff.Add(neededWheel);
        neededStuff.Add(neededBattery);
        neededStuff.Add(neededFuel);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().inventory) {
            collectedStuff.Add(other.gameObject.GetComponent<PlayerController>().inventory);
            neededStuff.Remove(other.gameObject.GetComponent<PlayerController>().inventory);
            other.gameObject.GetComponent<PlayerController>().inventory.transform.SetParent(transform);

            if (other.gameObject.GetComponent<PlayerController>().inventory.Equals(neededWheel)) {
                other.gameObject.GetComponent<PlayerController>().inventory.transform.localEulerAngles = new Vector3(0, 0, 0);
                other.gameObject.GetComponent<PlayerController>().inventory.transform.localPosition = new Vector3(1.6f, -2.85f, -1.9f);
                other.gameObject.GetComponent<PlayerController>().inventory.GetComponent<MeshRenderer>().enabled = true;
            }

            other.gameObject.GetComponent<PlayerController>().inventory = null;

            if (neededStuff.Count == 0) {
                FadeOut.SetActive(true);
                HUD.SetActive(false);
                Invoke("Restart",2);
            }
        }
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
