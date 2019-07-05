using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int scoreValue = 5;
    [SerializeField] AudioClip pickupSFX = default;
    [SerializeField] float pickupVol = 1f;

    bool taken = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (taken) { return; }
        else
        {
            taken = true;
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position, pickupVol);
            FindObjectOfType<GameSession>().AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
