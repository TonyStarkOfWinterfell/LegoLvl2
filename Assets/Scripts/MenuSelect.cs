using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class MenuSelect : MonoBehaviour
    {

        [SerializeField, Tooltip("Character Item")]
        public GameObject CharacterItem;

        [SerializeField, Tooltip("Title Item")]
        public GameObject title;


        private GameObject[] menuList;
        private GameObject[] characterList;
        private int index;




        void Start()
        {
            index = PlayerPrefs.GetInt("MiniFig");

            menuList = new GameObject[transform.childCount];
        }
            

        // Select Player
        public void menuSelect()
        {

        // Set the title name
        title.GetComponent<TMPro.TMP_Text>().text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMPro.TMP_Text>().text;

        // Set the button name
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string c = buttonName.Substring(buttonName.Length - 1);
        int index = int.Parse(c) - 1;

        // Set the Character visibility
        characterList = new GameObject[CharacterItem.transform.childCount];

        // Fill  the array with our models
        for (int i = 0; i < CharacterItem.transform.childCount; i++)
            characterList[i] = CharacterItem.transform.GetChild(i).gameObject;

        // We toggle off the characters
        foreach (GameObject character in characterList)
            character.SetActive(false);

        // We toogle on the selected character
        if (characterList[index])
            characterList[index].SetActive(true);



    }
   

}