using UnityEngine;
public interface IUIManager
{
    void ShowWinAnimation();
    void ShowSolutionImage(Sprite solutionSprite);
    void UpdatePoints(int points);
}