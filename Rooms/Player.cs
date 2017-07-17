﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rooms
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
            stats.Add("hp", 5f);
            stats.Add("+hp", 0f);
            stats.Add("movespeed", 2f);
            stats.Add("+movespeed", 0f);
            stats.Add("maxspeed", 1.25f);
            stats.Add("+maxspeed", 0f);
            Velocity = Vector2.Zero;
        }

        public Vector2 FindClickedThing(Vector2 ClickedPosition)
        {
            Vector2 tV = new Vector2(999999, 999999);

            float x0, x1, y0, y1;

            foreach (KeyValuePair<Vector2, Room> room in Stage)
            {
                x0 = room.Value.Position.X * ScreenManager.TileSize.X;
                x1 = x0 + ScreenManager.TileSize.X;
                y0 = room.Value.Position.Y * ScreenManager.TileSize.Y;
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
            ScreenManager.GGPScreen.GameUIs[UITemplate.toolbar01].Position = new Vector2(2, 2) + ScreenManager.PlayerCamera.Position;
            ScreenManager.GGPScreen.GameUIs[UITemplate.income].Position = new Vector2(2 + ScreenManager.Resolution.X - 100, 2) + ScreenManager.PlayerCamera.Position;
            ScreenManager.GGPScreen.GameUIs[UITemplate.turn01].Position = new Vector2(ScreenManager.Resolution.X - ScreenManager.GGPScreen.GameUIs[UITemplate.turn01].Size.X, ScreenManager.Resolution.Y - ScreenManager.GGPScreen.GameUIs[UITemplate.turn01].Size.Y) + ScreenManager.PlayerCamera.Position;
            ScreenManager.GGPScreen.GameUIs[UITemplate.log].Position = new Vector2(2, ScreenManager.Resolution.Y - 180) + ScreenManager.PlayerCamera.Position;
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

            ScreenManager.PlayerCamera.SetFocalPoint(Position);
        }
    }
}
