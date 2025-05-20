using UnityEngine;
using UnityEngine.InputSystem;

public class NovaShipController : MonoBehaviour, NovaShipControls.IPlayerActions
{
    private NovaShipControls controls;
    private Vector2 moveInput;
    private Shot shotScript;
    private bool isFiring = false;
    
    private void Awake()
    {
        shotScript = GetComponent<Shot>();
        controls = new NovaShipControls();
        controls.Player.SetCallbacks(this); // Define os callbacks da interface
    }

    private void OnEnable()
    {
        controls.Player.Enable(); // Ativa o mapa de ações
    }

    private void OnDisable()
    {
        controls.Player.Disable(); // Desativa o mapa de ações
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        // Disparo contínuo enquanto o botão estiver pressionado
        if (context.started)
        {
            isFiring = true;
        }
        else if (context.canceled)
        {
            isFiring = false;
        }
    }

    private void Update()
    {
        transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime * 1.5f); // antes era 5f

        if (isFiring) shotScript.Shoot();
        if (Keyboard.current.spaceKey.isPressed) shotScript.Shoot();
        
    }
}
