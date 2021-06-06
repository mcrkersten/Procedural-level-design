namespace UnitsAndFormation {
    public enum CellType {
        none = 0,
        sand,
        grass,
        longGrass,
        shallowWater,
        deepWater,
        rock,
        snow,
        unpassable,
    }

    public static class CellCalculation {
        public static byte GetCellTypeCost(CellType type)
        {
            switch (type)
            {
                case CellType.none:
                    return byte.MaxValue;
                case CellType.sand:
                    return (byte)10;
                case CellType.grass:
                    return (byte)10;
                case CellType.longGrass:
                    return (byte)15;
                case CellType.shallowWater:
                    return (byte)30;
                case CellType.deepWater:
                    return (byte)200;
                case CellType.rock:
                    return (byte)75;
                case CellType.snow:
                    return (byte)100;
                case CellType.unpassable:
                    return byte.MaxValue;
                default:
                    return byte.MaxValue;
            }
        }
    }
}