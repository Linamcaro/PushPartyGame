using Netcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.Netcode;
using UnityEngine;

public class LevelController : NetworkBehaviour
{
    private static LevelController _instance;
    public static LevelController Instance
    {
        get { return _instance;  }
    }

    public LevelPiece[] levelPieces;

    public int drawDistance;

    public float pieceLenght;
    public float speed;

    private Vector3 levelLength;

    Queue<GameObject> activePieces = new Queue<GameObject>();
    List<int> probabilityList = new List<int>();

    int currentCamStep = 0;
    int lastCamStep = 0;

    private void Awake()
    {
        _instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Singleton.InitializePool();

            StartGame();
        };

        BuildProbabilityList();

        currentCamStep = (int)(transform.position.z / pieceLenght);
        lastCamStep = currentCamStep;
    }

    private void Update()
    {
        if (!IsServer) return;

        if (PushPartyGameManager.Instance.IsGamePlaying())
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, Time.deltaTime * speed);
            currentCamStep = (int)(transform.position.z / pieceLenght);
            if (currentCamStep != lastCamStep)
            {
                lastCamStep = currentCamStep;
                DespawnLevelPiece();
                SpawnNewLevelPiece();
            }
        }
    }

    /// <summary>
    /// Instantiate a new random platform prefab from levelPieces
    /// 
    /// working on using network pool
    /// </summary>
    void SpawnNewLevelPiece()
    {
        if (!IsServer) return;

        int pieceIndex = probabilityList[Random.Range(0, probabilityList.Count)];

        if (activePieces.Count < 2) pieceIndex = 0;

        GameObject newLevelPiece = Instantiate(levelPieces[pieceIndex].prefab, new Vector3(0f, 0f, (currentCamStep + activePieces.Count) * pieceLenght), Quaternion.identity);
        //GameObject newLevelPiece = NetworkObjectPool.Singleton.GetNetworkObject(levelPieces[pieceIndex].prefab).gameObject;
        //newLevelPiece.transform.position = new Vector3(0f, 0f, (currentCamStep + activePieces.Count) * pieceLenght);
        newLevelPiece.GetComponent<NetworkObject>().Spawn();
        activePieces.Enqueue(newLevelPiece);

        levelLength = newLevelPiece.transform.position;
    }

    /// <summary>
    /// destroy the first platform form the activePieces Queue
    /// </summary>
    void DespawnLevelPiece()
    {
        if (!IsServer) return;

        GameObject oldLevelPiece = activePieces.Dequeue();
        NetworkObject netObject = oldLevelPiece.GetComponent<NetworkObject>();
        netObject.Despawn();
        Destroy(oldLevelPiece);
        //NetworkObjectPool.Singleton.ReturnNetworkObject(netObject, oldLevelPiece);
    }

    void BuildProbabilityList()
    {
        int index = 0;
        foreach (LevelPiece piece in levelPieces)
        {
            for (int i = 0; i < piece.probability; i++)
            {
                probabilityList.Add(index);
            }
            index++;
        }
    }


    public void StartGame()
    {
        if (!IsServer) return;

        //spawn starting level piece
        for (int i = 0; i < drawDistance; i++)
        {
            SpawnNewLevelPiece();
        }
    }

    /// <summary>
    /// Return the platform position
    /// </summary>
    /// <returns></returns>
    public Vector3  PlatformPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// Return the platform length
    /// </summary>
    /// <returns></returns>
    public Vector3 LevelLength()
    {

        if(levelLength == null)
        {
            return  new Vector3(0, 0, 0);
        }
        else
        {
            return levelLength;
        }

    }

}

  

[System.Serializable]
public class LevelPiece
{
    public string name;
    public GameObject prefab;
    public int probability = 1;
}
