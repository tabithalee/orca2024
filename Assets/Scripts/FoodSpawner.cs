using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public int poolSize = 10;
    public float spawnRateMin = 2.0f;
    public float spawnRateMax = 3.0f;
    public float minSpawnHeight = -5.0f;
    public float maxSpawnHeight = 5.0f;
    private List<GameObject> foodPool;
    private bool isSpawning = true;
    // This flag controls spawning when we add game win or loose conditing set the falf to false and reset on restart the game
    private bool isGameRunning = true;

    void Start()
    {
        InitializePool();
        StartCoroutine(SpawnFood());
    }

    // Initialize the pool of food objects
    private void InitializePool()
    {
        foodPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject foodObject = Instantiate(foodPrefab, transform);
            foodObject.SetActive(false);
            foodPool.Add(foodObject);

            // Subscribe to the OnReturnToPool event
            FoodMovement foodMovement = foodObject.GetComponent<FoodMovement>();
            foodMovement.OnReturnToPool += ReturnFoodToPool;
        }
    }

    private IEnumerator SpawnFood()
    {
        while (isGameRunning)  // Check if the game is still running
        {
            float spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            yield return new WaitForSeconds(spawnRate);
            Spawn();
        }
    }

    private void Spawn()
    {
        // Find an inactive food object and activate it
        foreach (var food in foodPool)
        {
            if (!food.activeInHierarchy)
            {
                float spawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
                food.transform.position = new Vector3(0, spawnHeight, -1);
                food.GetComponent<FoodMovement>().isHeld = false;
                food.GetComponent<Collider>().enabled = true;
                food.SetActive(true);
                break;
            }
        }
    }
    // Get a food object from the pool
    private GameObject GetPooledFood()
    {
        foreach (var food in foodPool)
        {
            if (!food.activeInHierarchy)
            {
                return food;
            }
        }
        return null;  // No available food in the pool
    }

    // Return food to the pool
    public void ReturnFoodToPool(GameObject foodObject)
    {
        foodObject.SetActive(false);
    }

    // This function can be called when the game is won or lost
    public void StopSpawning()
    {
        isGameRunning = false;
        // Optionally, you can deactivate all currently active food
        foreach (var food in foodPool)
        {
            if (food.activeInHierarchy)
            {
                food.SetActive(false);
            }
        }
    }
}
