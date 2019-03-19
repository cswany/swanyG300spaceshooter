using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
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
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        enemyEv.start();
        Debug.Log("ENEMY fires");
    }
}
