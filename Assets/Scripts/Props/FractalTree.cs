using UnityEngine;
using System.Collections;


public class FractalTree : MonoBehaviour
{
    [Header("Probabilities")]
    public float spawnProbabilityY = 0.9f;
    public float spawnProbabilityZ = 0.7f;
    public float spawnProbabilityX = 0.7f;
    [Header("Animation")]
    public float waitTime = 0.5f;
    [Header("Depth")]
    public int maxDepth = 3;
    [Header("Scaling")]
    public Vector3 childScale = new Vector3(0.5f, 0.5f, 0.5f);
    public float positionScale = 0.33f;
    [Header("Visualization")]
    public Material material;


    void Start()
    {
        RunIteration(this.gameObject, new Vector3(0, 0.5f, 0), Vector3.zero, 0);
    }

    private void RunIteration(GameObject parent, Vector3 position, Vector3 rotation, int depth)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.parent = parent.transform;
        go.transform.localPosition = position * this.positionScale;
        go.transform.localRotation = Quaternion.Euler(rotation);
        go.transform.localScale = this.childScale;
        go.GetComponent<MeshRenderer>().material = this.material;
        go.GetComponent<BoxCollider>().enabled = false;    // Disable collider
        // Increase depth
        depth++;
        // Spawn children
        if (depth <= this.maxDepth)
        {
            // Spawn child if within probability
            if (Random.Range(0f, 1f) < this.spawnProbabilityY)
            {
                StartCoroutine(this.WaitForChild(this.waitTime, go, new Vector3(0, 2f, 0), Vector3.zero, depth));
            }
            if (Random.Range(0f, 1f) < this.spawnProbabilityZ)
            {
                StartCoroutine(this.WaitForChild(this.waitTime, go, new Vector3(0, 0, 2f), new Vector3(90, 0, 0), depth));
            }
            if (Random.Range(0f, 1f) < this.spawnProbabilityZ)
            {
                StartCoroutine(this.WaitForChild(this.waitTime, go, new Vector3(0, 0, -2f), new Vector3(-90, 0, 0), depth));
            }
            if (Random.Range(0f, 1f) < this.spawnProbabilityX)
            {
                StartCoroutine(this.WaitForChild(this.waitTime, go, new Vector3(2f, 0, 0), new Vector3(0, 0, 90), depth));
            }
            if (Random.Range(0f, 1f) < this.spawnProbabilityX)
            {
                StartCoroutine(this.WaitForChild(this.waitTime, go, new Vector3(-2f, 0, 0), new Vector3(0, 0, -90), depth));
            }
        }
    }

    IEnumerator WaitForChild(float time, GameObject parent, Vector3 position, Vector3 rotation, int depth)
    {
        yield return new WaitForSeconds(time);
        this.RunIteration(parent, position, rotation, depth);
    }
}
