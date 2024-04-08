using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // �������� ��������
    public float jumpForce = 7.0f; // ���� ������
    public Transform cameraTransform; // ������ �� ��������� ������

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �������� ���� � ����������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ������������ ����������� �������� ������������ ������
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 movement = (cameraForward * verticalInput + cameraTransform.right * horizontalInput) * speed;

        // ��������� �������� � Rigidbody
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // ������
        if (Input.GetButtonDown("Jump"))
        {
            if (Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
