using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private int _maxExtraJumps =2, _currentExtraJumps;
    private float _jumpForce = 10;
    private bool _isGrounded;
    [SerializeField] private Transform _groundDetectPositionTransform;

    [SerializeField] private LayerMask ground;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundDetectPositionTransform.position, 0.4f, ground);
        rb.velocity = new Vector2(5, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && _currentExtraJumps > 0)
        {
            rb.velocity = Vector2.up * _jumpForce;
            _currentExtraJumps--;
        }
        else if(Input.GetButton("Fire1") && _isGrounded)
        {
            
        }
        
        
        if (_currentExtraJumps < _maxExtraJumps && _isGrounded)
        {
            _currentExtraJumps = _maxExtraJumps;
        }
    }
}
