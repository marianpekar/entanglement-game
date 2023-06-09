using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector3 anotherDimensionOffset;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Picker picker;

    [SerializeField]
    private ButtonPusher buttonPusher;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Detectable detectable;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private DimensionSwitcher dimensionSwitcher;

    [SerializeField]
    public Texture2D cursorBrain;

    [SerializeField]
    public Texture2D cursorHand;

    [SerializeField]
    public Texture2D cursorWalk;

    [SerializeField]
    public Texture2D cursorCantWalk;

    [SerializeField]
    public LevelManager levelManager;

    private bool isActive = false;

    private float pickerSocketRoationSpeed = 600f;

    [SerializeField]
    private PlayerAnimationController animationController;

    [SerializeField]
    private UnityEvent OnPlayerKilled;

    public void Kill()
    {
        SetActive(false);
        animationController.TriggerDie();
        agent.isStopped = true;
        OnPlayerKilled?.Invoke();
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    private enum MouseButton 
    {
        Left = 0,
        Right = 1,
        Middle = 2,
    }

    private NavMeshPath navMeshPath;

    private void Awake()
    {
        navMeshPath = new NavMeshPath();
    }

    private void FixedUpdate()
    {
        detectable.CanBeHear = agent.velocity.magnitude > 0.1f;
    }

    void Update()
    {
        if (!isActive)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (levelManager.IsInGameMenuEnabled())
            {
                levelManager.HideInGameMenu();
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.1f;
                levelManager.ShowInGameMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && levelManager.IsInGameMenuEnabled())
        {
            Time.timeScale = 1f;
            levelManager.HideInGameMenu();
            levelManager.LoadMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.R) && levelManager.IsInGameMenuEnabled())
        {
            isActive = false;
            Time.timeScale = 1f;
            levelManager.HideInGameMenu();
            levelManager.RestartCurrentLevel();
        }

        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out var hit, int.MaxValue, ~(1 << 2 | 1 << 3)))
        {
            bool canWalk = agent.CalculatePath(hit.point, navMeshPath);

            GameObject gameObject = hit.collider.gameObject;

            Teleportable teleportable = gameObject.GetComponent<Teleportable>();
            Pickable pickable = gameObject.GetComponent<Pickable>();
            ButtonPushable buttonPushable = gameObject.GetComponent<ButtonPushable>();

            if (teleportable != null || pickable != null)
            {
                Cursor.SetCursor(cursorBrain, Vector2.zero, CursorMode.Auto);
                canWalk = true;
            }
            else if (buttonPushable != null)
            {
                Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.Auto);
                canWalk = true;
            }
            else
            {
                if (!canWalk)
                {
                    Cursor.SetCursor(cursorCantWalk, Vector2.zero, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(cursorWalk, Vector2.zero, CursorMode.Auto);
                }
            }

            if (Input.GetMouseButton((int)MouseButton.Right) && !levelManager.IsInGameMenuEnabled())
            {
                if (picker.HasPickable())
                {
                    picker.DropObject();
                    picker.ResetSocketSotation(playerTransform.position, Vector3.up);
                }
                else
                {
                    picker.ResetSocketSotation(playerTransform.position, Vector3.up);

                    Debug.DrawLine(playerCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);

                    if (teleportable != null)
                    {
                        teleportable.Teleport(anotherDimensionOffset);
                    }                
                }
            }

            if (Input.GetMouseButton((int)MouseButton.Left) && !levelManager.IsInGameMenuEnabled())
            {           
                picker.ResetSocketSotation(playerTransform.position, Vector3.up);

                Debug.DrawLine(playerCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);

                if (pickable != null)
                {
                    picker.PickObject(gameObject);
                }

                if (buttonPushable != null)
                {
                    buttonPusher.PushButton(buttonPushable);
                }          
                
                if (canWalk)
                {
                    SetTarget(hit.point);
                }
            }
        }

        if (Input.GetMouseButton((int)MouseButton.Middle) && !levelManager.IsInGameMenuEnabled())
        {
            if (picker.HasPickable())
            {
                picker.ThrowObject();
                picker.ResetSocketSotation(playerTransform.position, Vector3.up);
            }
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollDelta) > 0.001f && picker.HasPickable() && !levelManager.IsInGameMenuEnabled())
        {
            picker.RotateSocketAround(playerTransform.position, Vector3.up, pickerSocketRoationSpeed * Time.deltaTime * scrollDelta);         
        }

        if (Input.GetKeyDown(KeyCode.Space) && !levelManager.IsInGameMenuEnabled())
        {
            dimensionSwitcher.Switch();
        }
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }
}