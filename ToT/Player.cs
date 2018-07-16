using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToT
{
    public class Player : Thing
    {
        public Vector2 Velocity;

        public void Initialize()
        {
            //thingBody.BodyType = BodyType.Dynamic;
            //thingBody.Friction = 10f;
            //thingBody.GravityScale = 0f;
            //thingBody.CollisionCategories = Category.Cat1; //assigning the entity to a category
            //thingBody.CollidesWith = Category.All; //which category will the entity collide with? i pick all in this case
            //thingBody.UserData = this; // just leave this be as it is for now
            //thingBody.Position = Position; // Sets the position of the object
        }

        public Player(string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;
            InitPlayer();
        }

        public void InitPlayer(string template = "base01")
        {
            stats.Add(StatType.HP.ToString(), 5f);
            stats.Add("+" + StatType.HP.ToString(), 0f);
            stats.Add(StatType.MoveSpeed.ToString(), 10f);
            stats.Add("+" + StatType.MoveSpeed.ToString(), 0f);
            stats.Add(StatType.UsedMove.ToString(), 0f);
            Velocity = Vector2.Zero;
        }

        public Vector2 FindClickedThing(Vector2 ClickedPosition)
        {
            Vector2 tV = new Vector2(999999, 999999);

            float x0, x1, y0, y1;

            foreach (KeyValuePair<Vector2, Tile> room in Stage)
            {
                x0 = room.Value.Position.X * (ScreenManager.TileSize.X + 1);
                x1 = x0 + ScreenManager.TileSize.X;
                y0 = room.Value.Position.Y * (ScreenManager.TileSize.Y + 1);
                y1 = y0 + ScreenManager.TileSize.Y;

                if (ClickedPosition.X >= x0 && ClickedPosition.X <= x1
                    && ClickedPosition.Y >= y0 && ClickedPosition.Y <= y1)
                {
                    tV = room.Key;
                }
            }

            return tV;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime);
            ScreenManager.GameUIs[UITemplate.toolbar01].Position = new Vector2(2, 2) + ScreenManager.PlayerCamera.Position;
            ScreenManager.GameUIs[UITemplate.income].Position = new Vector2(2 + ScreenManager.Resolution.X - ScreenManager.GameUIs[UITemplate.income].Size.X, 2) + ScreenManager.PlayerCamera.Position;
            ScreenManager.GameUIs[UITemplate.tileSheet].Position = ActiveRoom.RoomPosition + new Vector2(ScreenManager.TileSize.X - 28, 0);
            ScreenManager.GameUIs[UITemplate.buildUI].Position = ActiveRoom.RoomPosition + new Vector2(ScreenManager.TileSize.X - 28, 0);
            ScreenManager.GameUIs[UITemplate.improveUI].Position = ActiveRoom.RoomPosition + new Vector2(ScreenManager.TileSize.X - 28, 0);
            ScreenManager.GameUIs[UITemplate.tileExpendNorth].Position = ActiveRoom.RoomPosition + new Vector2((ScreenManager.TileSize.X / 2) - 14, -14);
            ScreenManager.GameUIs[UITemplate.tileExpendEast].Position = ActiveRoom.RoomPosition + new Vector2((ScreenManager.TileSize.X) - 14, (ScreenManager.TileSize.Y / 2) - 14);
            ScreenManager.GameUIs[UITemplate.tileExpendSouth].Position = ActiveRoom.RoomPosition + new Vector2((ScreenManager.TileSize.X / 2) - 14, (ScreenManager.TileSize.Y) - 14);
            ScreenManager.GameUIs[UITemplate.tileExpendWest].Position = ActiveRoom.RoomPosition + new Vector2(-14, (ScreenManager.TileSize.Y / 2) - 14);
            ScreenManager.GameUIs[UITemplate.turn01].Position = ScreenManager.Resolution - ScreenManager.GameUIs[UITemplate.turn01].Size + ScreenManager.PlayerCamera.Position;
            ScreenManager.GameUIs[UITemplate.log].Position = new Vector2(2, ScreenManager.Resolution.Y - ScreenManager.GameUIs[UITemplate.log].Size.Y) + ScreenManager.PlayerCamera.Position;
            if (ScreenManager.TTToggled)
            {
                ScreenManager.GameUIs[UITemplate.tooltip].ToDraw = true;
                ScreenManager.GameUIs[UITemplate.tooltip].Position = ScreenManager.Input.MousePosition() + ScreenManager.PlayerCamera.Position + new Vector2(5, -5);
            }
            else
                ScreenManager.GameUIs[UITemplate.tooltip].ToDraw = false;
            base.Update(gameTime);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            Vector2 tV;

            if (ScreenManager.Input.MouseRightPressed())
            {
                tV = FindClickedThing(ScreenManager.Input.MousePosition() + ScreenManager.PlayerCamera.Position);
                if (tV != new Vector2(999999, 999999))
                {
                    SetActiveRoom(Stage[tV]);
                }
            }
            else
            {
                if (ScreenManager.Input.KeyPressed(Keys.Right, Keys.D) || ScreenManager.Input.ButtonPressed(Buttons.DPadRight, Buttons.LeftThumbstickRight))
                {
                    tV = new Vector2(ActiveRoom.Position.X + 1, ActiveRoom.Position.Y);
                    if (Stage.ContainsKey(tV))
                    {
                        SetActiveRoom(Stage[tV]);
                    }
                }
                else if (ScreenManager.Input.KeyPressed(Keys.Left, Keys.A) || ScreenManager.Input.ButtonPressed(Buttons.DPadLeft, Buttons.LeftThumbstickLeft))
                {
                    tV = new Vector2(ActiveRoom.Position.X - 1, ActiveRoom.Position.Y);
                    if (Stage.ContainsKey(tV))
                    {
                        SetActiveRoom(Stage[tV]);
                    }
                }

                if (ScreenManager.Input.KeyPressed(Keys.Down, Keys.S) || ScreenManager.Input.ButtonPressed(Buttons.DPadDown, Buttons.LeftThumbstickDown))
                {
                    tV = new Vector2(ActiveRoom.Position.X, ActiveRoom.Position.Y + 1);
                    if (Stage.ContainsKey(tV))
                    {
                        SetActiveRoom(Stage[tV]);
                    }
                }
                else if (ScreenManager.Input.KeyPressed(Keys.Up, Keys.W) || ScreenManager.Input.ButtonPressed(Buttons.DPadUp, Buttons.LeftThumbstickUp))
                {
                    tV = new Vector2(ActiveRoom.Position.X, ActiveRoom.Position.Y - 1);
                    if (Stage.ContainsKey(tV))
                    {
                        SetActiveRoom(Stage[tV]);
                    }
                }
            }

            ScreenManager.PlayerCamera.SetFocalPoint(Position + (ScreenManager.TileSize / 2));
        }
    }
}
