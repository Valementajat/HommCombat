using UnityEngine;

public class ObjectSize : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    public Quaternion  orientation;

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

     public Quaternion  GetOrientation()
    {
        return orientation;
    }

    public void SetOrientation(Quaternion  orientation)
    {
        this.orientation =  orientation;
    }


}
