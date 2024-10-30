using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField]
    private RectTransform _bar;
    [SerializeField]
    private PlayerHealth _health;

    private float _maxSize;

    // Start is called before the first frame update
    void Start()
    {
        _maxSize = _bar.rect.width;
        _health.HealthChangeEvent += UpdateBar;
        UpdateBar();
    }
    private void UpdateBar()
    {
        float width = _maxSize * _health.GetNormalHealthPercent();
        _bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
