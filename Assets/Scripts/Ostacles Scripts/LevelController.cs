using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelPiece[] levelPieces;
    public Transform camera;
    public int drawDistance;

    public float pieceLenght;
    public float speed;

    Queue<GameObject> activePieces = new Queue<GameObject>();
    List<int> probabilityList = new List<int>();

    int currentCamStep = 0;
    int lastCamStep = 0;

    private void Start()
    {
        camera = Camera.main.transform;
        BuildProbabilityList();

        //spawn starting level piece
        for(int i = 0; i < drawDistance; i++)
        {
            SpawnNewLevelPiece();
        }

        currentCamStep = (int)(camera.position.z / pieceLenght);
        lastCamStep = currentCamStep;
    }

    private void Update()
    {
        camera.position = Vector3.MoveTowards(camera.position, camera.position + Vector3.forward, Time.deltaTime * speed);
        currentCamStep = (int)(camera.position.z / pieceLenght);
        if(currentCamStep != lastCamStep)
        {
            lastCamStep = currentCamStep;
            DespawnLevelPiece();
            SpawnNewLevelPiece();
        }
    }

    void SpawnNewLevelPiece()
    {
        int pieceIndex = probabilityList[Random.Range(0, probabilityList.Count)];
        GameObject newLevelPiece = Instantiate(levelPieces[pieceIndex].prefab, new Vector3(0f, 0f, (currentCamStep + activePieces.Count) * pieceLenght), Quaternion.identity);
        activePieces.Enqueue(newLevelPiece);
    }

    void DespawnLevelPiece()
    {
        GameObject oldLevelPiece = activePieces.Dequeue();
        Destroy(oldLevelPiece);
    }

    void BuildProbabilityList()
    {
        int index = 0;
        foreach(LevelPiece piece in levelPieces)
        {
            for(int i = 0; i < piece.probability; i++)
            {
                probabilityList.Add(index);
            }
            index++;
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
