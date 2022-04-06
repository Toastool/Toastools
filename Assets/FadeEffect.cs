using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10f)]
    private float fadeTime;
    private Image image;

    private void Awake()
    {
        image = GetComponent<image>();

        // Fade In
        StartCoroutine(fadeTime(1, 0));

        // Fade Out
        //StartCoroutine(Fade(0, 1));
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
    private IEnumerator Fade(float start, float end)
    {
        float currenTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            // fadeTime으로 나누어서 fadeTime 시간 동안
            // percent 값이 0에서 1로 증가하도록 함
            currentTime += fadeTime.deltaTime;
            percent = currenTime / fadeTime;

            // 알파값을 start부터 end까지 fadeTime 시간 동안 변화시킨다
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
