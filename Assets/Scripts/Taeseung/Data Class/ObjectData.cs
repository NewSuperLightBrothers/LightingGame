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


//공격 타입 (타입에 따라 파티클 처리 방식이 달라지기 때문, LaserParticleSystem에서 각 파티클마다 설정)
public enum ELaserGunType
{
    Beam,       //빔
    Bullet,     //탄알
    Blade,      //근접공격
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
        hs_tempColorSet = AssociationColorSet(colorType);
        return hs_tempColorSet.Contains(compareColorType);
    }


    public static HashSet<EObjectColorType> AssociationColorSet(EObjectColorType colorType)
    {
        HashSet<EObjectColorType> hs_tempColor = new();
        hs_tempColor.Add(colorType);
        if (colorType == EObjectColorType.Red)
        {
            int k = ((short)colorType) + ((short)EObjectColorType.Green) + 2;
            hs_tempColor.Add((EObjectColorType)k);
            k = ((short)colorType) + ((short)EObjectColorType.Blue) + 2;
            hs_tempColor.Add((EObjectColorType)k);
        }
        else if (colorType == EObjectColorType.Green)
        {
            int k = ((short)colorType) + ((short)EObjectColorType.Red) + 2;
            hs_tempColor.Add((EObjectColorType)k);
            k = ((short)colorType) + ((short)EObjectColorType.Blue) + 2;
            hs_tempColor.Add((EObjectColorType)k);
        }
        else if (colorType == EObjectColorType.Blue)
        {
            int k = ((short)colorType) + ((short)EObjectColorType.Red) + 2;
            hs_tempColor.Add((EObjectColorType)k);
            k = ((short)colorType) + ((short)EObjectColorType.Green) + 2;
            hs_tempColor.Add((EObjectColorType)k);
        }

        return hs_tempColor;
    }

    public static List<EObjectColorType> DivideColorList(EObjectColorType colorType)
    {
        List<EObjectColorType> l_tempColor = new();
        short scolorType = (short)colorType;

        if(scolorType - 2 <= 0) l_tempColor.Add(colorType);
        else
        {
            if (scolorType - 2 == 1) {
                l_tempColor.Add(EObjectColorType.Red);
                l_tempColor.Add(EObjectColorType.Green);
            }

            else if (scolorType - 2 == 2)
            {
                l_tempColor.Add(EObjectColorType.Red);
                l_tempColor.Add(EObjectColorType.Blue);
            }

            else if (scolorType - 2 == 3)
            {
                l_tempColor.Add(EObjectColorType.Green);
                l_tempColor.Add(EObjectColorType.Blue);
            }
        }

        return l_tempColor;
    }


    public static (EObjectColorType, EObjectColorType) NoChoiceColor(EObjectColorType teamColor1, EObjectColorType teamColor2)
    {
        short sTC1 = (short)teamColor1;
        short sTC2 = (short)teamColor2;
        return ((EObjectColorType)(3 - (sTC1 + sTC2)), (EObjectColorType)sTC1 + sTC2 + 2);
    }


    public static void SetColorType(ref EObjectColorType colorType, ref EObjectColorType setColorType)
    {
        colorType = setColorType;
    }
    

}



