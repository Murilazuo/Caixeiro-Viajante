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
    public int CityCount { get => cityCount; }

    public static Manager Instance;

    public void Awake()
    {
        Instance = this;
        allCities = new();
        allPaths = new();
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
        allPaths = allPaths.OrderBy(x => x.totalDistance).ToList();

        foreach(StreetPath i in allPaths)
        {
            print(i.totalDistance);
        }

        allPaths[0].SetBetterPath();

        beterDistance.text = allPaths[0].totalDistance.ToString("F");
    }
    public void NextGeneration()
    {
        for (int i = (int)((float)population * percentageToPreserve); i < (int)((float)population * percentageToDesytoy); i++)
        {
            for(int j = 0;j < mutateAmount;j++)
                allPaths[i].Mutate();
        }
        for (int i = (int)((float)population * percentageToDesytoy); i < population; i++)
        {
            allPaths[i].Initialize();
        }

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
}
