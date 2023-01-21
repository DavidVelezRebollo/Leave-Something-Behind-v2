using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace LSB.Components.UI {
	public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
		[SerializeField] public bool IsLeftItem;
		
		public void OnPointerEnter(PointerEventData eventData) {
			if(IsLeftItem)
				transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.5f).SetLoops(-1, LoopType.Yoyo);
			else
				transform.DOScale(new Vector3(-1.2f, 1.2f, 1f), 0.5f).SetLoops(-1, LoopType.Yoyo);
		}


		public void OnPointerExit(PointerEventData eventData) {
			transform.DOKill();
			if (IsLeftItem)
				transform.DOScale(Vector3.one, 0.5f);
			else
				transform.DOScale(new Vector3(-1f, 1f, 1f), 0.5f);
		}
	}
	
}
