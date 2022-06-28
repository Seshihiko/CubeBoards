using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    public GameObject particleGreen;
    public GameObject particleBlue;
    public GameObject target;

    public int cubeIndex = 0; //����� ������
    public int victoryIndex = 0; //���������� ������

    private void Start()
    {
        target = gameObject;
    }
    public void ActiveParticle()
    {
        particleBlue.SetActive(true);
        particleGreen.SetActive(false);
    }
    public void NotActiveParticle()
    {
        particleBlue.SetActive(false);
        particleGreen.SetActive(true);
    }
}
