using UnityEngine;

// This script requires thoses components and will be added if they aren't already there
[RequireComponent(typeof(Rigidbody))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 600.0f;

    [SerializeField]
    private float _jumpHeight = 2.0f;
    [SerializeField]
    private bool _canDoubleJump = false;
    private int _jumpCount = 0;

    private Inputs _currentInputs;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void UpdateInput(Inputs inputs)
    {
        if (_currentInputs.Jump)
        {
            inputs.Jump = true;
        }
        
        _currentInputs = inputs;
    }

    private void FixedUpdate()
    {
        float velocityX = .0f;

        if (Mathf.Abs(_currentInputs.Horizontal) > .01f)
        {
            velocityX = _currentInputs.Horizontal * _speed * Time.fixedDeltaTime;
        }

        float velocityY = _rigidbody.velocity.y;
        
        if (_currentInputs.Jump && (_jumpCount < 1 || (_canDoubleJump && _jumpCount <2)))
        {
            velocityY = Mathf.Sqrt(-2.0f * Physics.gravity.y * _jumpHeight);
            _jumpCount++;
        }

        _rigidbody.velocity = new Vector3(velocityX, velocityY, .0f);

        ResetNecessaryInputs();
    }

    private void ResetNecessaryInputs()
    {
        _currentInputs.Jump = false;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float speed)
    {
        if (speed >= .0f)
        {
            _speed = speed;
        }
    }

    public void AllowDoubleJump(bool allow)
    {
        _canDoubleJump = allow;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(Vector3.up, contact.normal) >= .9)
            {
                _jumpCount = 0;
                return;
            }
        }
    }
    
}
