using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float textMoveSpeed;
    private float textalphaSpeed;
    private float textDestroyTime;
    private Transform mainCamera;

    private Color alpha;
    public float damage;
    [SerializeField] private TextMeshPro text;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        textMoveSpeed = 2.0f;
        textalphaSpeed = 2.0f;
        textDestroyTime = 2.0f;

        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString("N1");
        alpha = text.color;
        StartCoroutine(DestroyText());
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, textMoveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * textalphaSpeed);
        text.color = alpha;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
    }
    private IEnumerator DestroyText()
    {
        yield return new WaitForSeconds(textDestroyTime);
        Destroy(this.gameObject);
    }
}
