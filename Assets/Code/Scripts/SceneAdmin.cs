using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneAdmin : MonoBehaviour
{
    public Transform spaceship; // Uzay aracının Transform component'ı
    public Transform[] planets; // Gezegenlerin Transform component'ları
    public GameObject[] shouldDisable;
    public string[] planetScenes; // Gezegenlerin sahne isimleri
    public float threshold = 100f; // Yükleme mesafesi eşiği
    public Material [] skyboxes;
    public Animator transitionAnimator; // Fade efekti için bir Animator component'ı
    private bool isLoading = false; // Kilit mekanizması olarak kullanılacak değişken


    void Start()
    {
    }

    void Update()
    {
        // Tüm gezegenleri kontrol et
        for (int i = 0; i < planets.Length; i++)
        {
            float distance = Vector3.Distance(spaceship.position, planets[i].position);

            // Eğer uzay aracı bir gezegene yeterince yaklaştıysa, sahneyi yükle
            if (distance < threshold)
            {
                StartCoroutine(LoadPlanetScene(i));
                break;
            }
        }
    }

    IEnumerator LoadPlanetScene(int planetIndex)
    {
        if (isLoading) {
            yield break;
        }

        isLoading = true;
        // Fade-out animasyonunu başlat
        transitionAnimator.SetTrigger("Start");

        // Animasyon bitene kadar bekle
        yield return new WaitForSeconds(1f);


        for (int i = 0; i < shouldDisable.Length; i++)
            shouldDisable[i].SetActive(false);

        // Gezegen indeksine bağlı olarak uygun sahneyi yükle
        string sceneName = planetScenes[planetIndex];
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        // Fade-in animasyonunu başlat
        transitionAnimator.SetTrigger("End");
        RenderSettings.ambientIntensity = 10f;
        RenderSettings.skybox = skyboxes[planetIndex];
        spaceship.transform.position = new Vector3(0, 1000, 0);
        spaceship.transform.rotation = Quaternion.Euler(90, 0, 0);
        spaceship.GetComponent<Rigidbody>().useGravity = true;
    }
}
