using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 3f; // �������� �������� ������
    public float jumpForce = 5f; // ���� ������
    public int maxHits = 3; // ������������ ���������� ������, ����� ������� ����� ��������

    private Rigidbody rb;
    private bool canJump = true; // ����, ����������� ������ ��������� ������
    private Vector3 moveDirection; // ����������� �������� ������
    private float changeDirectionInterval = 2f; // �������� ����� �����������
    private float lastDirectionChangeTime; // ����� ��������� ����� �����������
    private int hits = 0; // ������� ������

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // �������������� ��������� ����������� ��������
        ChangeDirection();
    }

    private void Update()
    {
        // ���������, ������ �� ���������� ������� ��� ����� �����������
        if (Time.time - lastDirectionChangeTime >= changeDirectionInterval)
        {
            ChangeDirection();
        }

        // ���������� ������ � ������� �����������
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition + moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        // ������
        if (canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // ��� ���������� ������ ������������� ������������ �������� � ������������� ���������
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        // ��������� ������ �� ���������� ����������
        canJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �������� ����������� ������� ����� ������������
        canJump = true;
    }

    private void ChangeDirection()
    {
        // ���������� ����� ��������� �����������
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveDirection = new Vector3(randomX, 0f, randomZ).normalized;

        // ��������� ����� ��������� ����� �����������
        lastDirectionChangeTime = Time.time;

        // ������������ ������ � ������� ��������
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    private void OnMouseDown()
    {
        // ��������� ������� ����� ������ �����
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        // ����������� ������� ������
        hits++;

        if (hits >= maxHits)
        {
            // ���� ���������� ������ �������� ������������� ��������, ���������� ������
            Destroy(gameObject);
        }
    }
}
