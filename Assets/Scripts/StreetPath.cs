using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StreetPath : MonoBehaviour 
{
    
    [SerializeField] LineRenderer line; 
    [SerializeField] Gradient color; 
    [SerializeField] Gradient normalColor; 
    [SerializeField] List<int> totalCities;
    public List<int> cityPath;
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
    struct DividedStreerPath
    {
        List<int> originalPath;

        List<int> part1;
        List<int> part2;

        int halfCount;
        bool isPair;
        int firtHalfCount;

        public DividedStreerPath(List<int> path)
        {
            originalPath = path;

            halfCount = originalPath.Count / 2;
            isPair = originalPath.Count % 2 == 0;
            firtHalfCount = halfCount + (isPair ? 0 : 1);

            part1 = new List<int>();
            part2 = new List<int>();

            for (int i = 0; i < path.Count; i++)
            {
                if(i < firtHalfCount)
                {
                    part1.Add(path[i]);
                }
                else
                    part2.Add(path[i]);
            }
        }
        public List<int> MergedPart
        {
            get
            {
                List<int> result = new();

                for(int i = 0; i < part1.Count;i++)
                    result.Add(part1[i]);

                for (int i = 0; i < part2.Count; i++)
                    result.Add(part2[i]);

                return result;
            }
        }

        public void Crossover(DividedStreerPath parent2)
        {

            part2 = parent2.part2;
            List<int> indexToChange = new();

            for (int i = 0; i < firtHalfCount; i++)
            {
                if (part2.Contains(part1[i]))
                {
                    indexToChange.Add(i);
                    part1[i] = -1;
                }
            }


            List<int> mergedPath = new(MergedPart);
            List<int> remainingNumbers = new(originalPath);

            foreach(int i in mergedPath)
                if (i != -1)
                    remainingNumbers.Remove(i);

            foreach(int i in indexToChange)
            {
                part1[i] = remainingNumbers[0];
                remainingNumbers.RemoveAt(0);
                remainingNumbers.TrimExcess();
            }

        }

    }

    public void Crossover(List<int> parent1, List<int> parent2)
    {
        
        DividedStreerPath path1 = new(parent1);
        DividedStreerPath path2 = new(parent2);
        path1.Crossover(path2);

        cityPath = path1.MergedPart;
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
    public void ClearColor()
    {
        line.sortingOrder = 0;
        line.colorGradient = normalColor;
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

    public static string PrintPath(List<int> path)
    {
        string result = "";

        for (int i = 0; i < path.Count; i++)
            result += "| " + path[i] + " |";

        return result;
    }
}
