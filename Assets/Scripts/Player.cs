using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D _ownCollider;
    private GameObject ownBody;
    public float _jumpForce;
    private bool _isGrounded, _isSliding = false, _slideStop = false,  _bodyVisible = true;
    public bool _stumbling = false;
    [SerializeField] private Transform _groundDetectPositionTransform;
    [SerializeField] private Animator _animator;

    [SerializeField] private LayerMask ground, obstacles;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ownBody = transform.GetChild(0).gameObject;
        _ownCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.TipsOn &&
            UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play) { return; }

        _isGrounded = Physics2D.OverlapCircle(_groundDetectPositionTransform.position, 0.3f, ground + obstacles);
        rb.velocity = rb.velocity.y < 0 ? new Vector2(0, rb.velocity.y -1* Time.deltaTime) : new Vector2(0, rb.velocity.y);

        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) && !_isSliding && _isGrounded)
        {
            rb.velocity = Vector2.up * _jumpForce;
        }
        else if(Input.GetButton("Fire2") && _isGrounded && !_slideStop && !_isSliding)
        {
            StartCoroutine( SlideLimitationTimer());
           _animator.SetBool("SlideButton",true);
           _isSliding = true;
        }
        else if((!Input.GetButton("Fire2") || _slideStop) && _isGrounded && _isSliding)
        {
            StopAllCoroutines(); StartCoroutine(SlideCoolDown());
            _animator.SetBool("SlideButton",false);
            _isSliding = false;
        }

        if (!(transform.position.x >= -0.3) && !_stumbling)
        {
            StartCoroutine(PlayerStumbles());
        }
    }

    private IEnumerator PlayerStumbles()
    {
        transform.position = new Vector3(0, -6.49f);
        _stumbling = true;
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(20, 10),0, obstacles);
        foreach (var t in cols)
        { t.isTrigger = true; }
        PlayerFlickering();
        yield return new WaitForSeconds(2f);
        _stumbling = false;
        foreach (var t in cols)
        { t.isTrigger = false; }
        ownBody.SetActive(true); _bodyVisible = true;
        
    }

    private void PlayerFlickering()
    {
        if (!_stumbling) { return;}
        switch (_bodyVisible)
        {
            case true: ownBody.SetActive(false); _bodyVisible = false; break;
            case false: ownBody.SetActive(true); _bodyVisible = true; break;
        }
        if (_stumbling) { Invoke("PlayerFlickering",0.15f); }
    }

    private IEnumerator SlideLimitationTimer()
    {
        yield return new WaitForSeconds(4f);
        _slideStop = true;
    }

    private IEnumerator SlideCoolDown()
    {
        yield return new WaitForSeconds(1f);
        _slideStop = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundDetectPositionTransform.position,0.3f);
    }
    
}
