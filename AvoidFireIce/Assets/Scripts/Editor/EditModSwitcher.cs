using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModSwitcher : MonoBehaviour
{
    public GameObject EditorMod;
    public GameObject TestMod;
    public bool isEditing = true;

    private void OnEnable()
    {
        TestMod.gameObject.SetActive(false);
    }
    public void SwithingTest()
    {
        EditorMod.gameObject.SetActive(!isEditing);
        TestMod.gameObject.SetActive(isEditing);
        isEditing = !isEditing;
    }
}
