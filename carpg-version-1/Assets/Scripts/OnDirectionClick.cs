using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnDirectionClick : MonoBehaviour
{
    public GlobalParams.Direction dir;
    private bool isSelected;
    [SerializeField] private StateManager manager;

    private void OnEnable()
    {
        isSelected = false;
    }

    private void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !isSelected)
        {
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 100))
            {
                if (hit.collider.transform == transform)
                {
                    manager.ChangeAttackDirection(dir);
                    isSelected = true;
                }

            }
        }
    }
}
