using UnityEngine;
using System.Collections;

namespace DeckOfCards.GameCore {

	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class DCCard : MonoBehaviour {
		// Public Variables	
		[SerializeField] private GameObject cardHighlight;

		// Private Variables	
		private SpriteRenderer cardRenderer;
		private Collider2D cardCollider;

		private Vector3 originalPosition;
		private Vector3 targetPosition;

		// Static Variables

		private void Awake() {
			if (cardRenderer == null) {
				cardRenderer = GetComponent<SpriteRenderer>();
			}

			if (cardCollider == null) {
				cardCollider = GetComponent<Collider2D>();
			}
		}

		private void OnEnable() {
			targetPosition = Vector3.zero;

			SetCardHighlightActive(false);
		}

		public void SetCardActive(bool active) {
			gameObject.SetActive(active);
		}

		public void SetCardEnable(bool enable) {
			cardRenderer.enabled = enable;
		}

		public void SetCardHighlightActive(bool active) {
			if (cardHighlight == null)
				return;

			cardHighlight.SetActive(active);
		}

		public void SetCardImage(Sprite image) {
			if (cardRenderer == null) {
				cardRenderer = GetComponent<SpriteRenderer>();
			}

			cardRenderer.sprite = image;
		}

		public void SetCardLayer(int layer) {
			gameObject.layer = layer;
		}

		public void SetCardSortingLayer(int layer) {
			if (cardRenderer == null) {
				cardRenderer = GetComponent<SpriteRenderer>();
			}

			cardRenderer.sortingOrder = layer;
		}

		public void SetCardPosition(Vector3 position) {
			targetPosition = position;
			SetCardMoveTowardsTarget();
		}

		public void SetCardMoveTowardsTarget(float speed = 5f) {
			StartCoroutine(UpdateCardPosition(speed));
		}

		private IEnumerator UpdateCardPosition(float speed) {
			float timeStep = 0f;
			float distance = Vector3.Distance(transform.position, targetPosition);

			while (distance > 0f) {
				transform.position = Vector3.Lerp(transform.position, targetPosition, timeStep);
				distance = Vector3.Distance(transform.position, targetPosition);
				timeStep += Time.deltaTime * speed;
				yield return null;
			}
		}

		public void UpdateOriginalPosition() {
			originalPosition = transform.position;
		}

		public void ResetOriginalPosition() {
			SetCardPosition(originalPosition);
		}
	}
}