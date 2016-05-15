using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Camera mainCamera;

    float shakeTimer;
    float shakeAmount;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
            mainCamera.transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y);

            shakeTimer -= Time.deltaTime;
        } else
        {
            mainCamera.transform.position = new Vector3(0, 0);
        }
    }

    public void ShakeCamera(float ShakePower, float ShakeDuration)
    {
        shakeAmount = ShakePower;
        shakeTimer = ShakeDuration;
    }
}
