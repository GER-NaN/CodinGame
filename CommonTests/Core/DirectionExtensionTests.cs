using System;
using Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests
{
    [TestClass]
    public class DirectionExtensionTests
    {
        [TestMethod]
        public void TestPrint()
        {
            Assert.IsTrue(Direction.Up.Print() == "UP","Direction.Up printed something wrong");
            Assert.IsTrue(Direction.Down.Print() == "DOWN", "Direction.Down printed something wrong");
            Assert.IsTrue(Direction.Left.Print() == "LEFT", "Direction.LEFT printed something wrong");
            Assert.IsTrue(Direction.Right.Print() == "RIGHT", "Direction.Right printed something wrong");
            Assert.IsTrue(Direction.Top.Print() == "TOP", "Direction.TOP printed something wrong");
            Assert.IsTrue(Direction.Bottom.Print() == "BOTTOM", "Direction.Bottom printed something wrong");
            Assert.IsTrue(Direction.North.Print() == "NORTH", "Direction.North printed something wrong");
            Assert.IsTrue(Direction.South.Print() == "SOUTH", "Direction.South printed something wrong");
            Assert.IsTrue(Direction.East.Print() == "EAST", "Direction.East printed something wrong");
            Assert.IsTrue(Direction.West.Print() == "WEST", "Direction.West printed something wrong");
        }

        [TestMethod]
        public void TestOpposite()
        {
            Assert.IsTrue(Direction.Up.Opposite() == Direction.Down, "Direction.Up");
            Assert.IsTrue(Direction.Down.Opposite() == Direction.Up, "Direction.Down");
            Assert.IsTrue(Direction.Left.Opposite() == Direction.Right, "Direction.Left");
            Assert.IsTrue(Direction.Right.Opposite() == Direction.Left, "Direction.Right");
            Assert.IsTrue(Direction.Top.Opposite() == Direction.Bottom, "Direction.Top ");
            Assert.IsTrue(Direction.Bottom.Opposite() == Direction.Top, "Direction.Bottom");
            Assert.IsTrue(Direction.North.Opposite() == Direction.South, "Direction.North");
            Assert.IsTrue(Direction.South.Opposite() == Direction.North, "Direction.South");
            Assert.IsTrue(Direction.East.Opposite() == Direction.West, "Direction.East");
            Assert.IsTrue(Direction.West.Opposite() == Direction.East, "Direction.West");
        }

        [TestMethod]
        public void TestRelativeLeft()
        {
            Assert.IsTrue(Direction.Up.RelativeLeft() == Direction.Left, "Direction.Up");
            Assert.IsTrue(Direction.Down.RelativeLeft() == Direction.Right, "Direction.Down");
            Assert.IsTrue(Direction.Left.RelativeLeft() == Direction.Down, "Direction.Left");
            Assert.IsTrue(Direction.Right.RelativeLeft() == Direction.Up, "Direction.Right");
            Assert.IsTrue(Direction.North.RelativeLeft() == Direction.West, "Direction.North");
            Assert.IsTrue(Direction.South.RelativeLeft() == Direction.East, "Direction.South");
            Assert.IsTrue(Direction.East.RelativeLeft() == Direction.North, "Direction.East");
            Assert.IsTrue(Direction.West.RelativeLeft() == Direction.South, "Direction.West");
        }

        [TestMethod]
        public void TestRelativeRight()
        {
            Assert.IsTrue(Direction.Up.RelativeRight() == Direction.Right, "Direction.Up");
            Assert.IsTrue(Direction.Down.RelativeRight() == Direction.Left, "Direction.Down");
            Assert.IsTrue(Direction.Left.RelativeRight() == Direction.Up, "Direction.Left");
            Assert.IsTrue(Direction.Right.RelativeRight() == Direction.Down, "Direction.Right");
            Assert.IsTrue(Direction.North.RelativeRight() == Direction.East, "Direction.North");
            Assert.IsTrue(Direction.South.RelativeRight() == Direction.West, "Direction.South");
            Assert.IsTrue(Direction.East.RelativeRight() == Direction.South, "Direction.East");
            Assert.IsTrue(Direction.West.RelativeRight() == Direction.North, "Direction.West");
        }

        [TestMethod]
        public void TestStringToDirection()
        {
            Assert.IsTrue("Up".ToDirection() == Direction.Up);
            Assert.IsTrue("UP".ToDirection() == Direction.Up);
            Assert.IsTrue("up".ToDirection() == Direction.Up);

            Assert.IsTrue("Down".ToDirection() == Direction.Down);
            Assert.IsTrue("DOWN".ToDirection() == Direction.Down);
            Assert.IsTrue("down".ToDirection() == Direction.Down);

            Assert.IsTrue("Left".ToDirection() == Direction.Left);
            Assert.IsTrue("LEFT".ToDirection() == Direction.Left);
            Assert.IsTrue("left".ToDirection() == Direction.Left);

            Assert.IsTrue("Right".ToDirection() == Direction.Right);
            Assert.IsTrue("RIGHT".ToDirection() == Direction.Right);
            Assert.IsTrue("right".ToDirection() == Direction.Right);

            Assert.IsTrue("Top".ToDirection() == Direction.Top);
            Assert.IsTrue("TOP".ToDirection() == Direction.Top);
            Assert.IsTrue("top".ToDirection() == Direction.Top);

            Assert.IsTrue("Bottom".ToDirection() == Direction.Bottom);
            Assert.IsTrue("BOTTOM".ToDirection() == Direction.Bottom);
            Assert.IsTrue("bottom".ToDirection() == Direction.Bottom);

            Assert.IsTrue("North".ToDirection() == Direction.North);
            Assert.IsTrue("NORTH".ToDirection() == Direction.North);
            Assert.IsTrue("north".ToDirection() == Direction.North);

            Assert.IsTrue("South".ToDirection() == Direction.South);
            Assert.IsTrue("SOUTH".ToDirection() == Direction.South);
            Assert.IsTrue("south".ToDirection() == Direction.South);

            Assert.IsTrue("East".ToDirection() == Direction.East);
            Assert.IsTrue("EAST".ToDirection() == Direction.East);
            Assert.IsTrue("east".ToDirection() == Direction.East);

            Assert.IsTrue("West".ToDirection() == Direction.West);
            Assert.IsTrue("WEST".ToDirection() == Direction.West);
            Assert.IsTrue("west".ToDirection() == Direction.West);
        }
    }
}
