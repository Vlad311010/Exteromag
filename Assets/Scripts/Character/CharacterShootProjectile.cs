using UnityEngine;
using UnityEngine.InputSystem;
using Structs;
using Interfaces;

public class CharacterShootProjectile : MonoBehaviour
{
    [Header("Inputs")]
    DefaultControls control;

    [Header("Parameters")]
    public GameObject projectile;
    public GameObject projectileAlt;
    public float spawnPointOffset;
    public float power;


    [Header("Components")]
    private IAim aim;

    void OnEnable()
    {
        control = new DefaultControls();
        control.Enable();

        control.gameplay.Action.performed += OnAction;
        control.gameplay.ActionAlt.performed += OnActionAlt;
    }

    void OnDisable()
    {
        control.Disable();

        control.gameplay.Movement.performed -= OnAction;
        control.gameplay.ActionAlt.performed -= OnActionAlt;
    }

    void Start()
    {
        aim = GetComponentInChildren<IAim>();   
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        ShootProjectile(projectile, power);
    }

    private void OnActionAlt(InputAction.CallbackContext context)
    {
        ShootProjectile(projectileAlt, power);
    }

    private void ShootProjectile(GameObject projectile, float power)
    {
        AimSnapshot snapshot = aim.TakeSnapshot();
        Vector2 spawnPoint = transform.position + snapshot.castDirection * spawnPointOffset;
        GameObject projectileInstance = Instantiate(projectile, spawnPoint, Quaternion.LookRotation(new Vector3(0, 0, 1), snapshot.castDirection));
        projectileInstance.transform.rotation = Quaternion.Euler(0, 0, projectileInstance.transform.rotation.eulerAngles.z + 90);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(snapshot.castDirection * power, ForceMode2D.Impulse);
    }
}
