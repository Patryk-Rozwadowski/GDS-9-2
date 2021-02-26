using UnityEngine;

public class PathNode {
    public int gCost, hCost, fCost;
    public PathNode cameFromNode;
    public int x, y;
    
    private Grid<PathNode> _grid;
    public PathNode(Grid<PathNode> grid, int x, int y) {
        _grid = grid;
        
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public override string ToString() {
        return $"{x} , {y}";
    }
}