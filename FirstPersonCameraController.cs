using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    public float sensitivity = 2.0f; // ���������������� ����
    public Transform player; // ������ �� ������ "Steve"

    private float rotationX = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ����������� ������
        Cursor.visible = false; // �������� ������
    }

    private void Update()
    {
        // �������� ���� � ����
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // ������������ ������ ������ ����
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f); // ������������ ���� ������

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }
}
