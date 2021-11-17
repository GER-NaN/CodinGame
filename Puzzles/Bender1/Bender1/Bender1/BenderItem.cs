using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bender1
{
    //Elements of the City found on the map. These modify Benders behavior when he comes across them
    public enum BenderItem
    {
        Start = '@',
        SuicideBooth = '$',
        ObstacleBreakable = 'X',
        ObstacleUnbreakable = '#',
        ModifierSouth = 'S',
        ModifierNorth = 'N',
        ModifierEast = 'E',
        ModifierWest = 'W',
        CircuitInverter = 'I',
        Beer = 'B',
        Teleporter = 'T',
        Empty = ' '
    }
}
