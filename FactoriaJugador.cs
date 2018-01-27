using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class FactoriaJugador
    {
        private static int numeroJugador = 0;
        public static Participante NewInstance(Jugadores jugador, int nivel)
        {
            if (jugador == Jugadores.PERSONA)
                return Persona.getInstance(++numeroJugador);

            if (nivel == 1)
                return OrdenadorNivel1.getInstance(++numeroJugador);
            else if (nivel == 2)
                return OrdenadorNivel2.getInstance(++numeroJugador);
            else
                return OrdenadorNivel3.getInstance(++numeroJugador);
        }
    }
}
