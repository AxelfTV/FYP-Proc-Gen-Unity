using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PathGenerator
{
    
    public static List<int> GeneratePath(Vector3[] vertices, int startIndex, int endIndex, int xSize)
    {
        Node start = new Node();
        start.X = vertices[startIndex].x; start.Z = vertices[startIndex].z; start.Y = vertices[startIndex].y; start.Index = startIndex;

        Node end = new Node();
        end.X = vertices[endIndex].x; end.Y = vertices[endIndex].y; end.Z = vertices[endIndex].z; end.Index = endIndex;

        start.SetDistance(end.X, end.Z);

        var activeNodes = new List<Node>();
        activeNodes.Add(start);
        var visitedNodes = new List<Node>();

        while (activeNodes.Any())
        {
            var checkNode = activeNodes.OrderBy(x => x.CostDistance).First();
            if (checkNode.Index == end.Index)
            {
                Debug.Log("We are at the destination!");
                //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
                return GetPath(checkNode, startIndex);
            }
            visitedNodes.Add(checkNode);
            activeNodes.Remove(checkNode);

            var walkableNodes = GetWalkableNodes(vertices, checkNode, end, xSize);

            foreach (var walkableNode in walkableNodes)
            {
                //We have already visited this tile so we don't need to do so again!
                if (visitedNodes.Any(x => x.Index == walkableNode.Index))
                    continue;

                //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                if (activeNodes.Any(x => x.Index == walkableNode.Index))
                {
                    var existingNode = activeNodes.First(x => x.Index == walkableNode.Index);
                    if (existingNode.CostDistance > checkNode.CostDistance)
                    {
                        activeNodes.Remove(existingNode);
                        activeNodes.Add(walkableNode);
                    }
                }
                else
                {
                    //We've never seen this tile before so add it to the list. 
                    activeNodes.Add(walkableNode);
                }
            }
        }
        return new List<int>();
    }
    class Node
    {
        public float X;
        public float Y;
        public float Z;
        public int Index;

        public float Cost;
        public float Distance;

        public float CostDistance => Cost + Distance;
        public Node parent;
        
        public void SetDistance(float targetX, float targetZ)
        {
            this.Distance = Mathf.Sqrt((targetX - X) * (targetX - X) + (targetZ - Z) * (targetZ - Z));
        }
    }
    private static List<Node> GetWalkableNodes(Vector3[] vertices, Node currentNode, Node targetNode, int xSize)
    {
        float mult = 1;
        List<Node> nodes = new List<Node>();
        if(currentNode.Index + 1 < vertices.Length)
        {
            if (Mathf.Abs(vertices[currentNode.Index + 1].x - currentNode.X) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index + 1].x,
                Y = vertices[currentNode.Index + 1].y,
                Z = vertices[currentNode.Index + 1].z,
                Index = currentNode.Index + 1,
                parent = currentNode,
                Cost = Mathf.Abs(vertices[currentNode.Index + 1].y) * mult
            });
        }
        if(currentNode.Index - 1 >= 0)
        {
            if (Mathf.Abs(vertices[currentNode.Index - 1].x - currentNode.X) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index - 1].x,
                Y = vertices[currentNode.Index - 1].y,
                Z = vertices[currentNode.Index - 1].z,
                Index = currentNode.Index - 1,
                parent = currentNode,
                Cost = Mathf.Abs(vertices[currentNode.Index - 1].y) * mult
            });
        }
        if(currentNode.Index + xSize < vertices.Length)
        {
            if (Mathf.Abs(vertices[currentNode.Index + xSize].z - currentNode.Z) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index + xSize].x,
                Y = vertices[currentNode.Index + xSize].y,
                Z = vertices[currentNode.Index + xSize].z,
                Index = currentNode.Index + xSize,
                parent = currentNode,
                Cost = Mathf.Abs(vertices[currentNode.Index + xSize].y) * mult
            });
        }
        if (currentNode.Index - xSize >= 0)
        {
            if (Mathf.Abs(vertices[currentNode.Index - xSize].z - currentNode.Z) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index - xSize].x,
                Y = vertices[currentNode.Index - xSize].y,
                Z = vertices[currentNode.Index - xSize].z,
                Index = currentNode.Index - xSize,
                parent = currentNode,
                Cost = Mathf.Abs(vertices[currentNode.Index - xSize].y) * mult
            });
        }
        
        if (currentNode.Index - xSize - 1>= 0)
        {
            if (Mathf.Abs(vertices[currentNode.Index - xSize - 1].z - currentNode.Z) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index - xSize - 1].x,
                Y = vertices[currentNode.Index - xSize - 1].y,
                Z = vertices[currentNode.Index - xSize - 1].z,
                Index = currentNode.Index - xSize - 1,
                parent = currentNode,
                Cost = 1.414f * Mathf.Abs(vertices[currentNode.Index - xSize - 1].y) * mult
            });
        }        
        if (currentNode.Index - xSize + 1 >= 0)
        {
            if (Mathf.Abs(vertices[currentNode.Index - xSize + 1].z - currentNode.Z) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index - xSize + 1].x,
                Y = vertices[currentNode.Index - xSize + 1].y,
                Z = vertices[currentNode.Index - xSize + 1].z,
                Index = currentNode.Index - xSize + 1,
                parent = currentNode,
                Cost = 1.414f * Mathf.Abs(vertices[currentNode.Index - xSize + 1].y) * mult
            });
        }
        if (currentNode.Index + xSize + 1 < vertices.Length)
        {
            if (Mathf.Abs(vertices[currentNode.Index + xSize + 1].z - currentNode.Z) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index + xSize + 1].x,
                Y = vertices[currentNode.Index + xSize + 1].y,
                Z = vertices[currentNode.Index + xSize + 1].z,
                Index = currentNode.Index + xSize + 1,
                parent = currentNode,
                Cost = 1.414f * Mathf.Abs(vertices[currentNode.Index + xSize + 1].y) * mult
            });
        }
        if (currentNode.Index + xSize - 1 < vertices.Length)
        {
            if (Mathf.Abs(vertices[currentNode.Index + xSize - 1].z - currentNode.Z) < 2) nodes.Add(new Node
            {
                X = vertices[currentNode.Index + xSize - 1].x,
                Y = vertices[currentNode.Index + xSize - 1].y,
                Z = vertices[currentNode.Index + xSize - 1].z,
                Index = currentNode.Index + xSize - 1,
                parent = currentNode,
                Cost = 1.414f * Mathf.Abs(vertices[currentNode.Index + xSize - 1].y) * mult
            });
        }
        
        nodes.ForEach(node => node.SetDistance(targetNode.X, targetNode.Z));
        return nodes;
    }
    private static List<int> GetPath(Node activeNodes, int startIndex)
    {
        List<int> path = new List<int>();
        var checkNode = activeNodes;
        path.Add(checkNode.Index);
        while(checkNode.Index != startIndex)
        {
            path.Add(checkNode.parent.Index);
            checkNode = checkNode.parent;
        }
        return path;
    }
}
