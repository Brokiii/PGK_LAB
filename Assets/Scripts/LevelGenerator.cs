using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public Transform levelStartPoint;

    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();

    public int maxTimeGame = 20;
    bool shouldFinish = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AddPiece();
        AddPiece();
        shouldFinish = false;
    }

    public void Finish()
    {
        shouldFinish = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPiece()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Count - 1);
        LevelPieceBasic piece;

        if (shouldFinish)
        {
            piece = (LevelPieceBasic)Instantiate(levelPrefabs[3]);
        }
        else
        {
            if (pieces.Count < 1)
            {
                piece = (LevelPieceBasic)Instantiate(levelPrefabs[0]);
            }
            else
            {
                piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
            }
        }
        piece.transform.SetParent(this.transform, false);


        if (pieces.Count < 1)
        {
            piece.transform.position = new Vector2(0, 0);
        }
        else
            piece.transform.position = new Vector2(
                     pieces[pieces.Count - 1].exitPoint.position.x + (float)15.26 + (float)2,
                     pieces[pieces.Count - 1].exitPoint.position.y + (float)4.81
                );

        pieces.Add(piece);
    }

    public void RemoveOldestPiece()
    {
        if(pieces.Count > 1)
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
    }
}
