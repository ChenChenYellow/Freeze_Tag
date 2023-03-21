using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 1.0f;
    [SerializeField] private bool RootMotion = false;
    private Animator animator;
    private readonly static string AnimatorParameter_Forward = "Forward",
        AnimatorParameter_Right = "Right";
    void Start()
    {
        if (RootMotion)
        {
            animator = GetComponent<Animator>();
        }

    }
    void Update()
    {
        if (RootMotion)
        {
            animator.SetFloat(AnimatorParameter_Forward, Input.GetAxis("Vertical"));
            animator.SetFloat(AnimatorParameter_Right, Input.GetAxis("Horizontal"));
            if (!Mathf.Approximately(Input.GetAxis("Mouse X"), 0f))
            { transform.Rotate(transform.up, Input.GetAxis("Mouse X")); }
        }
        else
        {
            Vector3 movement = Vector3.zero;
            if (!Mathf.Approximately(Input.GetAxis("Horizontal"), 0f))
            { movement += Input.GetAxis("Horizontal") * transform.right * MovementSpeed; }
            if (!Mathf.Approximately(Input.GetAxis("Vertical"), 0f))
            { movement += Input.GetAxis("Vertical") * transform.forward * MovementSpeed; }
            movement = Vector3.ClampMagnitude(movement, MovementSpeed);
            transform.position += movement;
            if (!Mathf.Approximately(Input.GetAxis("Mouse X"), 0f))
            { transform.Rotate(transform.up, Input.GetAxis("Mouse X")); }

        }
    }
}
