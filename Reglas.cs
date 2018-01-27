using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    /// <summary>
    /// Clase con métodos para validar los movimientos realizados por
    /// los jugadores
    /// </summary>
    static class Reglas
    {
        /// <summary>
        /// Comprobación de que la posición elegida por el jugador está
        /// dentro del tablero
        /// </summary>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        /// <returns>Booleano indicando si está dentro del tablero
        /// </returns>
        public static Boolean PosicionDentroTablero(int row, int col)
        {
            if (row < 1 || row > 8) return false;
            if (col < 1 || col > 8) return false;

            return true;
        }

        /// <summary>
        /// Comprobación mediante las reglas del juego de que la posición
        /// elegida por el jugador es legal
        /// </summary>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        /// <param name="color">Color del jugador del cuál estamos comprobando
        /// la jugada</param>
        /// <returns>Booleano indicando si se cumplen o no las reglas
        /// del juego</returns>
        public static Boolean PosicionLegal(Tablero tablero, int row, int col, Color color)
        {
            /* Lo primero, rechazamos el movimiento si la posición ya está
            ocupada */
            if (tablero.GetBoard()[row, col] != (char)Color.VACIO) return false;

            /* Ahora contamos número de fichas blancas y negras alrededor de
            la posición que estamos comprobando para usarlo después */
            int countBlancas = 0, countNegras = 0;
            for (int i = row - 1; i < row + 2; i++)
                for (int j = col - 1; j < col + 2; j++)
                {
                    if (i == row && j == col) continue;
                    if (tablero.GetBoard()[i, j] == (char)Color.BLANCO)
                        countBlancas++;
                    else if (tablero.GetBoard()[i, j] == (char)Color.NEGRO)
                        countNegras++;
                }
            /* Si tenemos alrededor de la ficha tanto fichas blancas como
            negras, el movimiento siempre será válido */
            if (countBlancas > 0 && countNegras > 0) return true;
            /* Si alrededor no hay fichas, posición incorrecta */
            if (countBlancas == 0 && countNegras == 0) return false;
            /* Comprobación de la legalidad del movimiento para cada jugador
            en función de su color */
            if (color.Equals(Color.BLANCO) && countNegras > 0) return true;
            if (color.Equals(Color.NEGRO) && countBlancas > 0) return true;

            return false;
        }

        /// <summary>
        /// Pedimos un entero y comprobamos que la cadena introduce en efecto
        /// representa un entero, y lo escribimos en la dirección de memoria
        /// de la variable que queremos parsear
        /// </summary>
        /// <param name="numero">Número a escribir</param>
        /// <param name="dimension">Fila o columna</param>
        /// <returns>Booleano diciendo si el valor introducido es o no un
        /// entero</returns>
        public static Boolean ComprobarEntero(String numeroString, out int numero)
        {
            bool esEntero = false;

            esEntero = int.TryParse(numeroString, out numero);
            if (!esEntero) Console.WriteLine("No has introducido entero.");

            return esEntero;
        }

        /// <summary>
        /// Método que inicia el proceso de cambio de color en función de las
        /// reglas del juego y a partir de la posición elegida para realizar
        /// la jugada
        /// </summary>
        /// <param name="tablero">Tablero en su situación actual</param>
        /// <param name="color">Color del jugador</param>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        public static void CambioDeColor(Tablero tablero, Color color, int row,
                int col, out int piezasComidas)
        {
            piezasComidas = 0;
            /* Comprobamos las direcciones que cumplen que la pieza es dis-
            tinta a la que colocamos y no está vacía alrededor de la posición
            que hemos elegido */
            for (int i = row - 1; i < row + 2; i++)
                for (int j = col - 1; j < col + 2; j++)
                {
                    if (ComprobarCasilla(tablero, color, i, j))
                        ComprobarDireccion(tablero, color, i - row, j - col, row,
                                col, out piezasComidas);
                }
        }

        /// <summary>
        /// Comprobamos, en las direcciones en las que es posible el movi-
        /// miento, si podemos comernos piezas del jugador contrario por
        /// haber en dicha dirección alguna pieza nuestra
        /// </summary>
        /// <param name="tablero">Tablero en la situación actual de juego
        /// </param>
        /// <param name="color">Color del jugador</param>
        /// <param name="modH">Modificador de fila, marca la dirección
        /// horizontal en la cual buscar</param>
        /// <param name="modV">Modificar de columna, marca la dirección
        /// vertical en la cual buscar</param>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        public static void ComprobarDireccion(Tablero tablero, Color color,
                int modH, int modV, int row, int col, out int piezasComidas)
        {
            piezasComidas = 0;
            Boolean seguirBuscando = true;
            int posH = row, posV = col;
            do
            {
                posH += modH;
                posV += modV;
                seguirBuscando = ComprobarCasilla(tablero, color, posH, posV);
                piezasComidas++;
            } while (seguirBuscando);
            piezasComidas--;
            /* Si hemos encontrado una pieza de nuestro mismo color en la
            dirección que estamos comprobando, podemos cambiar las piezas
            que haya de distinto color en esa dirección */
            if (tablero.GetBoard()[posH, posV] == (char)color)
                /* Procedemos a deshacer el camino cambiando las piezas hasta
                que lleguemos a la posición de partida */
                while (posH != row || posV != col)
                {
                    posH -= modH;
                    posV -= modV;
                    tablero.SetBoard(posH, posV, color);
                }
        }

        /// <summary>
        /// Comprobamos si una casilla cumple no estar vacía y ser de color
        /// distinto a la del jugador, es decir, si es susceptible de ser 
        /// 'comida' por el jugador
        /// </summary>
        /// <param name="tablero">Tablero en su situación actual</param>
        /// <param name="color">Color del jugador</param>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        /// <returns>True en caso de ser casilla susceptible de ser comida,
        /// false en caso contrario</returns>
        private static Boolean ComprobarCasilla(Tablero tablero, Color color, int row, int col)
        {
            return tablero.GetBoard()[row, col] != (char)color &&
                   tablero.GetBoard()[row, col] != (char)Color.VACIO;

        }
    }
}
