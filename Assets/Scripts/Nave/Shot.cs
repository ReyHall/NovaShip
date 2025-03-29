using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject shotPrefab;
    public float fireRate = 0.2f;
    private float nextFire = 0f;
    private readonly Vector3 shotOffset = new Vector3(0.5f, 0.5f, 0);

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        InstantiateShot(new Vector3(-shotOffset.x, shotOffset.y, 0));
        InstantiateShot(shotOffset);
    }


    private void InstantiateShot(Vector3 offset)
    {
        GameObject shot = Instantiate(shotPrefab, transform.position + offset, Quaternion.Euler(0, 0, 90));
        Destroy(shot, 1f);
    }


}

