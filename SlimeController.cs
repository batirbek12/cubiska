using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f; // Скорость движения смайла
    public float jumpForce = 5f; // Сила прыжка
    public int maxHits = 3; // Максимальное количество ударов, после которых смайл исчезнет

    private Rigidbody rb;
    private bool canJump = true; // Флаг, позволяющий смайлу выполнять прыжок
    private Vector3 moveDirection; // Направление движения смайла
    private float changeDirectionInterval = 2f; // Интервал смены направления
    private float lastDirectionChangeTime; // Время последней смены направления
    private int hits = 0; // Счетчик ударов

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Инициализируем начальное направление движения
        ChangeDirection();
    }

    private void Update()
    {
        // Проверяем, прошло ли достаточно времени для смены направления
        if (Time.time - lastDirectionChangeTime >= changeDirectionInterval)
        {
            ChangeDirection();
        }

        // Перемещаем смайла в текущем направлении
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition + moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Прыжок
        if (canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // При выполнении прыжка устанавливаем вертикальную скорость с положительным значением
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        // Запрещаем прыжок до следующего обновления
        canJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Включаем возможность прыгать после столкновения
        canJump = true;
    }

    private void ChangeDirection()
    {
        // Генерируем новое случайное направление
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveDirection = new Vector3(randomX, 0f, randomZ).normalized;

        // Обновляем время последней смены направления
        lastDirectionChangeTime = Time.time;

        // Поворачиваем смайла в сторону движения
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    private void OnMouseDown()
    {
        // Обработка нажатия левой кнопки мышки
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        // Увеличиваем счетчик ударов
        hits++;

        if (hits >= maxHits)
        {
            // Если количество ударов достигло максимального значения, уничтожаем смайла
            Destroy(gameObject);
        }
    }
}
