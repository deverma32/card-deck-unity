using UnityEngine;
using System.Collections;

namespace DeckOfCards.GameCore {

	public class DCCardHolder : MonoBehaviour {
		// Public Variables	

		// Private Variables	
		private DCCard cardHeld;

		// Static Variables

		public DCCard CardHeld {
			get { return cardHeld; }
			set { cardHeld = value; }
		}
	}
}