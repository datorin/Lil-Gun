using UnityEngine;

public static class RoomManager
{
    public const int XOffset = -3;
    public const int YOffset = 9;

    public const int XRoomSize = 31;
    public const int YRoomSize = 17;
    
    public static Vector2 GetRoom(Vector2 position)
    {
        var xPos = 0;
        while (position.x > xPos * XRoomSize + XOffset)
        {
            xPos++;
        }

        var yPos = (int) (position.y - YOffset) / YRoomSize;

        if (yPos >= 0 && position.y > YRoomSize / 2f)
        {
            yPos++;
        }
        
        return new Vector2(xPos, yPos+1);
    }

    public static Vector2 GetCurrentRoomCenter(Vector2 position)
    {
        var room = GetRoom(position);
        Debug.Log(room);
        return new Vector2(room.x * XRoomSize + XOffset - XRoomSize / 2f, room.y * (YRoomSize) - YOffset - YRoomSize / 2f);
    }
}