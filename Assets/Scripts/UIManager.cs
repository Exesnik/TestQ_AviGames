using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class UIManager : MonoBehaviour, IUIManager
{
    [SerializeField] private GameObject _winAnimation;
    [SerializeField] private SpriteRenderer _solutionImage;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private Canvas _gameWinCanvas; // Add canvas variable
    [SerializeField] private float animationDuration = 3f; // Added animation duration

    private void Start()
    {
        _winAnimation.SetActive(false);
        _solutionImage.gameObject.SetActive(false);
        _gameWinCanvas.gameObject.SetActive(false); // Ensure canvas is inactive at start
    }
    public void ShowWinAnimation()
    {
        RopePuzzleManager.Instance.SetNodesDraggableByUIManager(false);
        _gameWinCanvas.gameObject.SetActive(true); // Activate the canvas
        _winAnimation.SetActive(true);
        _winAnimation.transform.localScale = Vector3.zero;
        _winAnimation.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        StartCoroutine(HideWinAnimationCoroutine());
    }

    private IEnumerator HideWinAnimationCoroutine()
    {
        yield return new WaitForSeconds(animationDuration);
        _gameWinCanvas.gameObject.SetActive(false); // Deactivate the canvas
        _winAnimation.SetActive(false);
        RopePuzzleManager.Instance.SetNodesDraggableByUIManager(true);
    }

    public void ShowSolutionImage(Sprite solutionSprite)
    {
        _solutionImage.gameObject.SetActive(true);
        _solutionImage.sprite = solutionSprite;
        Invoke("HideSolutionImage", 2);
    }

    public void HideSolutionImage()
    {
        _solutionImage.gameObject.SetActive(false);
    }
    public void UpdatePoints(int points)
    {
        _pointsText.text = $"Score: {points}";
    }
}