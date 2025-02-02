using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ApollosTrickOrTreat
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {

        bool doCauldronSmoke = false;
        int cauldronTimer = 0;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            
            helper.Events.Player.Warped += this.OnWarped;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;

        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player warps to a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnWarped(object? sender, WarpedEventArgs e)
        {
            // If the local player warps to a new location that is ApolloHouse, set doCauldronSmoke bool to true
            if (e.IsLocalPlayer && e.NewLocation == Game1.getLocationFromName("Abagaianye.tricktreatCP_ApolloHouse"))
            {
                doCauldronSmoke = true;
            }
            else // If the local player warps to a new location that is NOT ApolloHouse, set doCauldronSmoke bool to false
            {
                doCauldronSmoke = false;
            }
        }

        /// <summary>Raised every tick.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>       
        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady) return; // If the world isn't ready, don't run the rest of the method.
            if (doCauldronSmoke == false) return; // If the player hasn't warped to ApolloHouse, don't run the rest of the method.

            if (e.IsMultipleOf(6))
            {
                cauldronTimer -= 1;
            }

            if (cauldronTimer <= 0)
            {
                Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(372, 1956, 10, 10), new Vector2(14f, 23f) * 64f + new Vector2(Game1.random.Next(-32, 64), Game1.random.Next(16)), flipped: false, 0.002f, Color.Violet)
                {
                    alpha = 0.75f,
                    motion = new Vector2(0f, -0.5f),
                    acceleration = new Vector2(-0.002f, 0f),
                    interval = 99999f,
                    layerDepth = 0.144f - (float)Game1.random.Next(100) / 10000f,
                    scale = 3f,
                    scaleChange = 0.01f,
                    rotationChange = (float)Game1.random.Next(-5, 6) * (float)Math.PI / 256f
                });
                cauldronTimer = 3;
            }
        }
    }
}
