using System;
using Microsoft.Phone.Controls.Maps;

namespace Rider.Maps
{
    public abstract class BaseTileSource : TileSource, 
        IEquatable<BaseTileSource>
    {
        public string Name { get; set; }


        public bool Equals(BaseTileSource other)
        {
            return other != null && other.Name.Equals(Name);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseTileSource);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
