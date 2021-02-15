using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventArgsLibrary;
using Utilities;

namespace GlobalPositioningNS
{
    public class GlobalPositioning
    {
        float posX=0, posY=0, Theta=0, Fech=50f;

        public void PolarSpeedProcessIntoPosition (object sender, PolarSpeedEventArgs e)
        {
            Theta += (float)e.Vtheta / Fech;
            posX += (float)e.Vx / Fech * (float)Math.Cos(Theta);
            posY += (float)e.Vx /Fech * (float)Math.Sin(Theta);

            Console.WriteLine("posX : " + posX + "  posY : " + posY + "  Theta : " + Theta);
        }

        /************************** Ouput events ************************************************/
        public event EventHandler<LocationArgs> OnPositionEvent;
        public virtual void OnPosition(int robotID, Location loc)
        {
            var handler = OnPositionEvent;
            if (handler != null)
            {
                handler(this, new LocationArgs
                {
                    RobotId = robotID,
                    Location = loc
                }); ;
            }
        }
    }    
}
