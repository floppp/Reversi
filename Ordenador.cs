using System;
using System.Collections;

namespace Reversi
{
    class Ordenador
    {
        /* Color del jugador */
        protected Color color;
        /* Control para saber si el valor que obtenemos de la posición
        y que hemos de comprobar es fila o columna */
        protected int contadorFilaColumna = 0;
        /* Objeto en el que almacenamos par de valores finales que al-
        macenarán el movimiento finalmente elegido */
        protected Posicion valoresElegidos;
        /* Cola con las posibles posiciones que cumplen todas las reglas
        del juego y de la que extraeremos la posición finalmente elegida */
        protected Queue posicionesValidas;

        /// <summary>
        /// Método para comprobar en qué casillas podemos colocar piezas.
        /// </summary>
        /// <param name="tablero">Tablero en su situación actual</param>
        public void ComprobarValidas(Tablero tablero)
        {
            posicionesValidas = new Queue();

            Posicion posicion;
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    if (Reglas.PosicionLegal(tablero, row, col, color))
                    {
                        posicion = new Posicion();
                        posicion.Row = row;
                        posicion.Col = col;
                        posicionesValidas.Enqueue(posicion);
                    }
        }
    }
}