using UnityEngine;

public class RandomAI : MonoBehaviour
{
    public float moveSpeed = 3.0f;

    private void Update()
    {
        // ���������� ��������� ����������� ��� ��������
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomZ = Random.Range(-1.0f, 1.0f);
        Vector3 moveDirection = new Vector3(randomX, 0.0f, randomZ).normalized;

        // ������� AI � �������� �����������
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
