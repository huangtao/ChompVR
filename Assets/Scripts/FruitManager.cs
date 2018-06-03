using UnityEngine;
using System.Collections;

public class FruitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fruits;

    [SerializeField]
    private GameObject fruitSpawnPoint;

    [SerializeField]
    private float countDown = 20.0f;

    [SerializeField]
    private float disappearAfter = 7.0f;

    private GameObject spawnedFruit;


    void Start()
    {
        InvokeRepeating("SpawnFruit", countDown, countDown);
    }


    private int i = 0;
    void SpawnFruit()
    {
        // Only spawn fruit if the game is playing
        if (ApplicationModel.GameState != GameState.Playing)
            return;

        if (i == (fruits.Length - 1))
        {
            i = 0;
        }
        else
        {
            i++;
        }

        spawnedFruit = Instantiate(fruits[i], fruitSpawnPoint.transform.position, Quaternion.identity) as GameObject;

        MusicManager.Instance.PlayOneShot(Sound.FruitSpawned);

        Invoke("Disappear", disappearAfter);
    }


    void Disappear()
    {
        StartCoroutine(Animinate());
    }


    IEnumerator Animinate()
    {
        yield return StartCoroutine(Flash());
        Destroy(spawnedFruit);
    }


    IEnumerator Flash()
    {
        if (spawnedFruit != null)
        {
            //todo loop through the meshes on the fruit so they all flash
            Material mat = spawnedFruit.GetComponentsInChildren<MeshRenderer>().material;
            Color col = mat.color;

            for (int i = 0; i < 15; i++)
            {
                float time = i > 10 ? 0.05f : i > 5 ? 0.1f : 0.2f;

                mat.color = Color.white;
                yield return new WaitForSeconds(time);

                mat.color = col;
                yield return new WaitForSeconds(time);
            }
        }
    }
}
