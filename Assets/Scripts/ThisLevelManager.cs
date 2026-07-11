using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisLevelManager : MonoBehaviour
{

    [Header("Power Controls")]
    public bool powerOutage = false;
    public GameObject roomLights;
    public GameObject powerOnPrompt;
    public float outageIntervalMin = 15f; // Minimum time between outtages
    public float outageIntervalMax = 30f; // Maximum time between outtages
    public AudioManager audioManager;
    public AudioClip sound; // change to fit power on sound
    // public BreakerBox breakerbox;

    void Start()
    {
        StartCoroutine(CausePowerOutageRoutine());
    }

    void Update(){
        if (!powerOutage){
            roomLights.SetActive(true);
        }
    }

    public IEnumerator CausePowerOutageRoutine()
    {
        while (!powerOutage)
        {
            // Wait for a random interval
            // float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax); // old implementation
            float waitTime = randomizerTime(GameManager.Instance.difficulty); // new implementation (w/difficulty)
            // ^ in testing this gives an error because the GameManager does not exist when the game is in pieces
            yield return new WaitForSeconds(waitTime);

            powerOutage = true;
            Debug.Log("A power outage has occured.");

            roomLights.SetActive(false);
            powerOnPrompt.SetActive(true);

            audioManager.PlaySound(sound);
        }
    }

    float randomizerTime(int difficulty)
    {
        float waitTime;
        if (difficulty == 1) 
        {
            // easy
            waitTime = Random.Range(outageIntervalMin+(outageIntervalMin/3), outageIntervalMax);
        } 
        else if (difficulty == 2)
        {
            // medium
            waitTime = Random.Range(outageIntervalMin, outageIntervalMax);
        }
        else if (difficulty == 3)
        {
            // hard
            waitTime = Random.Range(outageIntervalMin, outageIntervalMax/2);
        }
        else if (difficulty == 4)
        {
            // x-mode (extreme)
            waitTime = Random.Range(outageIntervalMin, outageIntervalMax/3);
        } 
        else
        {
            waitTime = Random.Range(outageIntervalMin, outageIntervalMax);
        }
        return waitTime;
    }
}
