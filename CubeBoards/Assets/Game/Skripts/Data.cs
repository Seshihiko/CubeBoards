using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data //0 - �����(������ 1), 1 - �����(2), 2 - ������(3), 3 - ���������(4), 4 - ����������(5);
                  //����� ����� �� ������� 0, 2, 3, 5, 6, 8;
{
    public static int[] cubes = new int[5] {1, 4, 2, 3, 0}; 
    public static int[] spawnPoint = new int[5] { 0, 1, 2, 3, 4};
    public static int[] victoryCombination = new int[5] {1, 3, 4, 5, 2 };
}
