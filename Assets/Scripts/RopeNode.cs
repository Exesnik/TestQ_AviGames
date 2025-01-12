using UnityEngine;

public class RopeNode : MonoBehaviour, IRopeNode
{
    [SerializeField] private Vector2 startPosition;
    private bool _isDragging;
    public Vector2 Position => transform.position;
    public AudioManager _audioManager { get; set; }
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color hoverColor = Color.yellow;
    private Color originalColor;
    [SerializeField] private GameObject _hoverSprite;
    private bool _isDragSoundPlaying = false; // Add this
    private bool _isDraggable = true; // Add this
    private void Awake()
    {
        startPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        originalColor = _spriteRenderer.color;
        if (_hoverSprite != null)
        {
            _hoverSprite.SetActive(false);
        }
    }
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void StartDragging()
    {
        if (!_isDraggable) return;
        _isDragging = true;
        _audioManager.PlayClickSound();
    }
    public void StopDragging()
    {
        _isDragging = false;
        _audioManager.StopDragSound();
        _isDragSoundPlaying = false; // Reset flag when dragging stops
    }
    public void ResetPosition()
    {
        transform.position = startPosition;
    }
    public void ShowHover()
    {
        _spriteRenderer.color = hoverColor;
        if (_hoverSprite != null)
        {
            _hoverSprite.SetActive(true);
        }
    }
    public void HideHover()
    {
        _spriteRenderer.color = originalColor;
        if (_hoverSprite != null)
        {
            _hoverSprite.SetActive(false);
        }
    }
    private void OnMouseEnter()
    {
        ShowHover();
    }
    private void OnMouseExit()
    {
        HideHover();
    }
    private void OnMouseDown()
    {
        StartDragging();
    }
    private void OnMouseUp()
    {
        StopDragging();
    }
    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
            if (!_isDragSoundPlaying)
            {
                _audioManager.PlayDragSound();
                _isDragSoundPlaying = true;
            }
            UpdateConnectedLines();
            RopePuzzleManager.Instance.CheckRopeIntersections();
        }
    }
    private void UpdateConnectedLines()
    {
        RopePuzzleManager.Instance.UpdateLines(this);
    }
    public void SetDraggable(bool isDraggable)
    {
        _isDraggable = isDraggable;
    }
}