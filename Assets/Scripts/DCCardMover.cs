using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DeckOfCards.Utility;

namespace DeckOfCards.GameCore {

	public class DCCardMover : MonoBehaviour {
		// Public Variables	
		[SerializeField] private DCCardHolder cardHolder;
		[SerializeField] private float cardSpacing = 0.75f;
		[SerializeField] private float cardYOffset = 5f;
		[SerializeField] private float minTouchMovement = 20f;

		// Private Variables	
		private int deckSize = 5;
		private int posOffset = 3;
		private int curScrollIndx = 0;
		private int scrollIndxOffset = -3;
		private List<DCCard> cardList;

		private int touchID = -1;
		private Vector2 startPos;

		// Static Variables

		private void Start() {
			cardList = new List<DCCard>();
			cardList.AddRange(DCGameCardPool.SharedInstance.CardList);

			UpdateCardPosition();
		}

		private void Update() {

# if UNITY_STANDALONE
			if (Input.GetMouseButtonDown(0)) {
				ActiveCard();
				startPos = Input.mousePosition;
			}

			if (Input.GetMouseButtonUp(0)) {
				Vector3 delta = Input.mousePosition - (Vector3)startPos;
				if (delta.magnitude > minTouchMovement) {
					if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
						if (delta.x > 0) {
							MoveDeckLeft();
							return;
						}

						if (delta.x < 0) {
							MoveDeckRight();
							return;
						}
					}
					else {
						if (delta.y > 0) {
							PlaceCardInHolder();
							return;
						}

						if (delta.y < 0) {
							RemoveCardInHolder();
							return;
						}
					}
				}

				InactiveCard();
			}
# endif

# if UNITY_ANDROID || UNITY_IPHONE
			foreach (Touch touch in Input.touches) {
				Vector2 pos = touch.position;
				if (touch.phase == TouchPhase.Began && touchID == -1) {
					touchID = touch.fingerId;
					startPos = pos;
					ActiveCard();
				}
				
				if (touch.fingerId == touchID) {
					Vector2 delta = pos - startPos;
					if (touch.phase == TouchPhase.Moved && delta.magnitude > minTouchMovement) {
						touchID = -1;
						if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
							// Swipe Right
							if (delta.x > 0) {
								MoveDeckLeft();
								return;
							}
							// Swipe Left
							else if (delta.x < 0) {
								MoveDeckRight();
								return;
							}
						}
						else {
							// Swipe Up
							if (delta.y > 0) {
								PlaceCardInHolder();
								return;
							}
							// Swipe Down
							else if (delta.y < 0) {
								RemoveCardInHolder();
								return;
							}
						}
					}

					if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) {
						InactiveCard();
					}
				}
				
				if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) {
					touchID = -1;
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
				return;
			}
# endif
		}

		private void UpdateCardPosition() {
			// Disable all active cards
			cardList.ForEach(delegate(DCCard a) {
				if (a.gameObject.activeInHierarchy) {
					a.gameObject.SetActive(false);
				}
			});

			if (cardList == null)
				return;

			int sizeOffset = deckSize + 1;
			curScrollIndx = scrollIndxOffset + posOffset;

			for (int i = 1; i < sizeOffset; i++) {
				if (i + scrollIndxOffset < 0 || (i + scrollIndxOffset) > (cardList.Count - 1))
					continue;

				DCCard card = cardList[i + scrollIndxOffset];

				if (card == null)
					continue;

				card.SetCardActive(true);

				// Middle Card
				if ((i + scrollIndxOffset) == curScrollIndx) {
					card.SetCardPosition(new Vector2(0f, 1f - cardYOffset));
					card.SetCardSortingLayer(2);
					card.SetCardLayer(DCLayerManager.MainCardLayer);
				}
				// Other Cards
				else {
					card.SetCardPosition(new Vector2((i - posOffset) * cardSpacing, -cardYOffset));
					card.SetCardLayer(DCLayerManager.DefaultLayer);
				}

				// Cards beside the middle card
				if ((i - posOffset) == 1 || (i - posOffset) == -1) {
					card.SetCardPosition(new Vector2((i - posOffset) * cardSpacing, 0.5f - cardYOffset));
					card.SetCardSortingLayer(1);
				}

				// Cards at the far end
				if ((i - posOffset) == 2 || (i - posOffset) == -2) {
					card.SetCardSortingLayer(0);
				}
			}
		}

		public void PlaceCardInHolder() {
			if (cardHolder.CardHeld != null)
				return;

			DCCard card = cardList[curScrollIndx];

			if (card == null)
				return;

			card.UpdateOriginalPosition();
			card.SetCardHighlightActive(false);
			card.SetCardPosition(cardHolder.transform.position);
			cardHolder.CardHeld = card;
			cardList.Remove(card);

			if (curScrollIndx == cardList.Count) {
				MoveDeckLeft();
			}

			UpdateCardPosition();
		}

		public void RemoveCardInHolder() {
			DCCard card = cardHolder.CardHeld;

			if (card == null || cardHolder.CardHeld == null)
				return;

			if (curScrollIndx == cardList.Count - 1) {
				MoveDeckRight();
			}

			cardHolder.CardHeld = null;
			card.ResetOriginalPosition();
			cardList.Insert(curScrollIndx, card);

			UpdateCardPosition();
		}

		public void ActiveCard() {
			DCCard card = cardList[curScrollIndx];

			if (card == null || cardHolder.CardHeld != null)
				return;

			card.SetCardHighlightActive(true);
			card.UpdateOriginalPosition();
			Vector3 pos = card.transform.position;
			pos.y = -3.5f;
			card.SetCardPosition(pos);
		}

		public void InactiveCard() {
			if (cardHolder.CardHeld != null)
				return;

			DCCard card = cardList[curScrollIndx];

			if (card == null)
				return;

			card.SetCardHighlightActive(false);
			card.ResetOriginalPosition();
		}

		public void MoveDeckLeft() {
			scrollIndxOffset--;
			scrollIndxOffset = Mathf.Clamp(scrollIndxOffset, -posOffset, DCGameCardPool.SharedInstance.CardList.Count - 4);
			UpdateCardPosition();
		}

		public void MoveDeckRight() {
			scrollIndxOffset++;
			scrollIndxOffset = Mathf.Clamp(scrollIndxOffset, -posOffset, DCGameCardPool.SharedInstance.CardList.Count - 4);
			UpdateCardPosition();
		}
	}
}