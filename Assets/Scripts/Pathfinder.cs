using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private GridManager gridManager;
    private Dictionary<Vector3Int, Node> gameGrid;

    [SerializeField] private Node currentSearchNode;
    [SerializeField] Vector2Int pathStart;
    [SerializeField] Vector2Int pathEnd;

    // This array influences the path depending on the order of directions
    Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
    Vector3Int[] height = { Vector3Int.back, Vector3Int.forward };


    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();

        if (this.gridManager != null)
            this.gameGrid = this.gridManager.GameGrid;
    }

    // Start is called before the first frame update
    void Start()
    {
        ExploreNeighbours();
    }

    private void ExploreNeighbours()
    {
        // TODO: tweak this method so it recognizes neighboring tiles lying at a different height

        List<Node> neighboursList = new List<Node>();

        for(int i = 0; i < this.directions.Length; i++)
        {
            Vector3Int curNeighbourCoords = this.currentSearchNode.Coordinates + this.directions[i];

            if (this.gameGrid.ContainsKey(curNeighbourCoords))
            {
                if (GameObject.Find(curNeighbourCoords.ToString()) == null)
                {
                    for (int j = 0; j < this.height.Length; j++)
                    {
                        curNeighbourCoords = curNeighbourCoords + this.height[j];

                        if (GameObject.Find(curNeighbourCoords.ToString()) != null)
                        {
                            break;
                        }

                        curNeighbourCoords = curNeighbourCoords - this.height[j];
                    }
                }                    

                neighboursList.Add(this.gameGrid[curNeighbourCoords]);

                // TODO: remove after testing
                this.gameGrid[curNeighbourCoords].SetIsExplored(true);
                this.gameGrid[this.currentSearchNode.Coordinates].SetIsPath(true);
            }
                
        }

    }
}
