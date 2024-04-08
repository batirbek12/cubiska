using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    public float sensitivity = 2.0f; // Чувствительность мыши
    public Transform player; // Ссылка на объект "Steve"

    private float rotationX = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Захватываем курсор
        Cursor.visible = false; // Скрываем курсор
    }

    private void Update()
    {
        // Получаем ввод с мыши
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Поворачиваем камеру вокруг осей
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f); // Ограничиваем угол обзора

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }
}
