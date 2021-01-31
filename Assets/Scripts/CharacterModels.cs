using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModels : MonoBehaviour
{
    
    private List<GameObject> models;
    //Default index of the model
    private int selectionIndex = 0;
    private int index;

    private GameObject[] characterList;

    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("MiniFig");

        models = new List<GameObject>();
        
        characterList = new GameObject[transform.childCount];

        // Fill  the array with our models
        for (int i =0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;
        
        // Toggle off the characters
        foreach (GameObject go in characterList)
            go.SetActive(false);

        // Toogle on the first index
        if (characterList[index])
            characterList[index].SetActive(true);
    }
    
    
    public void Select(int index){
        if (index == selectionIndex)
            return;

        if (index < 0 || index >= models.Count)
            return;

        models[selectionIndex].SetActive(false);
        selectionIndex = index;
        models[selectionIndex].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

        
    }
   
}
