﻿namespace Infusion.LegacyApi
{
    public class NullUltimaClientWindow : IUltimaClientWindow
    {
        public void SetTitle(string title)
        {
        }

        public WindowBounds? GetBounds() => null;
        public void Focus()
        {
        }
    }
}