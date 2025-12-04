using UnityEngine;
using TMPro;

public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro tmp;

    private Vector3 startPos;
    private Vector3 endPos;

    private float moveDistance;
    private float moveSpeed;

    private Color startColor;

    public void Setup(string text, Color color, float _moveDistance = 2f, float _moveSpeed = 2f)
    {
        tmp = GetComponent<TextMeshPro>();
        tmp.text = text;
        startColor = color;
        tmp.color = color;

        startPos = transform.position;
        endPos = startPos + new Vector3(0, _moveDistance, 0);

        moveDistance = _moveDistance;
        moveSpeed = _moveSpeed;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-3f, 3f));

    }

    void Update()
    {
        if (tmp == null) return;

        // Move upward
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

        // Calculate alpha based on distance traveled
        float traveled = Vector3.Distance(transform.position, startPos);
        float alpha = 1f - Mathf.Clamp01(traveled / moveDistance);
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // Destroy when fully faded
        if (alpha <= 0f)
            Destroy(gameObject);
    }
}
