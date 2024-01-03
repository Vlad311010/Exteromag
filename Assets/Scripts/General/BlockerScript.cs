using UnityEngine;

public class BlockerScript : MonoBehaviour
{
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;

    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color unactiveColor = Color.white;


    [SerializeField] int id = 0;
    [SerializeField] bool startState = false;
    [SerializeField] int enemiesCountToDeactivate = -1;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        collider.size = new Vector2(0.5f, spriteRenderer.size.y);
        if (startState)
            Activate(id);
        else
            Deactivate(id);

        GameEvents.current.onBlockerActivation += Activate;
        GameEvents.current.onEnemiesCountChange += DeactiveOnEnemiesCount;
    }

    

    public void Activate(int id)
    {
        if (id != this.id) return;

        spriteRenderer.color = activeColor;
        collider.enabled = true;
    }

    public void Deactivate(int id)
    {
        if (id != this.id) return;

        spriteRenderer.color = unactiveColor;
        collider.enabled = false;
    }

    private void DeactiveOnEnemiesCount(int enemiesCount)
    {
        if (enemiesCount <= enemiesCountToDeactivate)
            Deactivate(this.id);
    }
}
