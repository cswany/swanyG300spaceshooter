using UnityEngine;
using System.Collections;

public class Boss_WeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn1;
    public Transform shotSpawn2;
    public float fireRate;
    public float delay;

    [FMODUnity.EventRef]
    public string enemyFMOD = "event:/EnemyShoot";
    FMOD.Studio.EventInstance enemyEv;


    void Start()
    {
        InvokeRepeating("Fire", delay, fireRate);
        enemyEv = FMODUnity.RuntimeManager.CreateInstance(enemyFMOD);
    }

    void Fire()
    {
        Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
        Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
        enemyEv.start();
        Debug.Log("ENEMY fires");
    }
}
