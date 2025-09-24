using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Bars")]
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    [Header("Style")]
    public float length = 20f;     // bar length (px)
    public float thickness = 2f;   // bar thickness (px)
    public Color color = Color.white;

    [Header("Spread")]
    public float restGap = 10f;    // gap from center at rest
    public float maxExtra = 24f;   // extra gap when “kicked”
    public float expandSpeed = 40f;
    public float shrinkSpeed = 20f;

    float targetGap, currentGap;

    // Initialize
    void Start()
    {
        currentGap = targetGap = restGap;
        ApplyStyle();
        ApplyPositions(currentGap);
    }
    void OnValidate(){ ApplyStyle(); ApplyPositions(currentGap); }

    // Smoothly animate the gap size
    void Update()
    {
        float s = currentGap < targetGap ? expandSpeed : shrinkSpeed;
        currentGap = Mathf.MoveTowards(currentGap, targetGap, s * Time.deltaTime);
        ApplyPositions(currentGap);
        if (Mathf.Approximately(currentGap, targetGap)) targetGap = restGap;
    }

    // Increase the gap by a fraction of maxExtra
    public void Kick(float strength01)
    {
        targetGap = Mathf.Clamp(restGap + strength01 * maxExtra, restGap, restGap + maxExtra);
    }
 
    // Apply style settings to the bars
    void ApplyStyle()
    {
        if (top)    top.sizeDelta    = new Vector2(thickness, length);
        if (bottom) bottom.sizeDelta = new Vector2(thickness, length);
        if (left)   left.sizeDelta   = new Vector2(length, thickness);
        if (right)  right.sizeDelta  = new Vector2(length, thickness);

        SetColor(top, color); SetColor(bottom, color);
        SetColor(left, color); SetColor(right, color);
    }

    // Position the bars based on the current gap
    void ApplyPositions(float gap)
    {
        float off = gap + length * 0.5f; // inner ends touch the gap
        if (top)    top.anchoredPosition    = new Vector2(0,  off);
        if (bottom) bottom.anchoredPosition = new Vector2(0, -off);
        if (left)   left.anchoredPosition   = new Vector2(-off, 0);
        if (right)  right.anchoredPosition  = new Vector2( off, 0);
    }

    // Helper to set color of a RectTransform's Image
    static void SetColor(RectTransform rt, Color c)
    {
        if (!rt) return;
        var img = rt.GetComponent<Image>();
        if (img) img.color = c;
    }
}
