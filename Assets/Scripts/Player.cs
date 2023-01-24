using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject ownBody;
    public float _jumpForce, _flickerNextTime;
    private bool _isGrounded, _isSliding = false, _slideStop = false,  _bodyVisible = true, _isJumping, _jumpTime;
    public bool _stumbling = false;
    [SerializeField] private Transform _groundDetectPositionTransform;
    [SerializeField] private Animator _animator;

    [SerializeField] private LayerMask ground, obstacles;

    private Coroutine _currentSlideTimer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ownBody = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play) { return; }

        _isGrounded = Physics2D.OverlapCircle(_groundDetectPositionTransform.position, 0.4f, ground + obstacles);
        rb.velocity = rb.velocity.y < 0 ? new Vector2(0, rb.velocity.y -10* Time.deltaTime) : new Vector2(0, rb.velocity.y);

        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) && !_isSliding && _isGrounded && !_isJumping)
        {
            rb.velocity = Vector2.up * _jumpForce;
            _isJumping = true;
            _animator.SetBool("Jump", true);
            _jumpTime = true; Invoke("SetJumpTimeFalse",1f);
        }
        else if(_isGrounded && _isJumping && !_jumpTime)
        {
            _isJumping = false;
            _animator.SetBool("Jump",false);
        }
        
        if(Input.GetButton("Fire2") && _isGrounded && !_slideStop&& !_isSliding && _currentSlideTimer == null)
        {
            _currentSlideTimer= StartCoroutine( SlideLimitationTimer());
           _animator.SetBool("SlideButton",true);
           _isSliding = true;
        }
        else if((!Input.GetButton("Fire2") || _slideStop) && _isGrounded && _isSliding)
        {
            if(_currentSlideTimer != null){ StopCoroutine(_currentSlideTimer); _currentSlideTimer = null;}
            StartCoroutine(SlideCoolDown());
            _animator.SetBool("SlideButton",false);
            _isSliding = false;
        }

        if (!(transform.position.x >= -0.3) && !_stumbling)
        {
            StopCoroutine(PlayerStumbles());
            StartCoroutine(PlayerStumbles());
        }
        if (_stumbling && Time.time > _flickerNextTime)
        {
             switch (_bodyVisible)
                    {
                        case true: ownBody.SetActive(false); _bodyVisible = false; break;
                        case false: ownBody.SetActive(true); _bodyVisible = true; break;
                    }
             _flickerNextTime += 0.1f;
        }
    }

    private IEnumerator PlayerStumbles()
    {
        _stumbling = true;
        Monster.Instance.MonsterCloseIn();
        transform.position = new Vector3(0, transform.position.y);
        _flickerNextTime = Time.time + 0.1f;
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(40, 10),0, obstacles);
        foreach (var t in cols)
        { t.enabled = false; }
        yield return new WaitForSeconds(2f);
        foreach (var t in cols)
        { if(t != null) { t.enabled = true;} }
        ownBody.SetActive(true); _bodyVisible = true;
        _stumbling = false;
        
    }
    private IEnumerator SlideLimitationTimer()
    {
        yield return new WaitForSeconds(4f);
        _currentSlideTimer = null;
        _slideStop = true;
    }

    private IEnumerator SlideCoolDown()
    {
        yield return new WaitForSeconds(1f);
        _slideStop = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundDetectPositionTransform.position,0.4f);
    }

    private void SetJumpTimeFalse()
    {
        _jumpTime = false;
    }
}
