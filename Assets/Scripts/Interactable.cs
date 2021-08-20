using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 0.5f;

    Transform player;
    bool isFocus = false;
    bool hasInteracted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus && !hasInteracted) // if focused but has not interacted yet
        {
            // get distance to focused object
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= radius) // if within range
            {
                Interact(); // base interact function
                hasInteracted = true;
            }
        }
    }

    // debuging purposes
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact()
    {
        
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform; // gets player position
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null; // removes player transform from player
        hasInteracted = false;
    }
}
