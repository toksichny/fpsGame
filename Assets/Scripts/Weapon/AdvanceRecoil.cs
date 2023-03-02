using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation, targetPos, currentPos, initialGunPos;
    public Transform cam;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;

    [SerializeField] float kickBackZ;

    public float snappiness;
    public float returnSpeed;
    void Start()
    {
        initialGunPos = transform.localPosition;
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);

        back();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            recoil();
        }
    }

    public void recoil()
    {
        targetPos += new Vector3(0, 0, kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    void back()
    {
        targetPos = Vector3.Lerp(targetPos, initialGunPos, Time.deltaTime * returnSpeed);
        currentPos = Vector3.Lerp(currentPos, targetPos, Time.fixedDeltaTime * snappiness);
        transform.localPosition = currentPos;
    }
}
