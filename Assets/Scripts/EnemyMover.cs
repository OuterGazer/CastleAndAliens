using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    //[SerializeField] List<Waypoint> path = new List<Waypoint>();
    private List<Node> path = new List<Node>();
    [Range(0f, 5f)] [SerializeField] float movementSpeed = default;

    private List<List<Node>> possiblePaths = new List<List<Node>>();
    private Dictionary<List<Node>, int> pathsWithDangerLevel = new Dictionary<List<Node>, int>();

    private int spawnCount = 0;

    private GridManager gridManager;
    private Pathfinder pathFinder;

    private bool canCalculatePath = false;


    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();
        this.pathFinder = GameObject.FindObjectOfType<Pathfinder>();

        this.canCalculatePath = false;
    }

    void OnEnable()
    {
        CheckHowManyTimesEnemyInstanceHasSpawned();

        if (!this.canCalculatePath)
        {
            FindPath();
            return; 
        }

        ReturnToStart();
        Debug.Log("This enemy has these many paths available: " + this.possiblePaths.Count);
        AssignPath();
        //FindPath(); //FindPath(true) // For dynamic pathfinding        
        this.StartCoroutine(FollowPath()); // For dynamic pathfinding we need to erase this line
    }

    private void CheckHowManyTimesEnemyInstanceHasSpawned()
    {
        this.spawnCount++;

        if (!this.canCalculatePath && this.spawnCount > 1)
            this.canCalculatePath = true;
    }

    private void ReturnToStart()
    {
        //this.gameObject.transform.position = this.path[0].gameObject.transform.position;
        this.gameObject.transform.position = this.gridManager.GetPosFromCoords(this.pathFinder.PathStart);
    }

    private void FindPath() // Change it to RecalculatePath(bool shouldPathBeReset) for dynamic changing of path
    {
        // This is for dynamic pathinding
        /*Vector3Int coordinates = new Vector3Int();
        if (shouldPathBeReset)
        {
            coordinates = this.pathFinder.PathStart;
        }
        else
        {
            coordinates = this.gridManager.GetCoordsFromPos(this.gameObject.transform.position);
        }*/

        //StopAllCoroutines() // Necessary for dynamic Pathfinding

        this.path.Clear();
        this.pathFinder.ClearChosenPath();

        this.possiblePaths = this.pathFinder.FindPath();

        //this.path = this.pathFinder.FindPath(this.possiblePaths);

        //this.StartCoroutine(FollowPath()); //Necessary for dynamic pathfinding
    }

    private void AssignPath()
    {
        int pathDangerLevel = 0;

        foreach(List<Node> item in this.possiblePaths)
        {
            pathDangerLevel = 0;

            foreach(Node tile in item)
            {
                Waypoint curNodeWayp = this.gridManager.TileList.Find(x => x.name == tile.Coordinates.ToString()).GetComponent<Waypoint>();
                pathDangerLevel += curNodeWayp.DangerLevel;
            }

            this.pathsWithDangerLevel.Add(item, pathDangerLevel);
        }

        IOrderedEnumerable<KeyValuePair<List<Node>, int>> sortedPaths = this.pathsWithDangerLevel.OrderBy(x => x.Value).ThenBy(x => x.Key.Count);

        foreach(KeyValuePair<List<Node>, int> item in sortedPaths)
        {
            this.path = item.Key;
            pathsWithDangerLevel.Clear();
            break;
        }
    }

    private IEnumerator FollowPath()
    {
        //foreach (Waypoint item in path)
        for(int i = 1; i < this.path.Count; i++)
        {
            Vector3 startPos = this.gameObject.transform.position;
            //Vector3 finishPos = item.transform.position;
            Vector3 finishPos = this.gridManager.GetPosFromCoords(this.path[i].Coordinates);
            float travelPercent = 0f;

            this.gameObject.transform.LookAt(finishPos);

            //this.gameObject.transform.position = item.transform.position; // Used for grid static movement
            //yield return new WaitForSeconds(this.waitTime);

            while (travelPercent < 1f)
            {
                travelPercent += this.movementSpeed * Time.deltaTime;

                this.gameObject.transform.position = Vector3.Lerp(startPos, finishPos, travelPercent);

                yield return new WaitForEndOfFrame();
            }

        }

        FinishPath();
    }

    private void FinishPath()
    {
        this.gameObject.GetComponent<Enemy>().StealGold();
        this.gameObject.SetActive(false);
    }
}
