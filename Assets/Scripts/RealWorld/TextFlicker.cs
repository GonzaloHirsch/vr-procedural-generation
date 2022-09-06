using UnityEngine;

public class TextFlicker : MonoBehaviour
{
    public float flickerTime = 1f;
    public GameObject text;
    private float currentTime = 0f;

    void Update() {
        this.currentTime += Time.deltaTime;
        if (this.currentTime >= this.flickerTime) {
            this.text.SetActive(!this.text.activeInHierarchy);
            this.currentTime = 0f;
        }
    }
}
