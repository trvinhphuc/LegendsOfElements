using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public void StartGame()
    {
        Application.LoadLevel("Main");

    }
	public void MenuGame()
	{
		Application.LoadLevel("menu");

	}
    public void OptionMenu()
    {
        Application.LoadLevel("option_menu");
    }
    public void AboutMenu()
    {
        Application.LoadLevel("about_menu");
    }

    public void HowplayMenu()
    {
        Application.LoadLevel("howplay_menu");
    }
		
}
