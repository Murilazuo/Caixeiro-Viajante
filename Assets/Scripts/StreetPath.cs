using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPath : MonoBehaviour 
{
    
    [SerializeField] LineRenderer line; 
    [SerializeField] Gradient color; 
    [SerializeField] Gradient normalColor; 
    [SerializeField] List<int> totalCities;
    [SerializeField] List<int> cityPath;
    public float totalDistance;
    public float TotalDistance { get => totalDistance; }
    public void CalculateTotalDistace()
    {
        totalDistance = 0;
        for(int i = 1; i < Manager.Instance.CityCount; i++)
        {
            totalDistance += Vector3.Distance( CityPosition(i - 1), CityPosition(i));
        }

        totalDistance += Vector3.Distance(CityPosition(Manager.Instance.CityCount-1), CityPosition(0));
    }
    Vector3 CityPosition(int id) => Manager.Instance.GetCity(cityPath[id]).Position;
    void AddRandomCity()
    {
        AddCity(Random.Range(0,totalCities.Count));
    }
    void AddCity(int index)
    {
        cityPath.Add(totalCities[index]);
        totalCities.RemoveAt(index);
        totalCities.TrimExcess();
    }
    public void Initialize()
    {
        totalCities = new();

        line.colorGradient = normalColor;


        for (int i = 0; i < Manager.Instance.CityCount; i++)
            totalCities.Add(i);

        Clear();

        AddCity(0);

        GenerateRandomPath();

        Draw();

        CalculateTotalDistace();
    }
    public void Mutate()
    {
        line.colorGradient = normalColor;

        int indexA = Random.Range(0, Manager.Instance.CityCount);
        int indexB = Random.Range(0, Manager.Instance.CityCount);
        while (indexA == indexB) indexB = Random.Range(0, Manager.Instance.CityCount);

        int idA = cityPath[indexA];
        int idB = cityPath[indexB];

        cityPath[indexA] = idB;
        cityPath[indexB] = idA;
    }
    public void Clear()
    {
        totalDistance = 0;
        cityPath = new();
    }

    void GenerateRandomPath()
    {
        int cityCount = totalCities.Count;
        for (int i = 0; i < cityCount; i++)
        {
            AddRandomCity();
        }
    }
    void Draw()
    {
        line.positionCount = cityPath.Count +1;
        int i = 0;
        foreach (int cityId in cityPath)
        {
            line.SetPosition(i,Manager.Instance.GetCity(cityId).Position);
            i++;
        }
        line.SetPosition(i, Manager.Instance.GetCity(0).Position);
    }
    public void SetBetterPath()
    {
        line.colorGradient = color;
        line.sortingOrder = 1;
    }
    private void Start()
    {
        SetScale(PathSizeSeter.size);
    }
    void SetScale(float size)
    {
        line.startWidth = size;
        line.endWidth = size;
    }
    private void OnEnable()
    {
        PathSizeSeter.OnSetPathSize += SetScale;
    }
    private void OnDisable()
    {
        PathSizeSeter.OnSetPathSize -= SetScale;
    }
}
