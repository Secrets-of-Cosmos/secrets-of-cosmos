using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    public Light sun;
    public float fullDayLength; // Bir günün ne kadar sürmesini istediğinizi saniye cinsinden belirtin.
    public float currentTimeOfDay; // 0 = gece yarısı, 0.5 = öğle.
    public float timeMultiplier = 1f; // Zamanın ne kadar hızlı geçmesini istediğinizi kontrol etmek için kullanılabilir.

    // Start is called before the first frame update
    void Start()
    {
        currentTimeOfDay = 0.25f; // Günün başlangıcını sabah olarak belirliyoruz.
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / fullDayLength) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        // Güneş ışığının yoğunluğunu değiştirme
        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = intensityMultiplier;
    }
}
