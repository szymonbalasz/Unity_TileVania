using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int scoreValue = 5;
    [SerializeField] AudioClip pickupSFX = default;
    [SerializeField] float pickupVol = 1f;

    int count = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (count == 1) { return; } // if jumping on coin from above both body and feet colliders trigger method
        AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position, pickupVol);
        FindObjectOfType<GameSession>().AddScore(scoreValue);
        count++;
        Destroy(gameObject);
    }
}
