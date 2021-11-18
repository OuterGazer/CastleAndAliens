using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] PossibleDirections possibleRoadDirections;
    public PossibleDirections PossibleRoadDirections => this.possibleRoadDirections;

    private int dangerLevel = 0;
    public int DangerLevel => this.dangerLevel;
    
    public void IncreaseDangerLevel(int dangerModifier)
    {
        this.dangerLevel += dangerModifier;
    }

    public void DecreaseDangerLevel(int dangerModifier)
    {
        this.dangerLevel -= dangerModifier;

        if (dangerLevel < 0)
            this.dangerLevel = 0;
    }
}
