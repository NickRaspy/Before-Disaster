using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Inject] GameManager gameManager;

    [Header("Parameters")]
    [SerializeField] private float normalSpeed = 5;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float rotationSpeed = 500;

    [Header("Player Objects")]
    public Transform objectHolder;
    public Transform bigObjectHolder;

    [Header("Camera")]
    [SerializeField] private Transform followingCamera;
    [SerializeField] private Vector3 offset;

    [Header("Animation")]
    [SerializeField] private string bigObjectHoldBoolName;

    public GameObject HoldableObject { get; set; }

    public Interactable Interactable { get; set; }

    public Animator Animator { get; set; }

    public bool CanMove { get; set; }

    private bool isRunning;
    private bool IsRunning
    {
        set 
        { 
            isRunning = value;
            Animator.SetBool("IsRunning", isRunning);
        }
    }

    private Rigidbody rb;
    private Vector3 inputVector;

    private Vector3 InputVector
    {
        get { return inputVector; }
        set
        {
            inputVector = value;
            Animator.SetBool("IsWalking", value != Vector3.zero);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        gameManager.players.Add(this);
    }

    void Update()
    {
        if (!CanMove)
        {
            InputVector = Vector3.zero;
            return;
        }
        GatherInput();
        Look();
        CameraFollow();
    }
    void FixedUpdate()
    {
        if(CanMove) Move();
    }
    private void GatherInput() => InputVector = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

    private void Look()
    {
        if (InputVector == Vector3.zero) return;

        var rot = Quaternion.LookRotation(InputVector.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }

    private void Move() => rb.MovePosition(transform.position + InputVector.normalized.magnitude * (isRunning ? runSpeed : normalSpeed) * Time.deltaTime * transform.forward);

    private void CameraFollow() => followingCamera.position = transform.position + offset;

    public void GiveObject(GameObject givableObject, bool isBig)
    {
        Instantiate(givableObject, isBig ? bigObjectHolder : objectHolder);
        HoldableObject = givableObject;
    }
    public void RemoveObject()
    {
        if(objectHolder.childCount > 0) Destroy(objectHolder.transform.GetChild(0).gameObject);
        else if(bigObjectHolder.childCount > 0) Destroy(bigObjectHolder.transform.GetChild(0).gameObject);
        HoldableObject = null;
        SetBigObjectMode(false);
    }
    public void SetBigObjectMode(bool isHavingBigObject) => Animator.SetBool(bigObjectHoldBoolName, isHavingBigObject);
    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.Space:
                    if(Interactable != null && Interactable.InteractingPlayer == null)
                    {
                        Interactable.InteractingPlayer = this;
                        Interactable.Interact();
                    }
                    break;
                case KeyCode.LeftShift: 
                case KeyCode.RightShift:
                    IsRunning = true;
                    break;
            }
        }
        if(Event.current.type == EventType.KeyUp)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.LeftShift:
                case KeyCode.RightShift:
                    IsRunning = false;
                    break;
            }
        }
    }
}
public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
