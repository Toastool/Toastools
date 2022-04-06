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
    private IEnumerator Fade(float start, float end)
    {
        float currenTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            // fadeTime���� ����� fadeTime �ð� ����
            // percent ���� 0���� 1�� �����ϵ��� ��
            currentTime += fadeTime.deltaTime;
            percent = currenTime / fadeTime;

            // ���İ��� start���� end���� fadeTime �ð� ���� ��ȭ��Ų��
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
