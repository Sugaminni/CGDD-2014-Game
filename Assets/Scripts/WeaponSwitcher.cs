#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;   // new Input System
#endif
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Tooltip("Leave empty to auto-fill with all direct children.")]
    public GameObject[] weapons;

    [SerializeField] int startIndex = 0;
    int index;

    void Awake()
    {
        // Auto-fill from children if not set
        if (weapons == null || weapons.Length == 0)
        {
            weapons = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                weapons[i] = transform.GetChild(i).gameObject;
        }
    }

    void Start()
    {
        index = Mathf.Clamp(startIndex, 0, Mathf.Max(0, weapons.Length - 1));
        SetActive(index);
    }

    // Handle input for switching weapons
    void Update()
    {
        if (weapons.Length == 0) return;

        // Number keys 1-9
#if ENABLE_INPUT_SYSTEM
        var kb = Keyboard.current;
        if (kb != null)
        {
            int max = Mathf.Min(9, weapons.Length);
            for (int i = 0; i < max; i++)
            {
                // Keyboard has an indexer by Key enum; Digit1..Digit9 are contiguous
                var keyCtrl = kb[(Key)((int)Key.Digit1 + i)];
                if (keyCtrl != null && keyCtrl.wasPressedThisFrame)
                {
                    SetActive(i);
                    return;
                }
            }
        }
#else
        for (int i = 0; i < Mathf.Min(9, weapons.Length); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetActive(i);
                return;
            }
        }
#endif

        // Scroll wheel
#if ENABLE_INPUT_SYSTEM
        float scroll = 0f;
        var mouse = Mouse.current;
        if (mouse != null) scroll = mouse.scroll.ReadValue().y;
#else
        float scroll = Input.GetAxis("Mouse ScrollWheel");
#endif
        if (scroll > 0f) SetActive((index + 1) % weapons.Length);
        else if (scroll < 0f) SetActive((index - 1 + weapons.Length) % weapons.Length);
    }

    // Activate only the weapon at the given index
    void SetActive(int i)
    {
        index = Mathf.Clamp(i, 0, weapons.Length - 1);
        for (int k = 0; k < weapons.Length; k++)
            if (weapons[k]) weapons[k].SetActive(k == index);
    }
}
