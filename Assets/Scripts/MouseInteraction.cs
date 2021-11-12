using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] GameObject basicTowerPrefab;

    [SerializeField] bool isPlaceable = default;
    public bool IsPlaceable => this.isPlaceable;
    [SerializeField] bool isRoadTile = default;

    private Bank bank;
    private GridManager gridManager;
    //private PathFinder pathFinder;
    Vector3Int tileCoordinates = new Vector3Int();

    private void Awake()
    {
        this.bank = GameObject.FindObjectOfType<Bank>();
        this.gridManager = GameObject.FindObjectOfType<GridManager>();
        //this.pathFinder = GameObject.FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        if(this.gridManager != null)
        {
            this.tileCoordinates = this.gridManager.GetCoordsFromPos(this.gameObject.transform.GetChild(0).position);

            if (this.isPlaceable || (!this.isPlaceable && !this.isRoadTile))
            {
                this.gridManager.BlockNode(this.tileCoordinates);
            }
        }
    }

    private void OnMouseDown()
    {
        if (this.isPlaceable && this.bank.CurrentBalance > 0) // if(this.gridManager[this.tileCoordinates].IsWalkable && !this.pathFinder.WillBlockPath(this.tileCoordinates))
        {
            //Debug.Log(this.gameObject.transform.position);
            Vector3 placementPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.2f, this.gameObject.transform.position.z);
            GameObject tower = Instantiate<GameObject>(this.basicTowerPrefab, placementPos, Quaternion.identity);
            
            this.isPlaceable = false;
            //this.gridManager.BlockNode(this.tileCoordinates);

            this.bank.Withdraw(tower.GetComponent<DefenseTower>().GoldCost);
        }        
    }
}
