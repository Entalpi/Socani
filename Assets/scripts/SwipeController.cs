using UnityEngine.EventSystems;
using UnityEngine;

public class SwipeController : EventTrigger {

  private enum Direction { Up, Down, Right, Left }

  public override void OnEndDrag(PointerEventData eventData) {
    Debug.Log("OnEndDrag ended");

    Player player = FindObjectOfType<Player>();
    if (player == null) { Debug.Log("Player null in SwipeController"); }

    Vector3 dragVector = (eventData.position - eventData.pressPosition).normalized;
    player.timeSinceLastMove += Time.deltaTime;
    if (player.timeSinceLastMove <= Player.MOVECOOLDOWN) { return; }
    bool didMove = false;
    switch (swipeDirection(dragVector)) {
      case Direction.Up:
        player.animator.Play("Walking Up");
        didMove = player.board.validMove(player.gameObject, player.tile.boardPosition, Vector3Int.up);
        break;
      case Direction.Down:
        player.animator.Play("Walking Down");
        didMove = player.board.validMove(player.gameObject, player.tile.boardPosition, Vector3Int.down);
        break;
      case Direction.Left:
        player.animator.Play("Walking Left");
        didMove = player.board.validMove(player.gameObject, player.tile.boardPosition, Vector3Int.left);
        break;
      case Direction.Right:
        player.animator.Play("Walking Right");
        didMove = player.board.validMove(player.gameObject, player.tile.boardPosition, Vector3Int.right);
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
