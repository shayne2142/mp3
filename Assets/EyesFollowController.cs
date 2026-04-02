using System.Collections;
using UnityEngine;

public class EyesFollowController : MonoBehaviour
{
    [Header("References")]
    public Transform leftEye;
    public Transform rightEye;
    public Transform target; // your controller transform

    [Header("Look Settings")]
    public float rotationSpeed = 12f;
    public Vector3 eyeForwardOffsetEuler;
    // Use this if the eye mesh faces the wrong way. Example: (0, 90, 0)

    [Header("Blink Settings")]
    public Renderer leftEyeRenderer;
    public Renderer rightEyeRenderer;
    public float minBlinkDelay = 2f;
    public float maxBlinkDelay = 5f;
    public float blinkDuration = 0.08f;

    private Quaternion eyeForwardOffset;

    void Start()
    {
        eyeForwardOffset = Quaternion.Euler(eyeForwardOffsetEuler);

        if (leftEyeRenderer == null && leftEye != null)
            leftEyeRenderer = leftEye.GetComponent<Renderer>();

        if (rightEyeRenderer == null && rightEye != null)
            rightEyeRenderer = rightEye.GetComponent<Renderer>();

        StartCoroutine(BlinkLoop());
    }

    void Update()
    {
        if (target == null) return;

        RotateEyeToward(leftEye);
        RotateEyeToward(rightEye);
    }

    void RotateEyeToward(Transform eye)
    {
        if (eye == null) return;

        Vector3 direction = target.position - eye.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up) * eyeForwardOffset;

        eye.rotation = Quaternion.Slerp(
            eye.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            float wait = Random.Range(minBlinkDelay, maxBlinkDelay);
            yield return new WaitForSeconds(wait);

            SetEyesVisible(false);
            yield return new WaitForSeconds(blinkDuration);
            SetEyesVisible(true);
        }
    }

    void SetEyesVisible(bool visible)
    {
        if (leftEyeRenderer != null)
            leftEyeRenderer.enabled = visible;

        if (rightEyeRenderer != null)
            rightEyeRenderer.enabled = visible;
    }
}