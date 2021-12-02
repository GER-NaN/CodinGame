using Common.TileMap;

namespace CrystalRush.BotStrategy
{
    public interface IRobotStrategy
    {
        string GetMove(TileMap<CrystalRushCell> map, Robot robot);
    }
}
