using System.Collections;
using UnityEngine;

public class GooglyEyes : MonoBehaviour
{
    [Header("Tracking")]
    public Transform targetController; // The player's VR controller
    public float trackingSpeed = 8f;   // How fast the eyes track the controller

    [Header("Blinking")]
    public GameObject[] eyeMeshes;     // The actual eye models we turn off/on
    public float minBlinkWait = 2f;    // Minimum time between blinks
    public float maxBlinkWait = 6f;    // Maximum time between blinks
    public float blinkDuration = 0.15f; // How long the eyes disappear

    void Start()
    {
        // Start the continuous blinking loop as soon as the object wakes up
        StartCoroutine(BlinkRoutine());
    }

    void Update()
    {
        if (targetController != null)
        {
            // 1. Figure out the direction from the eyes to the controller
            Vector3 directionToTarget = targetController.position - transform.position;

            // 2. Figure out the exact mathematical rotation required to look that way
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // 3. Smoothly animate the current rotation towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * trackingSpeed);
        }
    }

    private IEnumerator BlinkRoutine()
    {
        // This loop will run forever as long as the GameObject is active
        while (true)
        {
            // Wait for a random number of seconds
            float randomWait = Random.Range(minBlinkWait, maxBlinkWait);
            yield return new WaitForSeconds(randomWait);

            // --- CLOSE EYES ---
            foreach (GameObject eye in eyeMeshes)
            {
                if (eye != null) eye.SetActive(false);
            }

            // Keep them closed for a fraction of a second
            yield return new WaitForSeconds(blinkDuration);

            // --- OPEN EYES ---
            foreach (GameObject eye in eyeMeshes)
            {
                if (eye != null) eye.SetActive(true);
            }

            // Note: Some people blink twice in a row! 
            // You could easily add a small random chance here to immediately blink again.
        }
    }
}