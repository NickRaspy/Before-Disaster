using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotationSpeed = 500;

    public Transform objectHolder;
    public GameObject HoldableObject { get; set; }

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new(-Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement);
        if (movement != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
