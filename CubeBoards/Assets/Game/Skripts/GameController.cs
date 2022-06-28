using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private GameObject[] Points; //“очки на котрых могут спавнитс€ кубы
    [SerializeField] private GameObject[] Cubes;
    [SerializeField] private GameObject VictoryPanel;

    private List<GameObject> mainPoints = new List<GameObject>(); //ќсновные точки на которых должены быть кубики, масив нужен дл€ проверки правильности комбинации
    private int combinationCount = 0; 
    private void Start()
    {
        instance = this;
        SpawnCube();
    }

    public void Combination() // ѕровер€ет количество комбинаций
    {
        for (int i = 0; i < 5; i++)
        {
            if (mainPoints[i].GetComponent<PointController>().cubeIndex == Data.victoryCombination[i])
            {
                combinationCount++;
            }
        }
        if (combinationCount == 5) VictoryPanel.SetActive(true);
        else combinationCount = 0;
    }

    private void SpawnCube() 
    {
        for (int i = 0; i < Cubes.Length; i++)
        {
            var _cube = Cubes[Data.cubes[i]];

            mainPoints.Add(Points[Data.spawnPoint[i]]);
            mainPoints[i].GetComponent<PointController>().victoryIndex = Data.victoryCombination[i]; //устанавливает в ключивую точку номер нужного кубика
            
            Instantiate(_cube, mainPoints[i].transform.position, Quaternion.identity);//—павнит кубики на точки в нужной комбинации

            mainPoints[i].GetComponent<PointController>().cubeIndex = Cubes[Data.cubes[i]]
                .GetComponent<CubeSkript>().index; //ƒобавл€ет в точку номер куба, что бы можно было вы€снить правильно ли установленны кубы
        }
    }
}                         
