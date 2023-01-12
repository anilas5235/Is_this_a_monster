using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float _jumpForce;
    public bool _isGrounded, _isSliding = false, _slideStop = false;
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
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.TipsOn &&
            UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play) { return; }

        _isGrounded = Physics2D.OverlapCircle(_groundDetectPositionTransform.position, 0.3f, ground);
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
