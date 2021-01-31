using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour
{
    
    private GameObject[] characterList;
    private GameObject[] menuList;
    private int index;

    [SerializeField, Tooltip("Menu Item")]
    public GameObject MenuItem;

    [SerializeField, Tooltip("Title Item")]
    public GameObject title;


    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("MiniFig");

        characterList = new GameObject[transform.childCount];

        // Fill  the array with our models
        for (int i =0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;
        
        // We toggle off the characters
        foreach (GameObject character in characterList)
            character.SetActive(false);

        // We toogle on the selected character
        if (characterList[index])
            characterList[index].SetActive(true);

        
        menuList = new GameObject[MenuItem.transform.childCount];
        
        // Fill  the array with our models
        for (int i =0; i < (MenuItem.transform.childCount -1); i++)
            menuList[i] = MenuItem.transform.GetChild(i).gameObject;

        setMenu();

        


    }

    public void ToggleLeft()
    {
        // Toogle off the current model
        characterList[index].SetActive(false);
        index--;
        if (index < 0)
            index = characterList.Length - 1;

        // Toogle on the new model
        characterList[index].SetActive(true);

        setMenu();
    }

    public void ToggleRight()
    {
        // Toogle off the current model
        characterList[index].SetActive(false);
        index++;
        if (index == characterList.Length )
            index = 0;

        // Toogle on the new model
        characterList[index].SetActive(true);

        setMenu();
    }

    public void confirmButton()
    {
        PlayerPrefs.SetInt("MiniFig", index);
    
    }

    public void setMenu()
    {
        

        menuList = new GameObject[MenuItem.transform.childCount];

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuItem.transform.GetChild(index).gameObject);

        title.GetComponent<TMPro.TMP_Text>().text = MenuItem.transform.GetChild(index).gameObject.GetComponentInChildren<TMPro.TMP_Text>().text;

    }



}
