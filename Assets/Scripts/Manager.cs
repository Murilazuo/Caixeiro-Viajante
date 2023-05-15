using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    List<City> allCities;
    List<StreetPath> allPaths;
    [SerializeField] int population;
    [SerializeField] float percentageToDesytoy = .3f;
    [SerializeField] float percentageToPreserve = .7f;
    [SerializeField] float mutateAmount;
    [SerializeField] int cityCount;
    [SerializeField] GameObject cityPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] Vector2 spawnRange;
    [SerializeField] TMP_Text beterDistance;

    [SerializeField] TMP_Text autoplayText;
    public int CityCount { get => cityCount; }

    public static Manager Instance;

    public void Awake()
    {
        Instance = this;
        allCities = new();
        allPaths = new();
    }
    void ClearPathColor()
    {
        foreach(StreetPath path in allPaths)
        {
            path.ClearColor();
        }
    }
    public void GenerateCity()
    {
        for(int i = 0; i < allCities.Count; i++)
        {
            Destroy(allCities[i].gameObject);
        }
        allCities.Clear();
        for(int i = 0; i < cityCount; i++)
        {
            allCities.Add(Instantiate(cityPrefab, RandomPositionInRange(), Quaternion.identity).GetComponent<City>());
        }
    }
    public void GeneratePath()
    {
        for (int i = 0; i < allPaths.Count; i++)
        {
            Destroy(allPaths[i].gameObject);
        }
        allPaths.Clear();
        for (int i = 0; i < population; i++)
        {
            StreetPath streetPath = Instantiate(pathPrefab).GetComponent<StreetPath>();
            allPaths.Add(streetPath);
            streetPath.Initialize();
        }

        SelectPaths();
    }

    void SelectPaths()
    {
        ClearPathColor();

        allPaths = allPaths.OrderBy(x => x.totalDistance).ToList();
        /*
        foreach(StreetPath i in allPaths)
        {
            print(i.totalDistance);
        }
        */

        allPaths[0].SetBetterPath();

        beterDistance.text = allPaths[0].totalDistance.ToString("F");
    }
    public void NextGeneration()
    {
        List<int>[] paths = new List<int>[allPaths.Count];

        for (int i = 0; i < allPaths.Count; i++)
        {
            paths[i] = new List<int>(allPaths[i].cityPath);
        }

        int[] quarters = new int[3];
        quarters[0] = allPaths.Count / 4;
        quarters[1] = quarters[0] * 2;
        quarters[2] = quarters[0] * 3;

        for(int i = 0; i < allPaths.Count; i++)
        {
            if(i > quarters[2])
            {
                allPaths[i].Initialize();
            }
            else if (i > 0)
            {
                int parent1Id = Random.Range(0,quarters[1]);
                int parent2Id = Random.Range(0,quarters[1]);
            
                while(parent1Id == parent2Id)
                    parent2Id = Random.Range(0, quarters[1]);

                allPaths[i].Crossover(paths[parent1Id], paths[parent2Id]);

                if(Random.Range(0,10) <= 1)
                {
                    allPaths[i].Mutate();
                    
                    if (Random.Range(0, 10) <= 1)
                        allPaths[i].Mutate();
                }
            }
        }
        /*
        for (int i = (int)((float)population * percentageToPreserve); i < (int)((float)population * percentageToDesytoy); i++)
        {
            for(int j = 0;j < mutateAmount;j++)
                allPaths[i].Mutate();
        }
        for (int i = (int)((float)population * percentageToDesytoy); i < population; i++)
        {
            allPaths[i].Initialize();
        }
        */
        SelectPaths();
    }
    public City GetCity(int id) => allCities[id];
    public void SetCityNumber(string number)
    {
        if (number == string.Empty) number = "0";
        cityCount = int.Parse(number);
    }
    public void SetPopulationNumber(string number)
    {
        if (number == string.Empty) number = "0";
        population = int.Parse(number);
    }
    public void SetMutateAmount(string number)
    {
        if (number == string.Empty) number = "1";
        mutateAmount = int.Parse(number);
    }
    public void SetPercentageToDestoy(string number)
    {
        if (number == string.Empty) number = "0.1f";
        percentageToDesytoy = float.Parse(number);
    }
    public void SetPercetageToMutate(string number)
    {
        if (number == string.Empty) number = "0.5f";
        percentageToPreserve = float.Parse(number);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, spawnRange*2);
    }
    Vector2 RandomPositionInRange()
    {
        Vector2 result;
        result.x = Random.Range(-spawnRange.x, spawnRange.x);
        result.y = Random.Range(-spawnRange.y, spawnRange.y);
        
        return result;
    }
    bool isPlay;
    public void ToggleAutoPlay()
    {
        isPlay = !isPlay;

        if (isPlay)
            StartCoroutine(nameof(AutoPlay));
        else
            StopCoroutine(nameof(AutoPlay));
        
        autoplayText.text = isPlay ? "Stop" : "Autoplay";
    }
    [SerializeField] float timeToNextGen;
    IEnumerator AutoPlay()
    {
        while (isPlay)
        {
            NextGeneration();
            yield return new WaitForEndOfFrame();
        }
    }
}
