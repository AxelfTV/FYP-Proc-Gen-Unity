using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class NoisePathGenerator
{
    public static List<Vector2Int> GeneratePath(Vector2Int startPos, Vector2Int endPos)
    {
        Node start = new Node();
        start.position = startPos; start.height = MapManager.noiseMap.GetNoise(startPos.x, startPos.y);

        Node end = new Node();
        end.position = endPos; end.height = MapManager.noiseMap.GetNoise(endPos.x, endPos.y);

        start.SetDistance(end.position.x, end.position.y);

        var activeNodes = new List<Node>();
        activeNodes.Add(start);
        var visitedNodes = new List<Node>();

        while (activeNodes.Any())
        {
            var checkNode = activeNodes.OrderBy(x => x.CostDistance).First();
            if(checkNode.position == end.position)
            {
                Debug.Log("We are at the destination");
                return GetPath(checkNode, startPos);
            }
            visitedNodes.Add(checkNode);
            activeNodes.Remove(checkNode);

            var walkableNodes = GetWalkableNodes(checkNode, end);

            foreach(var walkableNode in walkableNodes)
            {
                if (visitedNodes.Any(x => x.position == walkableNode.position)) continue;

                if(activeNodes.Any(x => x.position == walkableNode.position))
                {
                    var existingNode = activeNodes.First(x => x.position == walkableNode.position);
                    if(existingNode.CostDistance > checkNode.CostDistance)
                    {
                        activeNodes.Remove(existingNode);
                        activeNodes.Add(walkableNode);
                    }

                }
                else
                {
                    activeNodes.Add(walkableNode);
                }
            }
        }
        return new List<Vector2Int>();
    }
    class Node
    {
        
        public float height;       
        public Vector2Int position;

        public float Cost;
        public float Distance;

        public float CostDistance => Cost + Distance;
        public Node parent;

        public void SetDistance(float targetX, float targetZ)
        {
            this.Distance = Mathf.Sqrt((targetX - position.x) * (targetX - position.x) + (targetZ - position.y) * (targetZ - position.y));
        }
    }
    private static List<Node> GetWalkableNodes( Node currentNode, Node targetNode)
    {
        float mult = 5;
        List<Node> nodes = new List<Node>();
        Vector2Int newPos = currentNode.position + Vector2Int.right;
        float newHeight = MapManager.noiseMap.GetNoise(newPos.x, newPos.y);
        nodes.Add(new Node
        {
            position = newPos,
            height = newHeight,
            parent = currentNode,
            Cost = Mathf.Abs(newHeight * mult)
        }); 
        newPos = currentNode.position+ Vector2Int.left;
        newHeight = MapManager.noiseMap.GetNoise(newPos.x, newPos.y);
        nodes.Add(new Node
        {
            position = newPos,
            height = newHeight,
            parent = currentNode,
            Cost = Mathf.Abs(newHeight * mult)
        });
        newPos = currentNode.position + Vector2Int.up;
        newHeight = MapManager.noiseMap.GetNoise(newPos.x, newPos.y);
        nodes.Add(new Node
        {
            position = newPos,
            height = newHeight,
            parent = currentNode,
            Cost = Mathf.Abs(newHeight * mult)
        });
        newPos = currentNode.position + Vector2Int.down;
        newHeight = MapManager.noiseMap.GetNoise(newPos.x, newPos.y);
        nodes.Add(new Node
        {
            position = newPos,
            height = newHeight,
            parent = currentNode,
            Cost = Mathf.Abs(newHeight * mult)
        });





        

        
        nodes.ForEach(node => node.SetDistance(targetNode.position.x, targetNode.position.y));
        return nodes;
    }
    private static List<Vector2Int> GetPath(Node activeNodes, Vector2Int startPos)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        var checkNode = activeNodes;
        path.Add(checkNode.position);
        while (checkNode.position != startPos)
        {
            path.Add(checkNode.parent.position);
            checkNode = checkNode.parent;
        }
        return path;
    }
}
