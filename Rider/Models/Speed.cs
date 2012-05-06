using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rider.Persistent;

namespace Rider.Models
{
    public class Speed
    {
        public enum Unit
        {
            Km = 0,
            Mi = 1
        }

        public static double MetersToUserSpeedUnit(double value)
        {
            Unit unit = UserData.Get<Speed.Unit>(UserData.UnitKey);
            switch (unit)
            {
                case Unit.Km:
                    return value * 3.6;
                case Unit.Mi:
                    return value * 2.23693629;
                default:
                    return value;
            }
        }

    }
}
