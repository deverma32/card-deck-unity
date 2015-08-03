using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeckOfCards.GameCore {

	public class DCGameCardPool : MonoBehaviour {
		// Public Variables
		[SerializeField] private GameObject refCard;

		// Private Variables	
		private List<Sprite> cardSprites;
		private List<DCCard> cardList;

		// Static Variables
		private static DCGameCardPool instance;

		public static DCGameCardPool SharedInstance {
			get { return instance; }
		}

		public List<Sprite> CardSprites {
			get { return cardSprites; }
		}

		public List<DCCard> CardList {
			get { return cardList; }
		}

		private const string ASSET_PATH = "Assets/Textures/DeckOfCards.png";
		private const string CARD_NAME_FORMAT = "Card_{0}";

		public void Awake() {
			instance = this;
			PopulateCardPool();
			//ReadTexture();
		}

		private void PopulateCardPool() {
			// Load all child sprites under the spritesheet
			Object[] sprites = Resources.LoadAll("DeckOfCards");
			//Object[] sprites = Resources.LoadAllAssetsAtPath(ASSET_PATH);		// Problem in Compiling UnityEditor namespace

			cardSprites = new List<Sprite>();
			cardList = new List<DCCard>();

			// Loop through the sprites and create each one a gameobject reference
			foreach (Object obj in sprites) {
				Sprite sprite = obj as Sprite;

				if (sprite == null)
					continue;

				cardSprites.Add(sprite);

				GameObject spriteObj = UnityEngine.GameObject.Instantiate(refCard);
				spriteObj.name = string.Format(CARD_NAME_FORMAT, obj.name);
				spriteObj.transform.parent = transform;
				spriteObj.transform.position = new Vector3(1.5f, -5f, 0f);

				DCCard cSprite = spriteObj.GetComponent<DCCard>();
				if (cSprite == null) {
					cSprite = spriteObj.AddComponent<DCCard>();
				}
				cSprite.SetCardImage(sprite);

				cardList.Add(cSprite);
			}
		}

		private void ReadTexture() {
			//UnityEditor.AssetDatabase.LoadAllAssetsAtPath(ASSET_PATH);
			//Debug.Log(Directory.Exists("Assets/Textures/"));
			////string[] files = Directory.GetFiles("Assets/Textures/", "*", SearchOption.AllDirectories);

			//string[] files = File.ReadAllLines(ASSET_PATH, Encoding.Default);

			//foreach (string str in files) {
			//    Debug.Log(str);
			//}
		}
	}
}