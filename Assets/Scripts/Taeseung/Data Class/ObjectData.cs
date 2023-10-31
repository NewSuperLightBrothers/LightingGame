using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectColorType
{
    Red,
    Green,
    Blue,
    Yellow,
    Magenta,
    Cyan,
    White,
    Black,
}


public static class ObjectData 
{
    public static Dictionary<EObjectColorType, Color> d_objectColor
    {
        get
        {
            if (_objectColor == null) {
                _objectColor = new();
                _objectColor.Add(EObjectColorType.Red, Color.red);
                _objectColor.Add(EObjectColorType.Green, Color.green);
                _objectColor.Add(EObjectColorType.Blue, Color.blue);
                _objectColor.Add(EObjectColorType.Yellow, Color.yellow);
                _objectColor.Add(EObjectColorType.Magenta, Color.magenta);
                _objectColor.Add(EObjectColorType.Cyan, Color.cyan);
                _objectColor.Add(EObjectColorType.White, Color.white);
                _objectColor.Add(EObjectColorType.Black, Color.black);
            }


            return _objectColor;
        }
        private set {}
    }

    private static Dictionary<EObjectColorType, Color> _objectColor;

}



