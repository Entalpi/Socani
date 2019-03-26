using UnityEngine.EventSystems;
using UnityEngine;

public class SwipeController : EventTrigger {

  public override void OnBeginDrag(PointerEventData data) {
    Debug.Log("OnBeginDrag called.");
  }

  public override void OnCancel(BaseEventData data) {
    Debug.Log("OnCancel called.");
  }

  public override void OnDeselect(BaseEventData data) {
    Debug.Log("OnDeselect called.");
  }

  public override void OnDrag(PointerEventData data) {
    Debug.Log("OnDrag called.");
  }

  public override void OnDrop(PointerEventData data) {
    Debug.Log("OnDrop called.");
  }

  public override void OnInitializePotentialDrag(PointerEventData data) {
    Debug.Log("OnInitializePotentialDrag called.");
  }

  public override void OnMove(AxisEventData data) {
    Debug.Log("OnMove called.");
  }

  public override void OnPointerClick(PointerEventData data) {
    Debug.Log("OnPointerClick called.");
  }

  public override void OnPointerDown(PointerEventData data) {
    Debug.Log("OnPointerDown called.");
  }

  public override void OnPointerEnter(PointerEventData data) {
    Debug.Log("OnPointerEnter called.");
  }

  public override void OnPointerExit(PointerEventData data) {
    Debug.Log("OnPointerExit called.");
  }

  public override void OnPointerUp(PointerEventData data) {
    Debug.Log("OnPointerUp called.");
  }

  public override void OnScroll(PointerEventData data) {
    Debug.Log("OnScroll called.");
  }

  public override void OnSelect(BaseEventData data) {
    Debug.Log("OnSelect called.");
  }

  public override void OnSubmit(BaseEventData data) {
    Debug.Log("OnSubmit called.");
  }

  public override void OnUpdateSelected(BaseEventData data) {
    Debug.Log("OnUpdateSelected called.");
  }

  Player player;

  private void Start() {
    player = FindObjectOfType<Player>();
  }

  private enum Direction { Up, Down, Right, Left }

  public void OnEndDrag(PointerEventData eventData) {
    Debug.Log("Drag ended");

    Vector3 dragVector = (eventData.position - eventData.pressPosition).normalized;
    bool didMove = false;
    switch (swipeDirection(dragVector)) {
      case Direction.Up:
        player.animator.Play("Walking Up");
        didMove = player.board.validMove(gameObject, player.tile.boardPosition, Vector3Int.up);
        break;
      case Direction.Down:
        player.animator.Play("Walking Down");
        didMove = player.board.validMove(gameObject, player.tile.boardPosition, Vector3Int.down);
        break;
      case Direction.Left:
        player.animator.Play("Walking Left");
        didMove = player.board.validMove(gameObject, player.tile.boardPosition, Vector3Int.left);
        break;
      case Direction.Right:
        player.animator.Play("Walking Right");
        didMove = player.board.validMove(gameObject, player.tile.boardPosition, Vector3Int.right);
        break;
    }
    if (didMove) {
      player.board.endMove();
      player.timeSinceLastMove = 0.0f;
      player.board.CheckGamestate();
    }
  }

  private Direction swipeDirection(Vector3 dragVector) {
    float positiveX = Mathf.Abs(dragVector.x);
    float positiveY = Mathf.Abs(dragVector.y);
    if (positiveX > positiveY) {
      if (dragVector.x > 0) { return Direction.Right; } else { return Direction.Left; }
    } else {
      if (dragVector.y > 0) { return Direction.Up; } else { return Direction.Down; }
    }
  }
}
