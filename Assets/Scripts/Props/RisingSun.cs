using UnityEngine;

public class RisingSun : Framework.MonoBehaviorSingleton<RisingSun>
{
    [Header("Movement")]
    public Vector3 initialPosition;
    public Vector3 finalPosition;
    private Vector3 targetPosition;
    private Vector3 movementDirection;
    public float movementSpeed;
    private bool isRising = false;
    public float positionDelta = 1f;

    void Start() {
        this.transform.position = this.initialPosition;
        this.movementDirection = (this.finalPosition - this.initialPosition).normalized;
    }

    void Update() {
        if (this.isRising) {
            this.transform.position = this.movementDirection * this.movementSpeed * Time.deltaTime + this.transform.position;
            // Reached position, stop rising
            this.isRising = !(Vector3.Distance(this.transform.position, this.targetPosition) < this.positionDelta);
        }
    }

    public void MoveSun(float completionPercentage){
        this.isRising = true;
        this.targetPosition = (this.finalPosition - this.initialPosition) * Mathf.Clamp(completionPercentage, 0f, 1f) + this.initialPosition;
    }
}
