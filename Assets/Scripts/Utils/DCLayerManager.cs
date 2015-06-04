using UnityEngine;
using System.Collections;

namespace DeckOfCards.Utility {

	public class DCLayerManager {
		// Static Variables
		private const string DEFAULT_LAYER = "Default";
		public static int DefaultLayer {
			get { return LayerMask.NameToLayer(DEFAULT_LAYER); ; }
		}

		private const string MAIN_CARD_LAYER = "Main Card";
		public static int MainCardLayer {
			get { return LayerMask.NameToLayer(MAIN_CARD_LAYER); }
		}
		
	}
}