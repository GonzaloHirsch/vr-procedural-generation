using UnityEngine;

public class TeleportManager : Framework.MonoBehaviorSingleton<TeleportManager>
{
    public GameObject[] markerParts;
    public Material treeMaterial;
    public Material walkMaterial;
    private MeshRenderer[] renderers;
    private Constants.PLAYER_MODES previousMode = Constants.PLAYER_MODES.TELEPORT;

    void Start() {
        // Allocate the array and get all the components
        this.renderers = this.GetComponentsInChildren<MeshRenderer>();
    }

    public void ChangePosition(Vector3 position, Vector3 normal) {
        this.gameObject.transform.position = position;
        this.gameObject.transform.up = normal;
    }

    public void ChangeMarkerMode(Constants.PLAYER_MODES mode) {
        // Avois multiple changes in the material
        if (this.previousMode == mode) return;
        switch (mode) {
            case Constants.PLAYER_MODES.TELEPORT:
                this.ChangeMaterial(this.walkMaterial);
                break;
            case Constants.PLAYER_MODES.TREE:
                this.ChangeMaterial(this.treeMaterial);
                break;
        }
        // Store the new mode used
        this.previousMode = mode;
    }

    private void ChangeMaterial(Material m) {
        foreach (Renderer r in this.renderers) {
            r.material = m;
        }
    }
}
