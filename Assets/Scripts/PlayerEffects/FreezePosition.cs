using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePosition : MonoBehaviour
{
    public string playerTag = "Player";
    public float freezeDuration = 50f;

    public List<GameObject> players = new List<GameObject>();
    private Coroutine freezeCoroutine;

    private GameObject collidingPlayer;

    private bool isFound;
    private void Update()
    {
        if (!isFound)
        {
            if (PushPartyGameManager.Instance.IsGamePlaying())
            {
                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag(playerTag);
                foreach (GameObject playerObject in playerObjects)
                {
                    players.Add(playerObject);
                    
                }
                isFound = true;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag(playerTag) && !players.Contains(other.gameObject))
        {
            players.Add(other.gameObject);
        }

        if (players.Count > 1 && freezeCoroutine == null)
        {
            collidingPlayer = other.gameObject;


            foreach (GameObject player in players)
            {
                if (player != collidingPlayer)
                {
                    freezeCoroutine = StartCoroutine(FreezePlayer(player));
                    break;
                }
            }
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && players.Contains(other.gameObject))
        {
            players.Remove(other.gameObject);

            if (other.gameObject == collidingPlayer)
            {
                collidingPlayer = null;
            }
        }
    }

    private IEnumerator FreezePlayer(GameObject player)
    {
       
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        yield return new WaitForSeconds(freezeDuration);

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        players.Remove(player);

        if (player == collidingPlayer)
        {
            collidingPlayer = null;
        }

        freezeCoroutine = null;
    }
}

