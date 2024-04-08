using UnityEngine;

public class BlockBreaker : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // ������ �� ������ ������
    [SerializeField] private float maxDistance = 5f; // ������������ ���������� ��� ������� ������

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryBreakBlock();
        }
    }

    private void TryBreakBlock()
    {
        // �������� ������� ������ ������
        Vector3 cameraCenter = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        // ������� ���, ������������ ������ �� ������ ������
        Ray ray = new Ray(cameraCenter, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            // ���������, ������ �� �� � ���� � ����� "BreakableBlock"
            if (hit.collider.CompareTag("Block"))
            {
                // ������� ������, �������������� ����
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
