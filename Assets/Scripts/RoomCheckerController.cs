using UnityEngine;

public class RoomCheckerController: MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(RoomManager.GetCurrentRoomCenter(transform.position), new Vector3(31,17,5));
        Gizmos.DrawCube(transform.position, new Vector3(1,1,1));
    }
}