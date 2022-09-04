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
    private TeleportManager teleportManager;
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
    [Header("Modes")]
    private Constants.PLAYER_MODES currentMode = Constants.PLAYER_MODES.TELEPORT;
    [Header("Initial Position")]
    public bool adjustInitialPosition = true;

    void Awake()
    {
        // Create mask by setting the explicit tags
        this.raycastMask = LayerMask.GetMask(Constants.INTERACTABLE_TAG, Constants.TELEPORT_TAG, Constants.PROP_TAG, Constants.UI_TAG);
        // Get the teleport marker and the component
        this.teleportMarker = GameObject.FindGameObjectWithTag(Constants.TELEPORT_PROP_TAG);
        this.teleportManager = this.teleportMarker?.GetComponent<TeleportManager>();
    }

    void Start()
    {
        // Adjust initial position to be on top of the ground
        if (this.adjustInitialPosition) this.AdjustInitialPosition();
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
        // Trees can only be planted on teleportable surfaces
        this.markerEnabled = gazedAtObject != null && gazedAtObject.CompareTag(Constants.TELEPORT_TAG);
        if (this.currentMode == Constants.PLAYER_MODES.TELEPORT)
        {
            this.markerEnabled = this.markerEnabled && Vector3.Distance(hit.point, this.initialPosition) <= this.absoluteTeleportDistance;
        }
        if (this.teleportMarker != null) this.teleportMarker.SetActive(this.markerEnabled);  // Activate the teleport marker only if the marker should be enabled
        // If the marker is enabled, calculate the position and set it
        if (this.markerEnabled)
        {
            this.nextPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            if (this.teleportMarker != null && this.teleportManager != null)
            {
                // Set the position
                this.teleportManager.ChangePosition(this.nextPosition, hit.normal);
                // Set the material
                this.teleportManager.ChangeMarkerMode(this.currentMode);
            }
        }

        // Checks for screen touches.
        if (ActionMapper.GetClick())
        {
            // If the trigger is pressed while looking at a teleport-enabled place
            // If marker is enabled, it's because the position is valid so it can teleport
            if (this.markerEnabled)
            {
                switch (this.currentMode)
                {
                    case Constants.PLAYER_MODES.TELEPORT:
                        this.transform.position = this.nextPosition;    // Use precomputed position from marker
                        break;
                    case Constants.PLAYER_MODES.TREE:
                        GameObject newTree = GameObject.Instantiate(TreeManager.Instance.GetTreeToPlant(), this.nextPosition, Quaternion.identity);
                        newTree.transform.up = hit.normal;
                        break;
                }
            }
            else
            {
                // Send options to not require receiver just in case, to avoid the error
                gazedAtObject?.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    // Cannot send gaze events to teleportable surfaces
    // Check that the target object is not teleportable
    private bool canSendGazeEvents()
    {
        return gazedAtObject != null && !gazedAtObject.CompareTag(Constants.TELEPORT_TAG);
    }

    private void AdjustInitialPosition()
    {
        // Throw ray up and down to find the position of the ground
        bool located = false;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
        {
            this.transform.position = hit.point;
            located = true;
        }
        if (!located && Physics.Raycast(this.transform.position, Vector3.up, out hit, 100f))
        {
            this.transform.position = hit.point;
        }
    }

    public void ChangePlayerMode(Constants.PLAYER_MODES mode)
    {
        this.currentMode = mode;
    }

    // Debug, dibuja un rayo en la dirección donde está mirando la cámara
    //void OnDrawGizmos()
    //{
    //    Debug.DrawRay(this.transform.position + this.cameraOffset.transform.localPosition, Camera.main.transform.forward, Color.red, maxDistance);
    //}
}
