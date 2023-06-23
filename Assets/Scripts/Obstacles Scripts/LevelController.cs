using Netcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.Netcode;
using UnityEngine;

public class LevelController : NetworkBehaviour
{

    public static LevelController Instance;

    public LevelPiece[] levelPieces;

    //public Transform camera;
    public int drawDistance;

    public float pieceLenght;
    public float speed;

    Queue<GameObject> activePieces = new Queue<GameObject>();
    List<int> probabilityList = new List<int>();

    int currentCamStep = 0;
    int lastCamStep = 0;

    public bool hasServerStarted = false;
    public bool hasGameStarted = false;


    private void Start()
    {
        if (Instance == null)
        {

            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Singleton.InitializePool();
            hasServerStarted = true;

            for (int i = 0; i < 2; i++)
            {
                SpawnNewLevelPiece();
            }
        };

        //camera = Camera.main.transform;
        BuildProbabilityList();

        currentCamStep = (int)(transform.position.z / pieceLenght);
        lastCamStep = currentCamStep;
    }

    private void Update()
    {
        if (!IsServer) return;

        if(hasGameStarted)
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

    void SpawnNewLevelPiece()
    {
        if (!IsServer) return;

        int pieceIndex = probabilityList[Random.Range(0, probabilityList.Count)];

        if (activePieces.Count < 2) pieceIndex = 0;

        //GameObject newLevelPiece = Instantiate(levelPieces[pieceIndex].prefab, new Vector3(0f, 0f, (currentCamStep + activePieces.Count) * pieceLenght), Quaternion.identity);
        GameObject newLevelPiece = NetworkObjectPool.Singleton.GetNetworkObject(levelPieces[pieceIndex].prefab).gameObject;
        newLevelPiece.transform.position = new Vector3(0f, 0f, (currentCamStep + activePieces.Count) * pieceLenght);
        newLevelPiece.GetComponent<NetworkObject>().Spawn();
        activePieces.Enqueue(newLevelPiece);
    }

    void DespawnLevelPiece()
    {
        if (!IsServer) return;

        GameObject oldLevelPiece = activePieces.Dequeue();
        NetworkObject netObject = oldLevelPiece.GetComponent<NetworkObject>();
        netObject.Despawn();
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

        hasGameStarted = true;

        //spawn starting level piece
        for (int i = 0; i < drawDistance; i++)
        {
            SpawnNewLevelPiece();
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
