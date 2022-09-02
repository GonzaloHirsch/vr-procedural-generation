using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Button : MonoBehaviour
{
    public float movementTime = 2f;
    public float movementDistance = 0.01f;
    private float intialTime;
    private Vector3 initialPosition;
    private bool isClicked = false;
    public UnityEvent onClick;
    private float timeDelta;
    private float newY;
    private float m;

    private void StoreInitialPosition() {
        this.initialPosition = this.transform.position;
    }

    // Handles when the pointer clicks the button
    public void OnPointerClick()
    {
        if (!this.isClicked)
        {
            this.StoreInitialPosition();
            this.isClicked = true;
            this.intialTime = Time.time;
            StartCoroutine(this.StopButton());
            if (onClick != null) onClick.Invoke();
        }
    }

    // Handle button movement
    void Update()
    {
        if (isClicked) this.HandleButtonMovement();
    }

    private void HandleButtonMovement()
    {
        this.m = this.movementDistance * -2f / this.movementTime;
        this.timeDelta = Time.time - this.intialTime;
        if (this.timeDelta - this.movementTime/2 > 0) {
            this.newY = -m * this.timeDelta - 2f * this.movementDistance;
        } else {
            this.newY = m * this.timeDelta;
        }
        this.transform.position = this.initialPosition + new Vector3(0, Mathf.Clamp(this.newY, -this.movementDistance, 0), 0);
    }

    private IEnumerator StopButton()
    {
        yield return new WaitForSeconds(this.movementTime);
        this.isClicked = false;
    }
}
