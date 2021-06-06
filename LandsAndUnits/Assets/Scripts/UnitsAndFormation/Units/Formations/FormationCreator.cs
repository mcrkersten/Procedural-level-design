using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormationCreator
{
    public static List<Vector2> CreateFormation(int _units, float _scale, FormationType type)
    {
        switch (type)
        {
            case FormationType.standard:
                return CreateStandardArmyFormation(_units, _scale);
            case FormationType.randomInArea:
                return CreateRandomInAreaFormation(_units, _scale);
            case FormationType.harvestinArea:
                return null;
            default:
                return null;
        }
    }

    private static List<Vector2> CreateStandardArmyFormation(int _units, float _scale)
    {
        List<Vector2> positions = new List<Vector2>();
        int row = 0;
        int units = _units;

        #region MainBulk
        //Rows of 9
        if (units >= 24)
        {
            while (units > 9)
            {
                for (int i = 0; i < 9; i++)
                    positions.Add(new Vector2(-4 + i, row) * _scale);

                row++;
                units -= 9;
            }
        }
        //Rows of 6
        else if (units >= 15)
        {
            while (units > 6)
            {
                for (int i = 0; i < 6; i++)
                    positions.Add(new Vector2(-2.5f + i, row) * _scale);

                row++;
                units -= 6;
            }
        }
        //Rows of 5
        else if (units >= 12)
        {
            while (units > 5)
            {
                for (int i = 0; i < 5; i++)
                    positions.Add(new Vector2(-2f + i, row) * _scale);

                row++;
                units -= 5;
            }
        }
        //Rows of 4
        else if (units >= 6)
        {
            while (units > 4)
            {
                for (int i = 0; i < 4; i++)
                    positions.Add(new Vector2(-1.5f + i, row) * _scale);

                row++;
                units -= 4;
            }
        }
        //Rows of 3
        else
        {
            while (units > 3)
            {
                for (int i = 0; i < 3; i++)
                    positions.Add(new Vector2(-1 + i, row) * _scale);

                row++;
                units -= 3;
            }
        }
        #endregion

        #region RestUnits
        //Even
        if (units % 2 == 0)
        {
            float secondOffset = -((units / 2) - .5f);
            for (int i = 0; i < units; i++)
                positions.Add(new Vector2(secondOffset + i, row) * _scale);
        }
        //UnEven
        else
        {
            float secondOffset = -Mathf.FloorToInt(units / 2);
            for (int i = 0; i < units; i++)
                positions.Add(new Vector2(secondOffset + i, row) * _scale);
        }
        #endregion

        return positions;
    }

    private static List<Vector2> CreateRandomInAreaFormation(int _units, float _scale)
    {
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < _units; i++)
        {
            positions.Add(new Vector2(Random.Range(-5 , 6), Random.Range(-5, 6)));
        }
        return positions;
    }
}

public enum FormationType
{
    standard = 0,
    randomInArea,
    harvestinArea,
}
