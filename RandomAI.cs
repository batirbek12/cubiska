using UnityEngine;

public class RandomAI : MonoBehaviour
{
    public float moveSpeed = 3.0f;

    private void Update()
    {
        // Генерируем случайное направление для движения
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomZ = Random.Range(-1.0f, 1.0f);
        Vector3 moveDirection = new Vector3(randomX, 0.0f, randomZ).normalized;

        // Двигаем AI в заданном направлении
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
