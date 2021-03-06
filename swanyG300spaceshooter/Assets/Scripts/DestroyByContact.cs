﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour 
{
    [FMODUnity.EventRef]
    public string explosionFMOD = "event:/Explosion";
    FMOD.Studio.EventInstance explosionEv;

    private GameController gameController;

    public int scoreValue;

    public GameObject explosion;

    private void Start()
    {
        explosionEv = FMODUnity.RuntimeManager.CreateInstance(explosionFMOD);

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(other.gameObject);
        gameController.AddScore(scoreValue);
        Destroy(gameObject);
        explosionEv.start();
    }

}
