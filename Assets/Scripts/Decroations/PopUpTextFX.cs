using UnityEngine;
using TMPro;

public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro tmp;

    private Color startColor;
    private Vector3 startPos;
    private Vector3 endPos;

    private float moveDistance;
    private float moveSpeed;

    private float lifetime = 0.8f;
    private float timer;

    // Animation settings
    private bool punchScale;
    private bool squashStretch;
    private bool curvedArc;
    private float curveDirection;

    public void Setup(string text, Color color, float _moveDistance = 2f, float _moveSpeed = 2f,
                      bool usePunch = false, bool useSquash = false, bool useCurve = false)
    {
        tmp = GetComponent<TextMeshPro>();
        tmp.text = text;

        startColor = color;
        tmp.color = color;

        startPos = transform.position;
        moveDistance = _moveDistance;
        moveSpeed = _moveSpeed;
        endPos = startPos + new Vector3(0, moveDistance, 0);

        punchScale = usePunch;
        squashStretch = useSquash;
        curvedArc = useCurve;

        curveDirection = Random.Range(0, 2) == 0 ? -1f : 1f; // random left / right

        transform.localScale = Vector3.zero; // start small for pop animation
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifetime;

        // -------------------------------------------
        // ★ Punch Scale (impact pop)
        // -------------------------------------------
        if (punchScale)
        {
            float pop = Mathf.Sin(t * 4f * Mathf.PI) * 0.4f; 
            float scale = 1 + Mathf.Max(0, pop);
            transform.localScale = Vector3.one * scale;
        }
        // -------------------------------------------
        // ★ Squash & Stretch (bouncy)
        // -------------------------------------------
        else if (squashStretch)
        {
            float squash = Mathf.Sin(t * Mathf.PI);
            float x = 1 + squash * 0.4f;
            float y = 1 - squash * 0.3f;
            transform.localScale = new Vector3(x, y, 1);
        }
        // -------------------------------------------
        // ★ Default smooth scale-in
        // -------------------------------------------
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 8f);
        }

        // -------------------------------------------
        // ★ Movement (straight or curved)
        // -------------------------------------------
        Vector3 targetPos = endPos;

        if (curvedArc)
        {
            float horizontal = Mathf.Sin(t * Mathf.PI) * 0.5f * curveDirection;
            targetPos += new Vector3(horizontal, 0, 0);
        }

        transform.position = Vector3.Lerp(startPos, targetPos, t);

        // -------------------------------------------
        // ★ Fade Out
        // -------------------------------------------
        float alpha = 1f - t;
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // Destroy on fade
        if (alpha <= 0f)
            Destroy(gameObject);
    }
}
