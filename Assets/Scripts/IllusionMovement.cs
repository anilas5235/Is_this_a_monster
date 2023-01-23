using UnityEngine;

public class IllusionMovement : MonoBehaviour
{
    public float _levelSpeed;
    [SerializeField] private GameObject _resetGameObject;
    [SerializeField] private Camera _camera;
    private float _startXPosition, _spriteXExtents, _camaraExtentsX;
    private SpriteRenderer _spriteRenderer;
    private bool _hasReset = true, _hasCycleLimitation = false;
    [SerializeField] private bool endsWithSprite = true ,forDistanceMasure = false;
    public int amountOFCycles;
    private int _cycleCounter;

    private void Start()
    {
        if (_resetGameObject == null) { _hasReset = false; }
        if (endsWithSprite)
        {
            _spriteRenderer = _resetGameObject.GetComponentInChildren<SpriteRenderer>();
            _spriteXExtents = _spriteRenderer.bounds.extents.x;
        }
        _startXPosition = transform.position.x;
        _camaraExtentsX = _camera.aspect * _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.TipsOn &&
            UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play) { return; }
        if (amountOFCycles > 1 && !_hasCycleLimitation) { _hasCycleLimitation = true;}
        transform.position += new Vector3( _levelSpeed * -Time.deltaTime,0,0);
        if (forDistanceMasure) { UIManagerInGame.Instance.distanceRun += _levelSpeed * Time.deltaTime;}
        if (_hasReset)
        {
            if (_hasCycleLimitation && _cycleCounter >= amountOFCycles) {return; }
            if (_camera.transform.position.x - _camaraExtentsX > _resetGameObject.transform.position.x - _spriteXExtents)
            {
                transform.position = new Vector3(_startXPosition, transform.position.y, transform.position.z);
                _cycleCounter++;
            }
        }
    }
}
