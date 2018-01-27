using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    static class Opciones
    {
        public static void SeleccionJugadores(out Jugadores pl1, out Jugadores pl2,
                out int nivel1, out int nivel2)
        {
            ElegirJugadores(out pl1, out pl2, out nivel1, out nivel2);
        }

        /// <summary>
        /// Elegimos los jugadores que van a jugar la partida
        /// </summary>
        /// <param name="pl1">Jugador 1</param>
        /// <param name="pl2">Jugador 2</param>
        /// <param name="nivel">Nivel elegido</param>
        private static void ElegirJugadores(out Jugadores pl1, out Jugadores pl2,
                out int nivel1, out int nivel2)
        {
            String jugador1, jugador2;

            /* Limpiamos buffer de entrada */
            Console.ReadLine();

            /* 0 por defecto, será el valor pasado en caso de jugador humano,
            valor que no se procesará */
            nivel1 = nivel2 = 0;

            Console.Write("Eleccion tipo de jugadores\n");
            Console.Write("(p)ersona / (o)rdenador)\n-----" + 
                "--------------------\n");

            do
            {
                Console.Write("Jugador 1: "); jugador1 = Console.ReadLine();
                pl1 = ComprobarOpcionJugador(jugador1);
            } while (pl1 == Jugadores.ERROR);
            if (pl1 == Jugadores.CPU) nivel1 = ElegirNivelCPU();
            do
            {
                Console.Write("Jugador 2: "); jugador2 = Console.ReadLine();
                pl2 = ComprobarOpcionJugador(jugador2);
            } while (pl2 == Jugadores.ERROR);
            if (pl2 == Jugadores.CPU) nivel2 = ElegirNivelCPU();
            
        }

        /// <summary>
        /// Elegimos el nivel del rival en caso de que sea el ordenador
        /// </summary>
        /// <returns>Nivel elegido</returns>
        private static int ElegirNivelCPU()
        {
            int numero = 0;
            String nivel;
            do
            {
                Console.WriteLine("Elegir nivel CPU (1-3): ");
                nivel = Console.ReadLine();
                Reglas.ComprobarEntero(nivel, out numero);
                if (numero == 3) Console.WriteLine("Aún no implementado!!!!");
            } while (numero < 1 || numero > 2);

            return numero;
        }

        /// <summary>
        /// Comprobamos la correcta eleccion del tipo de jugador
        /// </summary>
        /// <param name="jugador1"></param>
        /// <returns>Tipo de Jugadores seleccionado</returns>
        private static Jugadores ComprobarOpcionJugador(String jugador1)
        {
            if (jugador1.Equals("p") || jugador1.Equals("persona")) 
                return Jugadores.PERSONA;
            if (jugador1.Equals("o") || jugador1.Equals("ordenador") ||
                    jugador1.Equals("c"))
                return Jugadores.CPU;

            return Jugadores.ERROR;
        }

        
    }

    

}
