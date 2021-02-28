using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    public static GridPathfinding gridPathfinding;

    private void Start() {
        //Sound_Manager.Init();

        //FunctionPeriodic.Create(SpawnEnemy, 1.5f);
        //for (int i = 0; i < 1000; i++) SpawnEnemy();
        
        gridPathfinding = new GridPathfinding(new Vector3(-400, -400), new Vector3(400, 400), 5f);
        gridPathfinding.RaycastWalkable();
    }
}