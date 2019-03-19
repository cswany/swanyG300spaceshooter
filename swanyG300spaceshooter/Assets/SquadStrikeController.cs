using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquadStrikeController : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string pewpew = "event:/Shoot";
    FMOD.Studio.EventInstance pewpewEv;


    public GameObject shot;
    public Transform shotSpawn;


    private void Start()
    {
        StartCoroutine(ShootShoot());
    }

    private IEnumerator ShootShoot()
    {
        Debug.Log("hereiam");
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while(true)
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            pewpewEv = FMODUnity.RuntimeManager.CreateInstance(pewpew);
            pewpewEv.start();
            yield return wait;
        }
    }
}
