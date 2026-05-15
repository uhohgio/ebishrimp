using System.Collections;
using UnityEngine;

public class FakeShrimpForFridge : MonoBehaviour
{
    [Header("Fridge Settings")]
    public Animator fridgeAnimator; // Animator to control the fridge door animation
    public string fridgeOpenAnimationName = "FridgeOpen"; // The name of the opening animation
    public float animationDuration = 2.5f; // Total duration of the animation
    public float spawnIntervalMin = 3f; // Minimum time between spawns
    public float spawnIntervalMax = 6f; // Maximum time between spawns

    void Start()
    {
        StartCoroutine(SpawnShrimpRoutine());
    }

    IEnumerator SpawnShrimpRoutine()
    {
        while (true)
        {
            // Wait for a random interval
            // float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax); // old implementation
            float waitTime = randomizerTime(GameManager.Instance.difficulty); // new implementation (w/difficulty)
            yield return new WaitForSeconds(waitTime);

            // Open the fridge door
            if (fridgeAnimator != null)
            {
                fridgeAnimator.SetTrigger(fridgeOpenAnimationName);
            }

            // Wait for the animation to complete (assuming animationDuration matches its length)
            yield return new WaitForSeconds(animationDuration);

            // Wait for the remaining part of the animation (door closing)
            yield return new WaitForSeconds(animationDuration);
        }
    }

    float randomizerTime(int difficulty)
    {
        float waitTime;
        if (difficulty == 1) 
        {
            // easy
            waitTime = Random.Range(spawnIntervalMin+3.0f, spawnIntervalMax);
        } 
        else if (difficulty == 2)
        {
            // medium
            waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
        else if (difficulty == 3)
        {
            // hard
            waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax/2);
        }
        else if (difficulty == 4)
        {
            // x-mode (extreme)
            waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax/3);
        } 
        else
        {
            waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
        return waitTime;
    }


}