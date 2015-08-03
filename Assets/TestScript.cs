using UnityEngine;
using System.Collections;
using PxlSq.Utility;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//PxlSqUtility.CreateAssetFolder("/ASD");
		Object[] objs = PxlSqUtility.GetImageChildren("/Textures/DeckOfCards_1.png");
		//foreach (Object obj in objs) {
		//    Debug.Log(obj.name);
		//}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
