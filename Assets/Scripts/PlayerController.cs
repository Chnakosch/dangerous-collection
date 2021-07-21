using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public List<GameObject> weapons;

    public GameObject weaponInHand; //was private

    public GameObject FadeOut, HUD;
    public GameObject inventory;

    public float health;
    public float maxHealth;

    public GameObject bulletPrefab;

    private bool canFire = true;

    public bool reloading = false;

    void Start()
    {
        inventory = null;
        if (weapons.Count != 0) {
            weaponInHand = weapons[0];
            weaponInHand.GetComponent<MeshCollider>().enabled = false;
            weaponInHand.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            weaponInHand.transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 0);
            weaponInHand.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            weaponInHand.transform.Translate(weaponInHand.transform.forward * -0.35f);

            weaponInHand.gameObject.GetComponent<WeaponScript>().currentAmmo = weaponInHand.gameObject.GetComponent<WeaponScript>().maxAmmo;

            weaponInHand.gameObject.layer = 11; // NoShadow layer
        } else {
            weaponInHand = null;
        }
    }

    void Update()
    {
        // Simple Shooting
        if (Input.GetButtonDown("Fire1") && weaponInHand != null && canFire && weaponInHand.GetComponent<WeaponScript>().currentAmmo > 0) {
            if (weaponInHand.GetComponent<WeaponScript>().fullAuto) {
                InvokeRepeating("FireBullet", 0, weaponInHand.GetComponent<WeaponScript>().fireRate);
            } else {
                canFire = false;
                FireBullet();
                Invoke("CanFireReset", weaponInHand.GetComponent<WeaponScript>().fireRate);
            }  
        }
        if (Input.GetButtonUp("Fire1") || (weaponInHand != null && weaponInHand.GetComponent<WeaponScript>().currentAmmo <= 0)) {
            CancelInvoke("FireBullet");
        }
        //Reload
        if(Input.GetButtonDown("Reload") && weaponInHand != null && canFire)
        {
            canFire = false;
            reloading = true;
            CancelInvoke("FireBullet");
            Invoke("ReloadGun",weaponInHand.GetComponent<WeaponScript>().reloadTime);
        }
        // Weapon switch with mouse wheel
        if (weapons.Count != 0) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                canFire = true;
                reloading = false;
                CancelInvoke("FireBullet");
                CancelInvoke("ReloadGun");
                weaponInHand.GetComponent<MeshRenderer>().enabled = false;
                if (weapons.IndexOf(weaponInHand) == weapons.Count - 1) {
                    weaponInHand = weapons[0];
                } else {
                    weaponInHand = weapons[weapons.IndexOf(weaponInHand) + 1];
                }
                weaponInHand.GetComponent<MeshRenderer>().enabled = true;
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                canFire = true;
                reloading = false;
                CancelInvoke("FireBullet");
                CancelInvoke("ReloadGun");
                weaponInHand.GetComponent<MeshRenderer>().enabled = false;
                if (weapons.IndexOf(weaponInHand) == 0) {
                    weaponInHand = weapons[weapons.Count -1];
                } else {
                    weaponInHand = weapons[weapons.IndexOf(weaponInHand) - 1];
                }
                weaponInHand.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        // Mouse aim
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("MouseAimLayer"))) {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }

        // Movement
        GetComponent<CharacterController>().SimpleMove(new Vector3(
            Input.GetAxis("Horizontal") * playerSpeed,
            0,
            Input.GetAxis("Vertical") * playerSpeed
        ));
        //Death
        if(health<=0)
        {
            FadeOut.SetActive(true);
            HUD.SetActive(false);
            Invoke("Restart",2);
        } 
    }
    // This method fires a bullet :)
    void FireBullet() {
        GameObject bullet = Instantiate(bulletPrefab,weapons[0].transform.position,weapons[0].transform.rotation);
        bullet.GetComponent<BulletController>().damage = weaponInHand.GetComponent<WeaponScript>().damage;
        weaponInHand.GetComponent<WeaponScript>().currentAmmo--;
    }
    // Reload method
    void ReloadGun()
    {
        weaponInHand.GetComponent<WeaponScript>().currentAmmo = weaponInHand.GetComponent<WeaponScript>().maxAmmo;
        reloading = false;
        CanFireReset();
    }

    // This method makes it possible to fire a bullet again
    void CanFireReset() {
        canFire = true;
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void OnControllerColliderHit(ControllerColliderHit hit) {

        // Picking up a weapon
        if (hit.gameObject.tag == "weapon") {
            weapons.Add(hit.gameObject);
            hit.collider.enabled = false;
            hit.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            hit.transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 0);
            hit.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            hit.transform.Translate(hit.transform.forward * -0.35f);

            hit.gameObject.GetComponent<WeaponScript>().currentAmmo = hit.gameObject.GetComponent<WeaponScript>().maxAmmo;

            hit.gameObject.layer = 11; // NoShadow layer

            if (!weaponInHand) {
                weaponInHand = weapons[0];
            } else {
                hit.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
                     
            hit.transform.SetParent(transform);
        }

        // Picking up a medkit
        if (hit.gameObject.tag == "medkit" && health < maxHealth) {
            health += hit.gameObject.GetComponent<MedkitScript>().healValue;
            if (health > maxHealth) health = maxHealth;
            Destroy(hit.gameObject);
        }

        // Picking up collectibles
        if (hit.gameObject.tag == "collectible" && !inventory) {
            inventory = hit.gameObject;
            hit.collider.enabled = false;
            inventory.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
