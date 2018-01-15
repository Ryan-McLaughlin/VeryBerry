using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {

	public bool selected;
	public Sprite tool;
	public Sprite toolSelected;
	public Sprite[] berry;

	private GameObject playerScript;
	private bool active;

	public void OnMouseDown() {
		// deselect all tools before selecting one
		DeSelectTools ();
		DeActivateSeedSelection ();

		switch (gameObject.name) {
		case "Tool_Hoe":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().hoe;
				selected = true;
				break;
			}
		case "Tool_WaterCan":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().watercan;
				selected = true;
				break;
			}
		case "Tool_Mulch":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().mulch;
				selected = true;
				break;
			}
		case "Tool_Hand":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().hand;
				selected = true;
				break;
			}
		case "Tool_PlantSeed":
			{				
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().plantSeed;
				selected = true;
				break;
			}
		case "Tool_SeedSelect":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().seedSelect;
				selected = true;
				ActivateSeedSelection ();
				break;
			}
		case "Tool_RedSeed":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().plantSeed;
				playerScript.GetComponent<Player> ().seed = playerScript.GetComponent<Player> ().redSeed;
				GameObject.Find("Tool_PlantSeed").GetComponent<Tools>().selected = true;
				DeActivateSeedSelection ();
				break;
			}
		case "Tool_BlueSeed":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().plantSeed;
				playerScript.GetComponent<Player> ().seed = playerScript.GetComponent<Player> ().blueSeed;
				GameObject.Find("Tool_PlantSeed").GetComponent<Tools>().selected = true;
				DeActivateSeedSelection ();
				break;
			}
		case "Tool_GreenSeed":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().plantSeed;
				playerScript.GetComponent<Player> ().seed = playerScript.GetComponent<Player> ().greenSeed;
				GameObject.Find("Tool_PlantSeed").GetComponent<Tools>().selected = true;
				DeActivateSeedSelection ();
				break;
			}
		case "Tool_PurpleSeed":
			{
				playerScript.GetComponent<Player> ().tool = playerScript.GetComponent<Player> ().plantSeed;
				playerScript.GetComponent<Player> ().seed = playerScript.GetComponent<Player> ().purpleSeed;
				GameObject.Find("Tool_PlantSeed").GetComponent<Tools>().selected = true;
				DeActivateSeedSelection ();
				break;
			}
		}
	}

	private void DeSelectTools() {
		GameObject.Find("Tool_Hoe").GetComponent<Tools>().selected = false;
		GameObject.Find("Tool_WaterCan").GetComponent<Tools>().selected = false;
		GameObject.Find("Tool_Mulch").GetComponent<Tools>().selected = false;
		GameObject.Find("Tool_Hand").GetComponent<Tools>().selected = false;
		GameObject.Find("Tool_PlantSeed").GetComponent<Tools>().selected = false;
		GameObject.Find("Tool_SeedSelect").GetComponent<Tools>().selected = false;
	}

	private void ActivateSeedSelection() {
		GameObject.Find ("Tool_RedSeed").GetComponent<Tools> ().active = true;
		GameObject.Find ("Tool_BlueSeed").GetComponent<Tools> ().active = true;
		GameObject.Find ("Tool_GreenSeed").GetComponent<Tools> ().active = true;
		GameObject.Find ("Tool_PurpleSeed").GetComponent<Tools> ().active = true;
	}

	private void DeActivateSeedSelection() {
		GameObject.Find ("Tool_RedSeed").GetComponent<Tools> ().active = false;
		GameObject.Find ("Tool_BlueSeed").GetComponent<Tools> ().active = false;
		GameObject.Find ("Tool_GreenSeed").GetComponent<Tools> ().active = false;
		GameObject.Find ("Tool_PurpleSeed").GetComponent<Tools> ().active = false;
	}

	void Start () {
		// this.GetComponent<SpriteRenderer> ().sprite = generic;
		playerScript = GameObject.Find ("Player");
		active = true;
		if (gameObject.name == "Tool_RedSeed" || gameObject.name == "Tool_BlueSeed" || gameObject.name == "Tool_GreenSeed" || gameObject.name == "Tool_PurpleSeed")
			active = false;
	}

	void Update () {		
		

		// added this to save time while constantly updating sprite sheet
		// condense during cleanup
		if (gameObject.name == "Tool_PlantSeed" || gameObject.name == "Tool_SeedSelect") {
			if (playerScript.GetComponent<Player> ().seed == playerScript.GetComponent<Player> ().redSeed) {
				if (!selected)
					this.GetComponent<SpriteRenderer> ().sprite = berry [0];
				else
					this.GetComponent<SpriteRenderer> ().sprite = berry [1];
			} else if (playerScript.GetComponent<Player> ().seed == playerScript.GetComponent<Player> ().blueSeed) {
				if (!selected)
					this.GetComponent<SpriteRenderer> ().sprite = berry [2];
				else
					this.GetComponent<SpriteRenderer> ().sprite = berry [3];
			} else if (playerScript.GetComponent<Player> ().seed == playerScript.GetComponent<Player> ().greenSeed) {
				if (!selected)
					this.GetComponent<SpriteRenderer> ().sprite = berry [4];
				else
					this.GetComponent<SpriteRenderer> ().sprite = berry [5];
			} else if (playerScript.GetComponent<Player> ().seed == playerScript.GetComponent<Player> ().purpleSeed) {
				if (!selected)
					this.GetComponent<SpriteRenderer> ().sprite = berry [6];
				else
					this.GetComponent<SpriteRenderer> ().sprite = berry [7];
			}
		} else {
			if (!selected)
				this.GetComponent<SpriteRenderer> ().sprite = tool;
			else
				this.GetComponent<SpriteRenderer> ().sprite = toolSelected;
		}

		if (!active)
			this.GetComponent<SpriteRenderer> ().sprite = null;	
	}
}
