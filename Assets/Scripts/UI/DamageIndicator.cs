using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageIndicator : MonoBehaviour
{
    public RectTransform _imageIndicator;
    public HitBox _normalPlayerHitBox;
    public HitBox _spherePlayerHitBox;
    public Image Image;
    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;
    private void Start()
    {
        //_imageIndicator = GetComponent<RectTransform>();

        _normalPlayerHitBox.DirectionDamageEvent += ShowDamageIndicator;
        _spherePlayerHitBox.DirectionDamageEvent += ShowDamageIndicator;


    }
    private void ShowDamageIndicator(Vector3 pos) 
    {
        StopCoroutine(HideIndicatorAfterDelay(0.8f));
        Image.gameObject.SetActive(true);
        tPos = pos;
        Vector3 direction = _spherePlayerHitBox.transform.position - tPos;
        tRot = Quaternion.LookRotation(direction);
        tRot.z = -tRot.y;
        tRot.x = 0;
        tRot.y = 0;
        Vector3 NorthDirection = new Vector3(0, 0, _spherePlayerHitBox.transform.eulerAngles.y);

        _imageIndicator.localRotation = tRot * Quaternion.Euler(NorthDirection);
        StartCoroutine(HideIndicatorAfterDelay(0.8f));
    }
    private System.Collections.IEnumerator HideIndicatorAfterDelay(float delay)
    {
        float curTime = 0f;
        Color originalC = Image.color;
        originalC.a = 1f;
        Image.color = originalC;
        while (curTime < delay) 
        {
            curTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, curTime / delay);

            var newC = Image.color;
            newC.a = alpha;
            Image.color = newC;
            yield return null;
        }
        Color finalColor = Image.color;
        finalColor.a = 0f;
        Image.color = finalColor;
        //Image.gameObject.SetActive(false);
    }
}
