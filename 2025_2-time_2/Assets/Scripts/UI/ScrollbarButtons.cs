using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarButtons : MonoBehaviour
{
    [SerializeField] private float buttonStrenght;
    
    private Scrollbar scrollbar;

    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    public void ScrollRight() 
    {
        scrollbar.value = Mathf.Min(scrollbar.value + buttonStrenght, 1);
    }

    public void ScrollLeft()
    {
        scrollbar.value = Mathf.Max(scrollbar.value - buttonStrenght, 0);
    }
}
