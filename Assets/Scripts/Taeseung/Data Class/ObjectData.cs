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

    public static bool IsAssociationLightColor(EObjectColorType colorType, EObjectColorType compareColorType)
    {
        HashSet<EObjectColorType> hs_tempColorSet = new();
        hs_tempColorSet.Add(colorType);
        if (colorType == EObjectColorType.Red) {
            int k = ((short)colorType) + ((short)EObjectColorType.Green) + 2;
            hs_tempColorSet.Add((EObjectColorType)k);
            k = ((short)colorType) + ((short)EObjectColorType.Blue) + 2;
            hs_tempColorSet.Add((EObjectColorType)k);
        }
        else if (colorType == EObjectColorType.Green)
        {
            int k = ((short)colorType) + ((short)EObjectColorType.Red) + 2;
            hs_tempColorSet.Add((EObjectColorType)k);
            k = ((short)colorType) + ((short)EObjectColorType.Blue) + 2;
            hs_tempColorSet.Add((EObjectColorType)k);
        }
        else if (colorType == EObjectColorType.Blue)
        {
            int k = ((short)colorType) + ((short)EObjectColorType.Red) + 2;
            hs_tempColorSet.Add((EObjectColorType)k);
            k = ((short)colorType) + ((short)EObjectColorType.Green) + 2;
            hs_tempColorSet.Add((EObjectColorType)k);
        }

        return hs_tempColorSet.Contains(compareColorType);
    }

    public static (EObjectColorType, EObjectColorType) NoChoiceColor(EObjectColorType teamColor1, EObjectColorType teamColor2)
    {

        short sTC1 = (short)teamColor1;
        short sTC2 = (short)teamColor2;
        return ((EObjectColorType)(3 - (sTC1 + sTC2)), (EObjectColorType)sTC1 + sTC2 + 2);
    }

}



