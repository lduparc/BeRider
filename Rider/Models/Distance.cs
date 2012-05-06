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
    public class Distance
    {
        public static double MetersToUserDataUnit(double value)
        {
            Speed.Unit unit = UserData.Get<Speed.Unit>(UserData.UnitKey);
            if (unit == Speed.Unit.Km)
                return value / 1000;
            else if (unit == Speed.Unit.Mi)
                return value * 0.00062137;
            else 
                return value;
        }
    }
}
