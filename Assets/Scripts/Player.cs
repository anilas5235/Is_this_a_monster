using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public int _maxExtraJumps;
    private int _currentExtraJumps;
    public float _jumpForce;
    public bool _isGrounded, _isSliding = false;
    public float _characterSpeed;
    [SerializeField] private Transform _groundDetectPositionTransform;
    [SerializeField] private Animator _animator;

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
        rb.velocity = rb.velocity.y < 0 ? new Vector2(_characterSpeed, rb.velocity.y -5* Time.deltaTime) : new Vector2(_characterSpeed, rb.velocity.y);
        
        rb.velocity = new Vector2(_characterSpeed, rb.velocity.y);
        if ((Input.GetButtonDown("Jump")|| Input.GetButtonDown("Fire1")) && _currentExtraJumps > 0 &&!_isSliding)
        {
            rb.velocity = Vector2.up * _jumpForce;
            _currentExtraJumps--;
        }
        else if(Input.GetButtonDown("Fire2") && _isGrounded && !_isSliding)
        {
           StartCoroutine( Slide());
        }
        if (_currentExtraJumps < _maxExtraJumps && _isGrounded) { _currentExtraJumps = _maxExtraJumps; }
    }

    private IEnumerator Slide()
    {
        _isSliding = true;
        _animator.Play("Slide");
        yield return new WaitForSeconds(1.4f);
        _isSliding = false;
    }
}
