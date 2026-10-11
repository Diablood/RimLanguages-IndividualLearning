using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimLanguages.IndividualLearning
{
    // Lifetime is exactly one TryInteractWith call, including nested interactions and exceptions.
    public sealed class InteractionPresentation
    {
        [ThreadStatic] private static InteractionPresentation current;
        public static InteractionPresentation Current => current;
        private readonly InteractionPresentation previous;
        private readonly Pawn initiator, recipient;
        private MoteBubble bubble;
        private Texture2D symbol;

        public InteractionPresentation(Pawn initiator, Pawn recipient)
        {
            this.initiator = initiator;
            this.recipient = recipient;
            previous = current;
            current = this;
        }
        public void End()
        {
            if (current == this) current = previous;
        }
        public void Capture(Pawn source, Pawn target, MoteBubble created, Texture2D icon)
        {
            if (source == initiator && target == recipient)
            {
                bubble = created;
                symbol = icon;
            }
        }
        public void MarkTransmission()
        {
            if (bubble == null || bubble.Destroyed || !bubble.Spawned || symbol == null) return;
            // Keep the original bubble, icon, target, duration and interaction record.
            bubble.SetupMoteBubble(symbol, recipient, new Color(0.45f, 0.9f, 1f));
        }
    }

    [HarmonyPatch(typeof(MoteMaker), nameof(MoteMaker.MakeInteractionBubble))]
    public static class CaptureInteractionBubblePatch
    {
        public static void Postfix(Pawn initiator, Pawn recipient, Texture2D symbol, MoteBubble __result) =>
            InteractionPresentation.Current?.Capture(initiator, recipient, __result, symbol);
    }
}
