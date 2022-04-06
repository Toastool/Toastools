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
        // image�� color ������Ƽ�� a ������ ���� set�� �Ұ����ؼ� ������ ����
        Color color = image.color;


        // ���� ��(a)�� 0���� ũ�� ���� �� ����
        if(color.a > 0 )
        {
            color.a -= Time.deltaTime;
        }
        
        // ���� ��(a)�� 1���� ������ ���� �� ����
        if (color.a < 1)
        {
            color.a += Time.deltaTime;
        }

        // �ٲ� ���� ������ image.color�� ����
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
            // fadeTime���� ����� fadeTime �ð� ����
            // percent ���� 0���� 1�� �����ϵ��� ��
            currenTime += Time.deltaTime;
            percent = currenTime / fadeTime;

            // ���İ��� start���� end���� fadeTime �ð� ���� ��ȭ��Ų��
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
