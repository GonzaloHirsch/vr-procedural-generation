using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongFractalTree : MonoBehaviour
{
    [Header("Probabilities")]
    public float spawnProbability = 0.9f;
    [Header("Animation")]
    public float waitTime = 0.5f;
    [Header("Depth")]
    public int maxDepth = 3;
    [Header("Scaling")]
    public Vector3 childScale = new Vector3(0.3f, 1f, 0.3f);
    public int childLimit = 3;
    public float childScaleHeightReduction = 0.5f;
    public float childScaleWidthReduction = 0.1f;
    [Header("Rotation")]
    public Vector3 rotationMax = new Vector3(40f, 360f, 40f);
    public Vector3 rotationMin = new Vector3(-40f, 0f, -40f);
    [Header("Leaves")]
    public Vector3 leafScale = new Vector3(1, 1, 1);
    public bool eventEmitted = false;
    [Header("Visualization")]
    public Material material;


    void Start()
    {
        RunIteration(this.gameObject, Vector3.zero, 0);
    }

    private void RunIteration(GameObject parent, Vector3 rotation, int depth)
    {
        // Create the empty one
        GameObject wrapper = new GameObject("Wrapper");
        wrapper.transform.parent = parent.transform;
        wrapper.transform.localPosition = Vector3.zero;
        wrapper.transform.rotation = Quaternion.identity;
        wrapper.transform.localScale = Vector3.one;
        // Fixed rotation only for the base
        if (depth > 0)
        {
            wrapper.transform.localRotation = Quaternion.Euler(rotation);
            wrapper.transform.localPosition = new Vector3(0, this.childScale.y - this.childScaleHeightReduction * depth, 0);
        }

        // Create the child of the wrapper
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Branch";
        go.transform.parent = wrapper.transform;    // Empty is always parent of the object itself
        float newWidth = this.childScale.x - this.childScaleWidthReduction * depth;
        go.transform.localScale = new Vector3(newWidth, this.childScale.y - this.childScaleHeightReduction * depth, newWidth);
        go.transform.localPosition = new Vector3(0, go.transform.localScale.y / 2f, 0);
        // We need the rotation, otherwise it doesn't work
        go.transform.localRotation = Quaternion.identity;
        go.GetComponent<MeshRenderer>().material = this.material;
        go.GetComponent<BoxCollider>().enabled = false;    // Disable collider
        // Increase depth
        depth++;
        // Spawn children
        if (depth <= this.maxDepth)
        {
            for (int i = 0, n = this.childLimit - depth; i < n; i++)
            {
                // Spawn child if within probability
                if (Random.Range(0f, 1f) < this.spawnProbability)
                {
                    Vector3 newRotation = new Vector3(
                        Random.Range(this.rotationMin.x, this.rotationMax.x),
                        this.rotationMin.y + i * (this.rotationMax.y - this.rotationMin.y) / n,
                        Random.Range(this.rotationMin.z, this.rotationMax.z)
                    );
                    StartCoroutine(this.WaitForChild(this.waitTime, wrapper, newRotation, depth));
                }
            }
        } 
        // We spawn the leaves
        else {
            StartCoroutine(this.WaitForLeaf(this.waitTime, go));
        }
    }

    private void RunLeafIteration(GameObject parent)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Leaf";
        go.transform.parent = parent.transform;    // Empty is always parent of the object itself
        go.transform.localScale = this.leafScale;
        go.transform.localPosition = new Vector3(0, go.transform.localScale.y / 2f + parent.transform.localScale.y / 2f, 0);
        // We need the rotation, otherwise it doesn't work
        go.transform.localRotation = Quaternion.identity;
        go.GetComponent<MeshRenderer>().material = this.material;
        go.GetComponent<BoxCollider>().enabled = false;    // Disable collider
        if (!this.eventEmitted) {
            // Once the leaves are loaded, it means that the tree finished growing, we count then
            TreeManager.Instance.TreePlanted();
            this.eventEmitted = true;
        }
    }

    IEnumerator WaitForChild(float time, GameObject parent, Vector3 rotation, int depth)
    {
        yield return new WaitForSeconds(time);
        this.RunIteration(parent, rotation, depth);
    }
    
    IEnumerator WaitForLeaf(float time, GameObject parent)
    {
        yield return new WaitForSeconds(time);
        this.RunLeafIteration(parent);
    }
}
