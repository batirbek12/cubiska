using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // Скорость движения
    public float jumpForce = 7.0f; // Сила прыжка
    public Transform cameraTransform; // Ссылка на трансформ камеры

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Получаем ввод с клавиатуры
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Рассчитываем направление движения относительно камеры
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 movement = (cameraForward * verticalInput + cameraTransform.right * horizontalInput) * speed;

        // Применяем движение к Rigidbody
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Прыжок
        if (Input.GetButtonDown("Jump"))
        {
            if (Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
