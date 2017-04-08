﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Rooms
{
    public class Player : Thing
    {
        int direction = 0;
        public Vector2 Velocity;

        public void Initialize()
        {
            thingBody.BodyType = BodyType.Dynamic;
            thingBody.Friction = 100f;
            thingBody.GravityScale = 0f;
            thingBody.CollisionCategories = Category.Cat2; //assigning the entity to a category
            thingBody.CollidesWith = Category.All; //which category will the entity collide with? i pick all in this case
            thingBody.UserData = this; // just leave this be as it is for now
            thingBody.Position = Position; // Sets the position of the object
        }

        public Player(World world, string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;
            //thingBody = BodyFactory.CreateRectangle(world, Rect.Width, Rect.Height, 1f, position);
            thingBody = BodyFactory.CreateCircle(world, Rect.Width / 2, 1f, position);
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

        private void DecreaseX()
        {
            if (Velocity.X > 0f)
            {
                Velocity.X -= 0.02f;
                if (Velocity.X < 0f)
                    Velocity.X = 0f;
            }
            else if (Velocity.X < 0f)
            {
                Velocity.X += 0.02f;
                if (Velocity.X > 0f)
                    Velocity.X = 0f;
            }
        }

        private void DecreaseY()
        {
            if (Velocity.Y > 0f)
            {
                Velocity.Y -= 0.02f;
                if (Velocity.Y < 0f)
                    Velocity.Y = 0f;
            }
            else if (Velocity.Y < 0f)
            {
                Velocity.Y += 0.02f;
                if (Velocity.Y > 0f)
                    Velocity.Y = 0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 tForce = Vector2.Zero;
            //if (ScreenManager.Input.KeyDown(Keys.Left, Keys.A) || ScreenManager.Input.ButtonDown(Buttons.DPadLeft, Buttons.LeftThumbstickLeft))
            //    tForce += new Vector2(-(force/5), 0);
            //if (ScreenManager.Input.KeyDown(Keys.Right, Keys.D) || ScreenManager.Input.ButtonDown(Buttons.DPadLeft, Buttons.LeftThumbstickRight))
            //    tForce += new Vector2(force/5, 0);
            //if (ScreenManager.Input.KeyPressed(Keys.Space) || ScreenManager.Input.ButtonPressed(Buttons.A))
            //{
            //    Jump();
            //}


            UpdateMovement(gameTime);

            thingBody.ApplyForce(tForce);

            base.Update(gameTime);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            if (ScreenManager.Input.KeyDown(Keys.Right, Keys.D) || ScreenManager.Input.ButtonDown(Buttons.DPadRight, Buttons.LeftThumbstickRight))
            {
                direction = 1;
                //moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);

                if (Velocity.X >= 0f)
                {
                    float tJumpSpeed;
                    tJumpSpeed = stats["movespeed"] + stats["+movespeed"];

                    Velocity.X += tJumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                    DecreaseX();
            }
            else if (ScreenManager.Input.KeyDown(Keys.Left, Keys.A) || ScreenManager.Input.ButtonDown(Buttons.DPadLeft, Buttons.LeftThumbstickLeft))
            {
                direction = 0;
                if (thingBody.Position.X + (-(stats["movespeed"] + stats["+movespeed"]) * (float)gameTime.ElapsedGameTime.TotalSeconds) > 0)
                {
                    //moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);

                    if (Velocity.X <= 0f)
                    {
                        float tJumpSpeed;
                        tJumpSpeed = stats["movespeed"] + stats["+movespeed"];

                        Velocity.X += -tJumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                        DecreaseX();
                }
            }
            else
                DecreaseX();

            if (ScreenManager.Input.KeyDown(Keys.Down, Keys.S) || ScreenManager.Input.ButtonDown(Buttons.DPadDown, Buttons.LeftThumbstickDown))
            {
                direction = 3;
                //moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);

                if (Velocity.Y >= 0f)
                {
                    float tJumpSpeed;
                    tJumpSpeed = stats["movespeed"] + stats["+movespeed"];

                    Velocity.Y += tJumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                    DecreaseY();
            }
            else if (ScreenManager.Input.KeyDown(Keys.Up, Keys.W) || ScreenManager.Input.ButtonDown(Buttons.DPadUp, Buttons.LeftThumbstickUp))
            {
                direction = 2;
                if (thingBody.Position.Y + (-(stats["movespeed"] + stats["+movespeed"]) * (float)gameTime.ElapsedGameTime.TotalSeconds) > 0)
                {
                    //moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);

                    if (Velocity.Y <= 0f)
                    {
                        float tJumpSpeed;
                        tJumpSpeed = stats["movespeed"] + stats["+movespeed"];

                        Velocity.Y += -tJumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                        DecreaseY();
                }
            }
            else
                DecreaseY();

            if (Velocity.X < 0)
                if (Velocity.X < -stats["maxspeed"] - stats["+maxspeed"])
                    Velocity.X = -stats["maxspeed"] - stats["+maxspeed"];
            if (Velocity.X > 0)
                if (Velocity.X > stats["maxspeed"] + stats["+maxspeed"])
                    Velocity = new Vector2(stats["maxspeed"] + stats["+maxspeed"], Velocity.Y);

            if (Velocity.Y < 0)
                if (Velocity.Y < -stats["maxspeed"] - stats["+maxspeed"])
                    Velocity.Y = -stats["maxspeed"] - stats["+maxspeed"];
            if (Velocity.Y > 0)
                if (Velocity.Y > stats["maxspeed"] + stats["+maxspeed"])
                    Velocity = new Vector2(Velocity.X, stats["maxspeed"] + stats["+maxspeed"]);


            thingBody.Position += Velocity;
            //if (HasBasicSkill("Speed Bonus")) stats["+movespeed"] = tSpeedBonus;
        }
    }
}
