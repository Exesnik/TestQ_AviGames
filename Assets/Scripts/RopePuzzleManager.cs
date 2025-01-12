using System.Collections.Generic;
using UnityEngine;

public class RopePuzzleManager : MonoBehaviour, IRopePuzzleManager
{
    public static RopePuzzleManager Instance { get; private set; }
    [SerializeField] private List<RopeNode> ropeNodes = new List<RopeNode>();
    [SerializeField] private List<RopeLine> ropeLines = new List<RopeLine>();
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private int points = 0;
    public GameObject ropeNodePrefab;
    public GameObject ropeLinePrefab;
    public Sprite solutionSprite;
    [SerializeField] private float circleRadius = 3f;
    [SerializeField] private int minNodes = 4;
    [SerializeField] private int maxNodes = 8;
    [SerializeField] private int pointsPerSolve = 350;
    private bool isSolved = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePuzzle();
        _uiManager.UpdatePoints(points);
        isSolved = false;
    }
    private void InitializePuzzle()
    {
        // Clear existing nodes and lines
        foreach (var node in ropeNodes)
        {
            Destroy(node.gameObject);
        }
        ropeNodes.Clear();
        foreach (var line in ropeLines)
        {
            Destroy(line.gameObject);
        }
        ropeLines.Clear();
        // Generate random number of nodes
        int nodeCount = Random.Range(minNodes, maxNodes + 1);
        // Initialize nodes on a circle
        for (int i = 0; i < nodeCount; i++)
        {
            float angle = i * (360f / nodeCount) * Mathf.Deg2Rad;
            float x = circleRadius * Mathf.Cos(angle);
            float y = circleRadius * Mathf.Sin(angle);
            Vector2 position = new Vector2(x, y);
            // Create node
            var node = Instantiate(ropeNodePrefab, position, Quaternion.identity).GetComponent<RopeNode>();
            ropeNodes.Add(node);
            node._audioManager = _audioManager;
        }
        // Initialize ropes with each node having exactly 2 connections
        if (nodeCount > 1)
        {
            for (int i = 0; i < nodeCount; i++)
            {
                int nodeAIndex = i;
                int nodeBIndex = (i + 1) % nodeCount; // Connect to the next node in the circle
                var line1 = Instantiate(ropeLinePrefab).GetComponent<RopeLine>();
                line1.Initialize(ropeNodes[nodeAIndex], ropeNodes[nodeBIndex]);
                ropeLines.Add(line1);
                if (nodeCount > 3) // prevent connecting to itself
                {
                    int nodeCIndex;
                    if (i < nodeCount - 2)
                    {
                        nodeCIndex = (i + 2) % nodeCount;
                    }
                    else
                    {
                        nodeCIndex = (i - 2 + nodeCount) % nodeCount;
                    }
                    var line2 = Instantiate(ropeLinePrefab).GetComponent<RopeLine>();
                    line2.Initialize(ropeNodes[nodeAIndex], ropeNodes[nodeCIndex]);
                    ropeLines.Add(line2);
                }
            }
        }
        isSolved = false;
        CheckRopeIntersections();
    }
    public void CheckRopeIntersections()
    {
        foreach (var rope in ropeLines)
        {
            bool intersects = false;
            foreach (var otherRope in ropeLines)
            {
                if (rope != otherRope && rope.CheckIntersection(otherRope))
                {
                    intersects = true;
                    break;
                }
            }
            if (intersects)
            {
                rope.SetColor(Color.red);
            }
            else
            {
                rope.SetColor(Color.green);
            }
        }
        if (CheckWinCondition() && !isSolved)
        {
            isSolved = true;
        }
    }
    private bool CheckWinCondition()
    {
        foreach (var line in ropeLines)
        {
            if (line.GetColor() != Color.green) return false;
        }
        return true;
    }
    public void SkipPuzzle()
    {
        if (isSolved)
        {
            _uiManager.ShowWinAnimation();
            AddPoints(pointsPerSolve);
        }
        else
        {
            _uiManager.ShowSolutionImage(solutionSprite);
        }


    }

    public void ResetPuzzle()
    {
        InitializePuzzle();
        foreach (var node in ropeNodes)
        {
            node.ResetPosition();
        }
        CheckRopeIntersections();
    }
    public void AddPoints(int points)
    {
        this.points += points;
        _uiManager.UpdatePoints(this.points);
    }
    public void UpdateLines(RopeNode node)
    {
        foreach (var line in ropeLines)
        {
            line.UpdateLinePosition();
        }
    }
    private void SetNodesDraggable(bool isDraggable)
    {
        foreach (var node in ropeNodes)
        {
            node.SetDraggable(isDraggable);
        }
    }
    public void SetNodesDraggableByUIManager(bool isDraggable)
    {
        SetNodesDraggable(isDraggable);
    }
}