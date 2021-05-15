using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5.0f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody)
        {
            _rigidbody.MovePosition(transform.position + Vector3.left * speed * Time.fixedDeltaTime);
        }
    }
}
