using UnityEngine;
using TMPro;
using Photon.Pun;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    //Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform WeaponAttackPoint;
    public RaycastHit rayHit;
    //public LayerMask whatIsEnemy;

    //Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI text;
    //public CamShake camShake;
    public float camShakeMagnitude, camShakeDuration;

    [SerializeField] public float recoilX;
    [SerializeField] public float recoilY;
    [SerializeField] public float recoilZ;

    [SerializeField] public float aimRecoilX;
    [SerializeField] public float aimRecoilY;
    [SerializeField] public float aimRecoilZ;

    //Settings
    [SerializeField] public float snappiness;
    [SerializeField] public float returnSpeed;

    //Bullet
    [Header("BulletSettings")]
    [SerializeField]
    public GameObject weaponBullet;
    public Transform weaponFirePos;
    public float weaponSpeed = 15.0f;
    public float weaponLife = 3.0f;

    [Header("VFX")]
    public GameObject hitVFX;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();

        //SetText
        text.SetText(bulletsLeft + "/" + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);


        Ray ray = new Ray(fpsCam.transform.position, fpsCam.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            //Hit VFX
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }

        //ShakeCamera
        //StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));



        //Graphics
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShoot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }


    private void ResetShoot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
