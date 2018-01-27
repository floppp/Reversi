using System;
using System.Collections.Generic;

namespace Reversi
{
    /* Enumeraciones a utilizar en todo el 'namespace' */
    public enum Jugadores { PERSONA = 1, CPU, ERROR };
    public enum Color { BLANCO = 219, NEGRO = 176, VACIO = 000 };

    /// <summary>
    /// Clase para inicial el juego, punto de entrada al programa
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Juego juego = Juego.Instance;
            juego.Partida();
        }
    }

    /// <summary>
    /// Clase principal para gestionar la partida.
    /// </summary>
    internal class Juego
    {
        private static Juego instance = null;
        internal Jugadores pl1, pl2;
        private Tablero tablero  = null;
        private int countBlancas;
        private int countNegras;
        private int countPasa;

        private Juego() { }

        public static Juego Instance
        {
            get 
            {
                if (instance == null)
                    instance = new Juego();
                return instance;
            }
        }

        /// <summary>
        /// Núcleo de la partida. Pedimos tipo de jugadores, vamos turnando
        /// entre ellos, etc.
        /// </summary>
        public void Partida()
        {
            Boolean terminada = false, pasan = false;
            int nivel1, nivel2;
            char[,] boardAnterior = new char[10, 10];

            tablero = new Tablero();
            tablero.Inicio();

            MostrarMensajeInicial();
            Opciones.SeleccionJugadores(out pl1, out pl2,
                        out nivel1, out nivel2);
            tablero.MostrarTablero();
            Participante jugador1 = FactoriaJugador.NewInstance(pl1, nivel1);
            Participante jugador2 = FactoriaJugador.NewInstance(pl2, nivel2);
            do
            {
                boardAnterior = (char[,]) tablero.GetBoard().Clone();
                Console.WriteLine("MOVIMIENTO JUGADOR 1 --------------\n");
                EjecutarJugada(jugador1);
                Console.WriteLine("MOVIMIENTO JUGADOR 2 --------------\n");
                EjecutarJugada(jugador2);
                ContarFichas();
                terminada = ComprobarFinDePartida();
                pasan = ComprobarPasaDosTurnosSeguidos(boardAnterior);
            } while (!terminada && !pasan);

            DeterminarGanador();
            Console.WriteLine("Presione tecla para salir...");
            Console.Read();
        }

        /// <summary>
        /// Mostramos un pequeño mensaje inicial de bienvenida con una ligera
        /// explicación de las reglas del juego
        /// </summary>
        private void MostrarMensajeInicial()
        {
            Console.WriteLine("-----Bienvenido al OTHELLO-----");
            Console.WriteLine("Las reglas del juego son:");
            Console.WriteLine(" 1. Puedes colocar piezas si hay piezas" + 
                " del contrario\n    a tu alrededor");
            Console.WriteLine(" 2. Comienza moviento el jugador con piezas" +
                " negras");
            Console.WriteLine(" 3. Puedes decidir pasar turno. Para ello" +
                " has de introducir\n    la posición (0, 0)");
            Console.WriteLine(" 4. Gana el jugador que tiene más piezas" +
                " cuando se llena\n    el tablero o cuando los dos jugadores" +
                "\n    han pasado turno de forma consecutiva\n");
            Console.WriteLine("\nSUERTE!!!!!\n");
            Console.WriteLine("Pulsa una tecla para continuar...");
            Console.Read();
        }
        /// <summary>
        /// Pasamos el turno al jugador al que le toque.
        /// </summary>
        /// <param name="jugador">Jugador que tiene el turno</param>
        private void EjecutarJugada(Participante jugador)
        {
            Console.WriteLine("Turno del jugador " + jugador.GetColor().ToString());
            jugador.Movimiento(tablero);
        }

        /// <summary>
        /// Método para contar el número de fichas de cada color que hay sobre
        /// el tablero
        /// </summary>
        private void ContarFichas()
        {
            this.countBlancas = this.countNegras = 0;

            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    if (tablero.GetBoard()[row, col] == (char)Color.BLANCO)
                        countBlancas++;
                    else if (tablero.GetBoard()[row, col] == (char)Color.NEGRO)
                        countNegras++;
        }

        /// <summary>
        /// Comprobamos el final de partida. Si la suma de todas las piezas 
        /// colocadas es 64, la partida ha terminado. En caso contrario
        /// continúa
        /// </summary>
        /// <returns>true si la partida termina, false si continúa</returns>
        private Boolean ComprobarFinDePartida()
        {
            if ((countBlancas + countNegras) == 64) return true;

            return false;
        }

        /// <summary>
        /// Comprobamos si en dos turnos consecutivos han pasado ambos
        /// jugadores, y en caso afirmativo terminamos la partida
        /// </summary>
        /// <param name="board">Tablero de juego en el estado anterior
        /// al estado en que se hace la comprobación</param>
        /// <returns>Booleano que nos confirma si se llevan dos turnos
        /// seguidos pasando</returns>
        private Boolean ComprobarPasaDosTurnosSeguidos(char[,] board)
        {
            Boolean pasan = true;
            countPasa++;
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    if (tablero.GetBoard()[row, col] != board[row, col])
                    {
                        /* Si alguna posición es distinta, es que alguien no
                        ha pasado, luego podemos salir de la comprobación y
                        reiniciar la cuenta de veces seguidas que se ha pasado
                        turno */
                        countPasa = 0;
                        break;
                    }
            if (countPasa < 2) pasan = false;

            return pasan;
        }

        /// <summary>
        /// Mostramos mensaje con el resultado de la partida
        /// </summary>
        private void DeterminarGanador()
        {
            if (countBlancas > countNegras)
                Console.WriteLine("El ganador es el jugador 2");
            else if (countBlancas < countNegras)
                Console.WriteLine("El ganador es el jugador 1");
            else
                Console.WriteLine("Empate");
        }
    }
}
