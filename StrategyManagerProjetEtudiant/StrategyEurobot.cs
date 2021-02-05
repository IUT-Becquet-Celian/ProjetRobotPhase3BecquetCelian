using Constants;
using EventArgsLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Utilities;
using WorldMap;

namespace StrategyManagerProjetEtudiantNS
{
    public class StrategyEurobot : StrategyGenerique
    {
        Stopwatch sw = new Stopwatch();

        public PointD robotDestination = new PointD(0, 0);
        PlayingSide playingSide = PlayingSide.Left;     

        TaskDemoMove taskDemoMove;
        TaskDemoMessage taskDemoMessage;

        //Timer configTimer;

        public StrategyEurobot(int robotId, int teamId, string multicastIpAddress) : base(robotId, teamId, multicastIpAddress)
        {
            taskDemoMove = new TaskDemoMove(this);
            taskDemoMessage = new TaskDemoMessage(this);
        }

        public override void InitStrategy()
        {
            //Obtenus directement à partir du script Matlab
            OnOdometryPointToMeter(1.178449e-06);
            On2WheelsAngleSetup(-1.570796e+00, 1.570796e+00);
            On2WheelsToPolarMatrixSetup(5.000000e-01, -5.000000e-01,
                                4.166667e+00, 4.166667e+00);
        }
                

        /*********************************** Events reçus **********************************************/
        

        /*********************************** Events de sortie **********************************************/
        
    }
       

}
