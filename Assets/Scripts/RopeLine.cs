using UnityEngine;

public class RopeLine : MonoBehaviour, IRopeLine
{
    private IRopeNode _nodeA, _nodeB;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Material _greenLineMaterial;
    [SerializeField] private Material _redLineMaterial;
    private Color _color = Color.green;
    [SerializeField] private float _lineWidth = 0.2f;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lineRenderer == null)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
        _lineRenderer.useWorldSpace = true;
    }

    public void Initialize(IRopeNode nodeA, IRopeNode nodeB)
    {
        _nodeA = nodeA;
        _nodeB = nodeB;
        UpdateLinePosition();
        SetColor(Color.green);
    }

    public void UpdateLinePosition()
    {
        if (_nodeA != null && _nodeB != null)
        {
            _lineRenderer.SetPosition(0, _nodeA.Position);
            _lineRenderer.SetPosition(1, _nodeB.Position);
        }
    }

    public void SetColor(Color color)
    {
        _color = color;
        UpdateLineColor();
    }

    private void UpdateLineColor()
    {
        if (_lineRenderer != null)
        {
            if (_color == Color.green)
            {
                _lineRenderer.material = _greenLineMaterial;
            }
            else if (_color == Color.red)
            {
                _lineRenderer.material = _redLineMaterial;
            }
        }
    }


    public bool CheckIntersection(IRopeLine other)
    {
        if (other is RopeLine otherLine)
        {
            if (_nodeA == null || _nodeB == null) return false;
            if (otherLine._nodeA == null || otherLine._nodeB == null) return false;
            Vector2 p1 = _nodeA.Position;
            Vector2 q1 = _nodeB.Position;
            Vector2 p2 = otherLine._nodeA.Position;
            Vector2 q2 = otherLine._nodeB.Position;
            float det = (q1.x - p1.x) * (q2.y - p2.y) - (q2.x - p2.x) * (q1.y - p1.y);
            if (det == 0) return false;
            float lambda = ((q2.y - p2.y) * (q2.x - p1.x) + (p2.x - q2.x) * (q2.y - p1.y)) / det;
            float mu = ((p1.y - q1.y) * (q2.x - p1.x) + (q1.x - p1.x) * (q2.y - p1.y)) / det;
            return (lambda > 0 && lambda < 1 && mu > 0 && mu < 1);
        }
        else
        {
            return false;
        }
    }

    public Color GetColor()
    {
        return _color;
    }
}