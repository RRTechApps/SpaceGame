﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	//This script belongs on the main canvas of the player and controls all UI elements.

	public GameObject scoreboard;
	public GameObject scoreboardPlayerPrefab;
	public Transform playersTransform;
	public GameObject pauseMenu;

	private int numAdded;
	private bool localPause;
	private Text[] textObjects;
	private Image[] imageObjects;
	private Transform changePanel;


	//Initialization
	void Start () {
		localPause = false;
		numAdded = 0;
		changePanel = pauseMenu.transform.Find("ChangePanel");
		textObjects = FindObjectsOfType<Text>();
		imageObjects = FindObjectsOfType<Image>();
		scoreboard.SetActive(false);
		//Assigning buttons
		pauseMenu.transform.Find("ResumeButton").GetComponent<Button>().onClick.AddListener(() => onResumeButton());
		pauseMenu.transform.Find("TeamButton").GetComponent<Button>().onClick.AddListener(() => onTeamButton());
		pauseMenu.transform.Find("NameButton").GetComponent<Button>().onClick.AddListener(() => onNameButton());
		pauseMenu.transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(() => onQuitButton());
		changePanel.transform.Find("DoneButton").GetComponent<Button>().onClick.AddListener(() => onDoneButton());
		pauseMenu.SetActive(false);

		changePanel.gameObject.SetActive(false);
	}

	//[ClientRpc]
	//public void RpcUpdateText(string textObjName, string newText){
	public void updateText(string textObjName, string newText){
		foreach(Text textObj in textObjects) {
			if(textObj.name.Equals(textObjName)) {
				textObj.text = newText;
			}
		}
	}

	//[ClientRpc]
	//public void RpcUpdateImage(string imgObjName, Material newMaterial){
	public void updateImage(string imgObjName, Material newMaterial){
		foreach(Image imgObj in imageObjects) {
			if(imgObj.name.Equals(imgObj)){
				imgObj.material = newMaterial;
			}
		}
	}

	//[ClientRpc]
	//public void RpcUpdateImage(string imgObjName, Vector2 newSize){
	public void updateImage(string imgObjName, Vector2 newSize){
		foreach(Image imgObj in imageObjects) {
			if(imgObj.name.Equals(imgObj)){
				imgObj.rectTransform.sizeDelta = newSize;
			}
		}
	}

	//[ClientRpc]
	//public void RpcUpdateImage(string imgObjName, Material newMaterial, Vector2 newSize){
	public void updateImage(string imgObjName, Material newMaterial, Vector2 newSize){
		foreach(Image imgObj in imageObjects) {
			if(imgObj.name.Equals(imgObj)){
				imgObj.material = newMaterial;
				imgObj.rectTransform.sizeDelta = newSize;
			}
		}
	}

	public void setScoreboardVisible(bool show){
		scoreboard.SetActive(show);
	}
	//[ClientRpc]
	//public void RpcAddScoreboardEntry(string name, Color color){
	public void addScoreboardEntry(string name, Color color){
		scoreboard.SetActive(true);
		scoreboard.GetComponent<RectTransform>().sizeDelta += new Vector2(0.0f, 10.0f);
		scoreboard.transform.Find("VertDivider").gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0.0f, 10.0f);
		scoreboard.SetActive(false);
		GameObject newScoreboardEntry = Instantiate(scoreboardPlayerPrefab, playersTransform, false) as GameObject;
		newScoreboardEntry.name = name;
		newScoreboardEntry.GetComponent<Text>().text = name;
		newScoreboardEntry.transform.localPosition = (new Vector3(-65.0f, -5.0f - numAdded * 10.0f, 0.0f));
		newScoreboardEntry.transform.Find("PlayerBackground").gameObject.GetComponent<Image>().color = color;
		numAdded++;
	}

	//[ClientRpc]
	//public void RpcUpdateScoreboardEntry(string name, int score){
	public void updateScoreboardEntry(string name, int score){
		playersTransform.Find(name).Find("PlayerScore").GetComponent<Text>().text = score + "";

	}

	//[ClientRpc]
	//public void RpcRemoveScoreboardEntry(string name){
	public void removeScoreboardEntry(string name){
		Destroy(playersTransform.Find(name).gameObject);
	}

	public void togglePauseMenuVisible(){
		localPause = !localPause;
		pauseMenu.SetActive(localPause);
	}

	public bool getLocalPause(){
		return localPause;
	}

	public void onResumeButton(){
		togglePauseMenuVisible();
	}

	public void onTeamButton(){
		changePanel.gameObject.SetActive(true);
		changePanel.Find("TeamSelect").gameObject.SetActive(true);
		changePanel.Find("NameInput").gameObject.SetActive(false);
	}

	public void onNameButton(){
		changePanel.gameObject.SetActive(true);
		changePanel.Find("NameInput").gameObject.SetActive(true);
		changePanel.Find("TeamSelect").gameObject.SetActive(false);
	}

	public void onDoneButton(){
		//TODO: Add code to send name change or color change to playermanager
		changePanel.Find("NameInput").gameObject.SetActive(false);
		changePanel.Find("TeamSelect").gameObject.SetActive(false);
		changePanel.gameObject.SetActive(false);
	}

	public void onQuitButton(){
		//TODO: Add code to quit the game
	}
}