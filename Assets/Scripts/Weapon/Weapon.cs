using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;  //Скорострельнсть

    private float nextFire;

    public Camera camera;
    
    void Update()
    {
        if (nextFire > 0)
            nextFire -= Time.deltaTime;

        if(Input.GetButtonDown("Fire1") && nextFire <= 0)
        {
            nextFire = 1 / fireRate;

            Fire();
        }
    }

    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            if(hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
}
