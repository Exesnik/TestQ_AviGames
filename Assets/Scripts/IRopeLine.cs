using UnityEngine;

public interface IRopeLine
{
    void Initialize(IRopeNode nodeA, IRopeNode nodeB);
    void UpdateLinePosition();
    void SetColor(Color color);
    bool CheckIntersection(IRopeLine other);
    Color GetColor();
}