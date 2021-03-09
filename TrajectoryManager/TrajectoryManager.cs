using EventArgsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TrajectoryManagerNS
{
    public enum TrajectoryState
    {
        Idle,
        Rotation,
        Avance,
        Recule,
    }
    public class TrajectoryManager
    {
        float Fech = 50f;

        int robotId;

        PointD destination;

        double accelerationAngulaire = 1;
        double vitesseAngulaireGhost;
        double vitesseAngulaireMax;
        double angleGhost;
        double accelerationLineaire = 1;
        double vitesseLineaireGhost;
        double vitesseLineaireMax;
        double distanceGhost;
        double xGhost;
        double yGhost;
        //double angleCible;

        TrajectoryState trajectoryState = TrajectoryState.Idle;

        public TrajectoryManager(int id)
        {
            robotId = id;
        }
        
        public void UpdateGhost()
        {

            switch(trajectoryState)
            {
                case TrajectoryState.Idle:
                    break;
                case TrajectoryState.Rotation:
                    {
                        /// On calcule dans un premier temps la distance d'arret du ghost
                        double angleArretGhost = vitesseAngulaireGhost * vitesseAngulaireGhost / (2 * accelerationAngulaire);
                        /// Puis on calcule l'angle cible
                        double angleCible = Math.Atan2(destination.Y - yGhost, destination.X - xGhost);
                        /// puis on calcule l'angle restant à parcourir,
                        double angleRestant = angleCible - Toolbox.ModuloByAngle(angleCible, angleGhost);
                        /// On regarde si on peut accélérer ou si il faut freiner ou rester à vitesse constante
                        if (angleArretGhost < Math.Abs(angleRestant))
                        {
                            if (Math.Abs(vitesseAngulaireGhost) < vitesseAngulaireMax)
                            {
                                /// On peut accélérer
                                if (angleRestant > 0)
                                    vitesseAngulaireGhost += accelerationAngulaire * 1 / Fech;
                                else
                                    vitesseAngulaireGhost -= accelerationAngulaire * 1 / Fech;
                            }
                            else
                            {
                                ///Rien, on reste à la même vitesse
                            }
                        }
                        else
                        {
                            /// On doit freiner
                            if (angleRestant > 0)
                                vitesseAngulaireGhost -= accelerationAngulaire * 1 / Fech;
                            else
                                vitesseAngulaireGhost += accelerationAngulaire * 1 / Fech;
                        }
                    }
                    break;

                case TrajectoryState.Avance:
                    {
                        ///On calcule dans un premier temps la distance d'arret du ghost
                        double distanceArretGhost = vitesseLineaireGhost * vitesseLineaireGhost / (2 * accelerationLineaire);
                        ///Puis on calcule la distance cible
                        double distanceCible = Math.Sqrt(Math.Pow(destination.Y - yGhost, 2) + Math.Pow(destination.X - xGhost, 2));
                        ///Puis on calcule la distance restante à parcourir
                        double distanceRestante = distanceCible - distanceGhost;
                        /// On regarde si on peut accélérer ou si il faut freiner ou rester à vitesse constante
                        if (distanceArretGhost < Math.Abs(distanceRestante))
                        {
                            if (Math.Abs(vitesseLineaireGhost) < vitesseLineaireMax)
                            {
                                /// On peut accélérer
                                vitesseLineaireGhost += accelerationLineaire * 1 / Fech;
                            }
                            else
                            {
                                ///Rien, on reste à la même vitesse
                            }
                        }
                        else
                        {
                            /// On doit freiner
                           vitesseLineaireGhost -= accelerationAngulaire * 1 / Fech;
                        }
                    }
                    break;
                default:
                    break;
            }

            OnGhostLocation(robotId, new Location(xGhost, yGhost, angleGhost, vitesseLineaireGhost * Math.Cos(angleGhost), vitesseLineaireGhost * Math.Sin(angleGhost), vitesseAngulaireGhost));


        }

        void UpdateDestination(PointD destination)
        {
            /// On calcule l'angle de destination
            /// On suppose pour l'instant qu'on est en état Idle (pas de mouvement), donc pas de freinage nécessaire dans un premier temps
            this.destination = destination;

        }

        /*************************************** Incoming events ***********************************/
        public void PositionReceived(object sender, LocationArgs e)
        {
            UpdateGhost();
            //AsservissementPositionSurGhost(e.Location);

            //oscilloX.AddPointToLine(1, e.timeStampMs / 1000.0, e.Vx);
            //oscilloTheta.AddPointToLine(1, e.timeStampMs / 1000.0, e.Vtheta);
            //currentTime = e.timeStampMs / 1000.0;

            //asserv2WheelsSpeedDisplay.UpdatePolarOdometrySpeed(e.Vx, e.Vtheta);
        }

        public void DestinationReceived(object sender, LocationArgs e)
        {
            UpdateDestination(new PointD(e.Location.X, e.Location.Y));
        }

        /***************************************  Outgoing events ********************************/
        public event EventHandler<LocationArgs> OnGhostLocationEvent;
        public virtual void OnGhostLocation(int id, Location loc)
        {
            var handler = OnGhostLocationEvent;
            if (handler != null)
            {
                handler(this, new LocationArgs { RobotId = id, Location = loc });
            }
        }
    }
}
