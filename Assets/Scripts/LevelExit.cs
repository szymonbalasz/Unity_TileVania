using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour
{
    bool triggered = false; // to prevent players' extra collider from triggering level skips
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") { return; }
        if (triggered) { return; }
        triggered = true;
        FindObjectOfType<GameSession>().LevelComplete();
    }
}
