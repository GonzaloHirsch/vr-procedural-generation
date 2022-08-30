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
    [Header("Visualization")]
    public Material material;


    void Start()
    {
        RunIteration(this.gameObject, 0);
    }

    private void RunIteration(GameObject parent, int depth)
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
            wrapper.transform.localRotation = Quaternion.Euler(
                new Vector3(
                    Random.Range(this.rotationMin.x, this.rotationMax.x),
                    Random.Range(this.rotationMin.y, this.rotationMax.y),
                    Random.Range(this.rotationMin.z, this.rotationMax.z)));
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
            for (int i = 0; i < this.childLimit - depth; i++)
            {
                // Spawn child if within probability
                if (Random.Range(0f, 1f) < this.spawnProbability)
                {
                    StartCoroutine(this.WaitForChild(this.waitTime, wrapper, depth));
                }
            }
        }
    }

    IEnumerator WaitForChild(float time, GameObject parent, int depth)
    {
        yield return new WaitForSeconds(time);
        this.RunIteration(parent, depth);
    }
}
