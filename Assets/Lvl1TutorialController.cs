using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1TutorialController : MonoBehaviour
{
    public GameObject pressETutorial;

    public void ShowTutorialLvl1()
    {
        pressETutorial.SetActive(true); //���������� ���������
    }
    public void HideTutorialLvl1()
    {
        pressETutorial.SetActive(false); //���������� ���������
    }
    private void Update()
    {
        if (References.electors.Count == 0 && pressETutorial.activeInHierarchy)
        {
            HideTutorialLvl1();
        }
    }
}
