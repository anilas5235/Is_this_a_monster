using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private float _movementSpeedOfPlayer, _ownSpeed;
    [SerializeField] private float _maxGape;
    [SerializeField] private GameObject Player;
    private Player _playerScript;

    private Rigidbody2D _rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerScript = Player.GetComponent<Player>();
        _movementSpeedOfPlayer = _playerScript._characterSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = new Vector2(_ownSpeed, 0);

        float gape = Player.transform.position.x - transform.position.x ; 
        _ownSpeed = gape > _maxGape ? _movementSpeedOfPlayer : _movementSpeedOfPlayer * 0.95f;

        Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(5, 6), 0);
        if (col != null)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Time.timeScale = 0;
                Debug.Log("You died");
                //death screen trigger
            }
        }
        
    }
}
