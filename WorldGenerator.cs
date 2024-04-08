using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject woodPrefab;
    public GameObject slimePrefab; // ������ ������
    public int maxSlimesPerChunk = 10;

    public GameObject stonePrefab;
    public GameObject leafPrefab;
    public int chunkSize = 100;
    public int chunkHeight = 5;
    public int worldSize = 2;
    public float tileSize = 1.0f;
    private Vector3 lastChunkPosition;
    private Transform playerTransform;
    private Dictionary<Vector3, bool> generatedChunks = new Dictionary<Vector3, bool>();
    private int minDistanceBetweenTrees = 5; // ����������� ���������� ����� ���������
    private int maxDistanceBetweenTrees = 10; // ������������ ���������� ����� ���������
    private int minTreesPerChunk = 4; // ����������� ���������� �������� � �����
    private int maxTreesPerChunk = 8; // ������������ ���������� �������� � �����

    private bool isGeneratingChunk = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateWorld();
    }

    void GenerateWorld()
    {
        Vector3 initialChunkPosition = new Vector3(
            Mathf.Floor(playerTransform.position.x / (chunkSize * tileSize)) * chunkSize * tileSize,
            0,
            Mathf.Floor(playerTransform.position.z / (chunkSize * tileSize)) * chunkSize * tileSize
        );

        lastChunkPosition = initialChunkPosition;

        for (int x = -worldSize; x <= worldSize; x++)
        {
            for (int z = -worldSize; z <= worldSize; z++)
            {
                Vector3 chunkPosition = initialChunkPosition + new Vector3(x * chunkSize * tileSize, 0, z * chunkSize * tileSize);
                GenerateChunk(chunkPosition);

                // ��������� ��������� ���������� ������� (�� 5 �� 10) ������ ������� �����
                int numSmiles = Random.Range(5, 11);
                for (int i = 0; i < numSmiles; i++)
                {
                    PlaceRandomSlime(chunkPosition);
                }
            }
        }
    }

    void PlaceRandomSlime(Vector3 chunkPosition)
    {
        // ��������� ��������� ������� ������ ����� ��� ���������� ������
        float randomX = Random.Range(chunkPosition.x, chunkPosition.x + chunkSize * tileSize);
        float randomZ = Random.Range(chunkPosition.z, chunkPosition.z + chunkSize * tileSize);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);

        // ������� ������ �� �������
        GameObject slime = Instantiate(slimePrefab, randomPosition, Quaternion.identity);
    }

    void GenerateChunk(Vector3 chunkPosition)
    {
        if (generatedChunks.ContainsKey(chunkPosition) && generatedChunks[chunkPosition])
        {
            return;
        }

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // ���������� ������ ����� � �������������� ���� �������
                float perlinValue = Mathf.PerlinNoise((chunkPosition.x + x) * 0.1f, (chunkPosition.z + z) * 0.1f);
                int height = Mathf.FloorToInt(perlinValue * chunkHeight);

                for (int y = 0; y <= height; y++)
                {
                    Vector3 spawnPosition = new Vector3(x * tileSize, y * tileSize, z * tileSize) + chunkPosition;

                    GameObject blockPrefab = GetBlockPrefabForHeight(y);
                    if (blockPrefab != null)
                    {
                        GameObject block = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
                    }
                }
            }
        }

        generatedChunks[chunkPosition] = true;
    }

    // ���������� ���� � ����������� �� ������
    GameObject GetBlockPrefabForHeight(int height)
    {
        if (height > 2)
        {
            return grassPrefab;
        }
        else if (height > 1)
        {
            return dirtPrefab;
        }
        else if (height > -5)
        {
            return stonePrefab; // ����������� stonePrefab ��� �����
        }
        else
        {
            return null; // ����� ����� �������� ������ ���� ������ ��� ������ �����
        }
    }

    bool IsNearChunkEdge(Vector3 currentChunkPosition, Vector3 playerPosition, float distance)
    {
        for (float x = currentChunkPosition.x - distance; x <= currentChunkPosition.x + distance; x += tileSize)
        {
            for (float z = currentChunkPosition.z - distance; z <= currentChunkPosition.z + distance; z += tileSize)
            {
                Vector3 checkPosition = new Vector3(x, 0, z);
                if (!IsBlockAtPosition(checkPosition))
                {
                    // ���� ������ ������ ���� � �������, �� ����� ��������� ����� � ���� �����
                    return true;
                }
            }
        }
        return false;
    }

    // �������� ������� ����� � �������� �������
    bool IsBlockAtPosition(Vector3 position)
    {
        // �������� ���� � ��������� �������
        GameObject block = GetBlockAtPosition(position);

        // ���������, �������� �� ���� ����� �� ����� ������ (grass, dirt, wood, leaf)
        if (block == grassPrefab || block == dirtPrefab || block == woodPrefab || block == leafPrefab)
        {
            return true; // ���� ������
        }

        return false; // ��� ����� � ������ �������
    }

    GameObject GetBlockAtPosition(Vector3 position)
    {
        // ����� ��� ����� ����������� ������ ��� ����������� ����� � ������ �������.
        // ��������, ����� ������������ Raycast ��� ����������� ����� � �������.

        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f))
        {
            return hit.collider.gameObject;
        }

        return null; // ���� ���� �� ������
    }

    private void Update()
    {
        Vector3 playerPosition = playerTransform.position;

        // ������������ ������� ��������� ����� ������
        Vector3 currentChunkPosition = new Vector3(
            Mathf.Floor(playerPosition.x / (chunkSize * tileSize)) * chunkSize * tileSize,
            0,
            Mathf.Floor(playerPosition.z / (chunkSize * tileSize)) * chunkSize * tileSize
        );

        // ���������, ���� �� ����� � ������� 10 ������ �� ������
        bool isNearChunkEdge = IsNearChunkEdge(currentChunkPosition, playerPosition, 10);

        // ���������, �� ����������� �� ��� ��������� �����
        if (isNearChunkEdge && !isGeneratingChunk)
        {
            GenerateChunk(currentChunkPosition);
            isGeneratingChunk = true;
        }

        // ���� ����� ������� �� ����� ����, ���������� ���� ���������
        if (currentChunkPosition != lastChunkPosition)
        {
            isGeneratingChunk = false;
            lastChunkPosition = currentChunkPosition;
        }
    }
}
