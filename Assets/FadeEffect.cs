using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop };

public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image;
    private FadeState fadeState;

    private void Awake()
    {
        image = GetComponent<Image>();

        // Fade In
        //StartCoroutine(Fade(1, 0));

        // Fade Out
        //StartCoroutine(Fade(0, 1));

        OnFade(FadeState.FadeOut);
    }

    /*private void Update()
    {
        // image의 color 프로퍼티는 a 변수만 따로 set이 불가능해서 변수에 저장
        Color color = image.color;


        // 알파 값(a)이 0보다 크면 알파 값 감소
        if(color.a > 0 )
        {
            color.a -= Time.deltaTime;
        }
        
        // 알파 값(a)이 1보다 작으면 알파 값 증가
        if (color.a < 1)
        {
            color.a += Time.deltaTime;
        }

        // 바뀐 색상 정보를 image.color에 저장
        image.color = color;
    }*/

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeInOut:
            //    ;
            case FadeState.FadeLoop:
                StartCoroutine(FadeInOut());
                break;
        }
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));

            yield return StartCoroutine(Fade(0, 1));

            if (fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float currenTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            // fadeTime으로 나누어서 fadeTime 시간 동안
            // percent 값이 0에서 1로 증가하도록 함
            currenTime += Time.deltaTime;
            percent = currenTime / fadeTime;

            // 알파값을 start부터 end까지 fadeTime 시간 동안 변화시킨다
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
