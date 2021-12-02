using Common.TileMap;

namespace CrystalRush.BotStrategy
{
    //Searches an area and digs all Tiles that are not a hole
    public class DigEmptyAreaStrategy : IRobotStrategy
    {
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find the closest tile in my area, TODO: Make the search area more defined (better than just a Y coord)
            var destination = map.FindNearest(tile => tile.Item.IsHole == false && tile.Position.X > 6 && tile.Position.Y >= robot.YStart && tile.Position.Y < robot.YStart + 3, robot.Position);

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            //If we dont have any tiles in our area, go somehwere random
            else if (destination == null)
            {
                destination = map.Find(tile => tile.Item.IsHole == false && tile.Position.X > 0);
                action = $"MOVE {destination.Position.X} {destination.Position.Y} SEARCH:Rand";
            }
            //If we are not at our destiation, go there
            else if (robot.Position != destination.Position)
            {
                action = $"MOVE {destination.Position.X} {destination.Position.Y} SEARCH:Area";
            }
            //We're at our destination, Dig it
            else
            {
                action = $"DIG {robot.Position.X} {robot.Position.Y} DIG";
            }

            return action;
        }
    }
}
