using UnityEngine;

public class PathNode {

    public int gCost, hCost, fCost;
    public PathNode cameFromNode;
    
    private Grid<PathNode> _grid;
    private int _x, _y;
    public PathNode(Grid<PathNode> grid, int x, int y) {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public void CalculateFCost() => fCost = gCost + hCost;
    
    public override string ToString() {
        return $"{_x} , {_y}";
    }
}