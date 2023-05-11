using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    //[SerializeField] private Slider _healthBar;

    [SerializeField] private Slider _bossHealthBar;
    private Vector3 Offset;
   // [SerializeField] TMP_Text _bossHealthBarPercentage;
    // Start is called before the first frame update
    void Start()
    {
        //_thrusterBarPercentage = 100;
    }

    // Update is called once per frame
    void Update()
    {
        _bossHealthBar.transform.position = Camera.main.WorldToScreenPoint( transform.parent.position + new Vector3(0, 2.3f, 0));
        
    }



    public void UpdateBossLives(float currentHealthLevel)
    {
        currentHealthLevel = Mathf.Clamp(currentHealthLevel, 0f, 100f);
        _bossHealthBar.value = currentHealthLevel;
        if (currentHealthLevel >= 100)
        {
            currentHealthLevel = 100f;
        }

    }




}
