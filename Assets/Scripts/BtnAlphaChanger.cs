using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnAlphaChanger : MonoBehaviour
{
    public Button targetButton;           // 변경할 버튼
    public float fadeDuration = 1.5f;     // 알파 값이 서서히 변경되는 시간
    public float waitTime = 1.2f;         // 알파 값이 1에서 대기하는 시간

    private void Start()
    {
        StartCoroutine(ChangeAlpha());
    }

    private IEnumerator ChangeAlpha()
    {
        Image buttonImage = targetButton.GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("Button에 Image 컴포넌트가 없습니다.");
            yield break;
        }

        while (true)
        {
            // 알파 0에서 1로 변화
            yield return StartCoroutine(FadeAlpha(buttonImage, 0.4f, 1f));

            // 최대 알파에서 대기
            yield return new WaitForSeconds(waitTime);

            // 알파 1에서 0으로 변화
            yield return StartCoroutine(FadeAlpha(buttonImage, 1f, 0.4f));

            // 최소 알파에서 대기
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator FadeAlpha(Image image, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 알파 값 보정
        color.a = endAlpha;
        image.color = color;
    }
}
