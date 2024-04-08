using UnityEngine;

public class BlockBreaker : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Ссылка на камеру игрока
    [SerializeField] private float maxDistance = 5f; // Максимальное расстояние для ломания блоков

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryBreakBlock();
        }
    }

    private void TryBreakBlock()
    {
        // Получаем позицию центра камеры
        Vector3 cameraCenter = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        // Создаем луч, направленный вперед от центра камеры
        Ray ray = new Ray(cameraCenter, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            // Проверяем, попали ли мы в блок с тегом "BreakableBlock"
            if (hit.collider.CompareTag("Block"))
            {
                // Удаляем объект, представляющий блок
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
