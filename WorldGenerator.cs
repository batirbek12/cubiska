using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject woodPrefab;
    public GameObject slimePrefab; // Префаб смайла
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
    private int minDistanceBetweenTrees = 5; // Минимальное расстояние между деревьями
    private int maxDistanceBetweenTrees = 10; // Максимальное расстояние между деревьями
    private int minTreesPerChunk = 4; // Минимальное количество деревьев в чанке
    private int maxTreesPerChunk = 8; // Максимальное количество деревьев в чанке

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

                // Размещаем случайное количество смайлов (от 5 до 10) внутри каждого чанка
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
        // Вычисляем случайную позицию внутри чанка для размещения смайла
        float randomX = Random.Range(chunkPosition.x, chunkPosition.x + chunkSize * tileSize);
        float randomZ = Random.Range(chunkPosition.z, chunkPosition.z + chunkSize * tileSize);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);

        // Создаем смайла из префаба
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
                // Определите высоту блока с использованием шума Перлина
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

    // Определите блок в зависимости от высоты
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
            return stonePrefab; // Используйте stonePrefab для низин
        }
        else
        {
            return null; // Здесь можно добавить другие типы блоков для разных высот
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
                    // Если найден пустой блок в радиусе, то игрок находится ближе к краю чанка
                    return true;
                }
            }
        }
        return false;
    }

    // Проверка наличия блока в заданной позиции
    bool IsBlockAtPosition(Vector3 position)
    {
        // Получите блок в указанной позиции
        GameObject block = GetBlockAtPosition(position);

        // Проверьте, является ли блок одним из типов блоков (grass, dirt, wood, leaf)
        if (block == grassPrefab || block == dirtPrefab || block == woodPrefab || block == leafPrefab)
        {
            return true; // Блок найден
        }

        return false; // Нет блока в данной позиции
    }

    GameObject GetBlockAtPosition(Vector3 position)
    {
        // Здесь вам нужно реализовать логику для определения блока в данной позиции.
        // Например, можно использовать Raycast для определения блока в позиции.

        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f))
        {
            return hit.collider.gameObject;
        }

        return null; // Если блок не найден
    }

    private void Update()
    {
        Vector3 playerPosition = playerTransform.position;

        // Рассчитываем текущее положение чанка игрока
        Vector3 currentChunkPosition = new Vector3(
            Mathf.Floor(playerPosition.x / (chunkSize * tileSize)) * chunkSize * tileSize,
            0,
            Mathf.Floor(playerPosition.z / (chunkSize * tileSize)) * chunkSize * tileSize
        );

        // Проверяем, есть ли блоки в радиусе 10 блоков от игрока
        bool isNearChunkEdge = IsNearChunkEdge(currentChunkPosition, playerPosition, 10);

        // Проверяем, не выполняется ли уже генерация чанка
        if (isNearChunkEdge && !isGeneratingChunk)
        {
            GenerateChunk(currentChunkPosition);
            isGeneratingChunk = true;
        }

        // Если игрок перешел на новый чанк, сбрасываем флаг генерации
        if (currentChunkPosition != lastChunkPosition)
        {
            isGeneratingChunk = false;
            lastChunkPosition = currentChunkPosition;
        }
    }
}
