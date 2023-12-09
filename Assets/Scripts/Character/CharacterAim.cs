using UnityEngine;
using Structs;
using Interfaces;

public class CharacterAim : MonoBehaviour, IAim
{
    private SpriteRenderer sprite;
    
    public Sprite aimSprite;

    private Vector3 castDirection;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = aimSprite;
    }


    private void Update()
    {
        Vector3 castPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        castPoint.z = transform.position.z;
        castDirection = (castPoint - transform.parent.position).normalized;

        // float castDistance = Vector3.Distance(transform.parent.position, castPoint);
        // castPoint = transform.parent.position + castDirection * castDistance;

        transform.position = castPoint;
        transform.rotation = Quaternion.identity;
    }

    public AimSnapshot TakeSnapshot()
    {
        return new AimSnapshot(transform.position, castDirection);
    }
}
