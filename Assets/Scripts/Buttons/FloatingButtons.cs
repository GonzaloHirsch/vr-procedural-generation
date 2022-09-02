using UnityEngine;

public class FloatingButtons : MonoBehaviour
{
    private GameObject player;
    public float deltaToPlayer = 2f;
    public float positionDelta = 0.5f;

    void Start()
    {
        this.player = this.FindPlayer();
    }

    void Update()
    {
        if (this.player != null && Vector3.Distance(this.player.transform.position, this.gameObject.transform.position) >= this.deltaToPlayer)
        {
            this.gameObject.transform.position = new Vector3(this.player.transform.position.x, this.player.transform.position.y + this.positionDelta, this.player.transform.position.z);
        }
        else
        {
            this.player = this.FindPlayer();
        }
    }

    private GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
    }
}
