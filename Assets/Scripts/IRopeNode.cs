using UnityEngine;

public interface IRopeNode
{
    Vector2 Position { get; }
    void SetPosition(Vector2 position);
    void StartDragging();
    void StopDragging();
    void ResetPosition();
}
