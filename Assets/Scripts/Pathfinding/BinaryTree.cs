using System;
using UnityEngine;

public class BinaryTree {
    private BinaryNode root;
    public int totalNodes;

    public BinaryTree() {
        root = null;
    }

    public void AddNode(PathNode pathNode) {
        if (root == null) {
            root = new BinaryNode(pathNode, null, null);
            totalNodes++;
        }
        else {
            // Has root
            try {
                addNode(root, pathNode, pathNode.fValue);
            }
            catch (Exception e) {
                Debug.Log(e);
            }

            totalNodes++;
        }
    }

    private void addNode(BinaryNode node, PathNode pathNode, int pathFvalue) {
        if (pathFvalue <= node.pathNode.fValue) {
            // Left
            if (node.left == null) // Become left
                node.left = new BinaryNode(pathNode, null, null);
            else // Check left
                addNode(node.left, pathNode, pathFvalue);
        }
        else {
            // Right
            if (node.right == null) // Become right
                node.right = new BinaryNode(pathNode, null, null);
            else // Check right
                addNode(node.right, pathNode, pathFvalue);
        }
    }

    public void RemoveNode(PathNode pathNode) {
        if (root.pathNode == pathNode) {
            // It's the root
            var prevRoot = root;
            if (root.left == null && root.right == null) {
                root = null;
                // Tree is dead
            }
            else {
                // Atleast one has something
                if (root.left == null && root.right != null) {
                    // Has right but no left
                    root = root.right;
                }
                else {
                    if (root.right == null && root.left != null) {
                        // Has left but no right
                        root = root.left;
                    }
                    else {
                        // Has both right and left
                        if (root.left.right == null) {
                            // Root left has no right
                            root.left.right = root.right;
                            root = root.left;
                        }
                        else {
                            // Root left has a right
                            var leafRight = getLeafRight(root.left);
                            root = leafRight.right;
                            if (leafRight.right.left != null) // This leaf has a left
                                leafRight.right = leafRight.right.left;

                            leafRight.right = null;
                            root.left = leafRight;
                            root.right = prevRoot.right;
                        }
                    }
                }
            }
        }
        else {
            var pathFvalue = pathNode.fValue;
            removeNode(root, pathNode, pathFvalue);
        }

        totalNodes--;
    }

    private void removeNode(BinaryNode node, PathNode pathNode, int pathFvalue) {
        if (pathFvalue <= node.pathNode.fValue) {
            // Check left
            if (node.left != null) {
                if (node.left.pathNode == pathNode) {
                    var del = node.left;
                    if (del.left == null && del.right == null) {
                        // Both are null
                        node.left = null;
                    }
                    else {
                        // Atleast one has something
                        if (del.left == null && del.right != null) {
                            // Has right but no left
                            node.left = del.right;
                        }
                        else {
                            if (del.right == null && del.left != null) {
                                // Has left but no right
                                node.left = del.left;
                            }
                            else {
                                // Has both right and left
                                if (del.left.right == null) {
                                    // Root left has no right
                                    del.left.right = del.right;
                                    node.left = del.left;
                                }
                                else {
                                    // Root left has a right
                                    var leafRight = getLeafRight(del.left);
                                    node.left = leafRight.right;
                                    if (leafRight.right.left != null) // This leaf has a left
                                        leafRight.right = leafRight.right.left;

                                    leafRight.right = null;
                                    node.left.left = leafRight;
                                    node.left.right = del.right;
                                }
                            }
                        }
                    }
                }
                else {
                    // Not this left
                    // Check next one
                    removeNode(node.left, pathNode, pathFvalue);
                }
            }
        }
        else {
            // Check right
            if (node.right != null) {
                if (node.right.pathNode == pathNode) {
                    var del = node.right;
                    if (del.left == null && del.right == null) {
                        node.right = null;
                    }
                    else {
                        if (del.left == null && del.right != null) {
                            node.right = del.right;
                        }
                        else {
                            if (del.right == null && del.left != null) {
                                node.right = del.left;
                            }
                            else {
                                // Has both right and left
                                if (del.left.right == null) {
                                    // Root left has no right
                                    del.left.right = del.right;
                                    node.right = del.left;
                                }
                                else {
                                    // Root left has a right
                                    var leafRight = getLeafRight(del.left);
                                    node.right = leafRight.right;
                                    if (leafRight.right.left != null) // This leaf has a left
                                        leafRight.right = leafRight.right.left;

                                    leafRight.right = null;
                                    node.right.left = leafRight;
                                    node.right.right = del.right;
                                }
                            }
                        }
                    }
                }
                else {
                    // Not this right
                    // Check next one
                    removeNode(node.right, pathNode, pathFvalue);
                }
            }
        }
    }

    private BinaryNode getLeafRight(BinaryNode node) {
        if (node.right.right == null)
            return node;
        return getLeafRight(node.right);
    }

    public int getTotalHeight() {
        return getHeight(root);
    }

    private int getHeight(BinaryNode node) {
        if (node == null) return 0;
        return 1 + Mathf.Max(getHeight(node.left), getHeight(node.right));
    }

    public PathNode GetSmallest() {
        PathNode pathNode = null;
        var count = 0;
        try {
            pathNode = getSmallest(root, ref count);
        }
        catch {
            Debug.LogError("GetSmallest: " + count);
        }

        return pathNode;
    }

    private PathNode getSmallest(BinaryNode node, ref int count) {
        if (node == null) return null;
        if (node.left != null) {
            count++;
            return getSmallest(node.left, ref count);
        }

        // No more left nodes
        return node.pathNode;
    }

    private class BinaryNode {
        public BinaryNode left;
        public readonly PathNode pathNode;
        public BinaryNode right;

        public BinaryNode(PathNode _pathNode, BinaryNode _left, BinaryNode _right) {
            pathNode = _pathNode;
            left = _left;
            right = _right;
        }
    }
}