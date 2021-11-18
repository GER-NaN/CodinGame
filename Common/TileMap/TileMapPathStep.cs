namespace Common.TileMap
{
    public class TileMapPathStep<T>
    {
        public TileMapPathStep(int stepNumber, Tile<T> tile)
        {
            StepNumber = stepNumber;
            Tile = tile;
        }

        public int StepNumber { get; }
        public Tile<T> Tile { get; }

    }
}
