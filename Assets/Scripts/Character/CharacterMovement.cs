using UnityEngine;

// This script requires thoses components and will be added if they aren't already there
[RequireComponent(typeof(Rigidbody))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private float _jumpHeight = 2.0f;

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
        if (Mathf.Abs(_currentInputs.Horizontal) > .01f)
        {
            //_rigidbody.AddForce(Vector3.right * _currentInputs.Horizontal * _speed * Time.deltaTime);
            //Debug.Log(Vector3.right * _currentInputs.Horizontal * _speed * Time.deltaTime);
        }

        float velocityX = _currentInputs.Horizontal * _speed * Time.fixedDeltaTime;
        float velocityY = _rigidbody.velocity.y;

        if (_currentInputs.Jump)
        {
            velocityY = Mathf.Sqrt(-2.0f * Physics.gravity.y * _jumpHeight);
        }

        _rigidbody.velocity = new Vector3(velocityX, velocityY, .0f);

        ResetNecessaryInputs();
    }

    private void ResetNecessaryInputs()
    {
        _currentInputs.Jump = false;
    }
}
