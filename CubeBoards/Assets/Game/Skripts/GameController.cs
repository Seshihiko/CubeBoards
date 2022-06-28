using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private GameObject[] Points; //����� �� ������ ����� ��������� ����
    [SerializeField] private GameObject[] Cubes;
    [SerializeField] private GameObject VictoryPanel;

    private List<GameObject> mainPoints = new List<GameObject>(); //�������� ����� �� ������� ������� ���� ������, ����� ����� ��� �������� ������������ ����������
    private int combinationCount = 0; 
    private void Start()
    {
        instance = this;
        SpawnCube();
    }

    public void Combination() // ��������� ���������� ����������
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
            mainPoints[i].GetComponent<PointController>().victoryIndex = Data.victoryCombination[i]; //������������� � �������� ����� ����� ������� ������
            
            Instantiate(_cube, mainPoints[i].transform.position, Quaternion.identity);//������� ������ �� ����� � ������ ����������

            mainPoints[i].GetComponent<PointController>().cubeIndex = Cubes[Data.cubes[i]]
                .GetComponent<CubeSkript>().index; //��������� � ����� ����� ����, ��� �� ����� ���� �������� ��������� �� ������������ ����
        }
    }
}                         
