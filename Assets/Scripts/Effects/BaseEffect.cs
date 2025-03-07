using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public abstract class BaseEffect : MonoBehaviour
{
    [Header("Base Effect Settings")]
    public float displayDuration = 3f;
    public float fadeOutDuration = 0.5f;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        StartCoroutine(AnimateEffect());
    }

    protected virtual IEnumerator AnimateEffect()
    {
        // 表示時間を待つ
        yield return new WaitForSeconds(displayDuration);

        // フェードアウト
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeOutDuration;
            canvasGroup.alpha = 1f - normalizedTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
