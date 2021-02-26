﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    public Grid<PathNode> grid;
    private List<PathNode> _openList, _closedList;
    public Pathfinding(int width, int height) {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero,
            (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Grid<PathNode> GetGrid() => grid;

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        Debug.Log($"{startX} {startY} {endX} {endY}");
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        
        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }
        
        _openList = new List<PathNode>{ startNode };
        _closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        while (_openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(_openList);

            if (currentNode == endNode) {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (_closedList.Contains(neighbourNode)) continue;

               
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode)) {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        
        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }


    private PathNode GetNode(int x, int y) => grid.GetGridObject(x, y);
    
 
    
    private int CalculateDistanceCost(PathNode a, PathNode b) {
        Debug.Log($"CalculateDinstanceCost: a: {a}, b: {b}");
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];

        for (var i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}