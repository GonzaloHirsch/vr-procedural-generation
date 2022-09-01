using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Teleporting stuff
    [Header("Teleporting")]
    public float maxDistance = 50;
    public float absoluteTeleportDistance = 25f;
    private Vector3 initialPosition;
    // Reference to the teleport marker
    public GameObject teleportMarker = null;
    [Header("Camera")]
    // Keep track of the camera offset in order to account for it in the raycast
    public GameObject cameraOffset = null;
    // Reference to the gazed at object
    private GameObject gazedAtObject = null;
    // Keeping track of the marker state
    private bool markerEnabled = false;
    // Save up computation of the next position
    private Vector3 nextPosition = Vector3.zero;
    // Raycast mask, only checks for collisions on the given layers, saves up computations
    private int raycastMask;

    void Awake() {
        // Create mask by setting the explicit tags
        this.raycastMask = LayerMask.GetMask(Constants.INTERACTABLE_TAG, Constants.TELEPORT_TAG, Constants.PROP_TAG);
    }

    void Start() {
        // Adjust initial position to be on top of the ground
        this.AdjustInitialPosition();
        // Store the initial position
        this.initialPosition = this.transform.position;
    }

    void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed at
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, this.raycastMask))
        {
            // GameObject detected in front of the camera.
            if (gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                // if (this.canSendGazeEvents()) gazedAtObject?.SendMessage("OnPointerExit");
                gazedAtObject = hit.transform.gameObject;
                // if (this.canSendGazeEvents()) gazedAtObject.SendMessage("OnPointerEnter");
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            // if (this.canSendGazeEvents()) gazedAtObject?.SendMessage("OnPointerExit");
            gazedAtObject = null;
        }

        // Set marker enabled only if the location can be teleported to
        this.markerEnabled = gazedAtObject != null && Vector3.Distance(hit.point, this.initialPosition) <= this.absoluteTeleportDistance && gazedAtObject.CompareTag(Constants.TELEPORT_TAG);
        this.teleportMarker.SetActive(this.markerEnabled);  // Activate the teleport marker only if the marker should be enabled
        // If the marker is enabled, calculate the position and set it
        if (this.markerEnabled) {
            this.nextPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            this.teleportMarker.transform.position = this.nextPosition;
            this.teleportMarker.transform.up = hit.normal;
        }

        // Checks for screen touches.
        if (ActionMapper.GetClick())
        {
            // If the trigger is pressed while looking at a teleport-enabled place
            // If marker is enabled, it's because the position is valid so it can teleport
            if (this.markerEnabled) {
                this.transform.position = this.nextPosition;    // Use precomputed position from marker
            } else {
                gazedAtObject?.SendMessage("OnPointerClick");
            }
        }
    }

    // Cannot send gaze events to teleportable surfaces
    // Check that the target object is not teleportable
    private bool canSendGazeEvents() {
        return gazedAtObject != null && !gazedAtObject.CompareTag(Constants.TELEPORT_TAG);
    }

    private void AdjustInitialPosition() {
        // Throw ray up and down to find the position of the ground
        bool located = false;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
        {
            this.transform.position = hit.point;
            located = true;
        }
        if (!located && Physics.Raycast(this.transform.position, Vector3.up, out hit, 100f)) {
            this.transform.position = hit.point;
        }
    }

    // Debug, dibuja un rayo en la dirección donde está mirando la cámara
    //void OnDrawGizmos()
    //{
    //    Debug.DrawRay(this.transform.position + this.cameraOffset.transform.localPosition, Camera.main.transform.forward, Color.red, maxDistance);
    //}
}
