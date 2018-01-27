using System;

namespace Reversi
{
    /// <summary>
    /// IA que elige la pieza que coloca buscando, de las posiciones
    /// legales, aquella que genere una mayor ganancia
    /// </summary>
    class OrdenadorNivel2 : Ordenador, Participante
    {
        private int[,] fichasComidas;

        private OrdenadorNivel2(int numeroJugador)
        {
            if (numeroJugador == 1) base.color = Color.NEGRO;
            else                    base.color = Color.BLANCO;

            fichasComidas = new int[10, 10];
        }

        public static OrdenadorNivel2 getInstance(int numeroJugador)
        {
            return new OrdenadorNivel2(numeroJugador);
        }

        /// <summary>
        /// Obtenemos el color del jugador
        /// </summary>
        /// <returns>Color</returns>
        public Color GetColor()
        {
            return base.color;
        }

        /// <summary>
        /// Método para pedir posición al jugador
        /// </summary>
        /// <param name="tablero">Tablero en su situación actutal</param>
        public void Movimiento(Tablero tablero)
        {
            int piezasComidas;
            valoresElegidos = new Posicion();
            ComprobarValidas(tablero);
            valoresElegidos = ElegirPosicion(tablero);
            bool pasa = false;

            if (valoresElegidos.Col == 0)
                if (valoresElegidos.Row == 0)
                    pasa = true;

            if (!pasa)
            {
                Reglas.CambioDeColor(tablero, color, valoresElegidos.Row,
                        valoresElegidos.Col, out piezasComidas);
                ActualizarTablero(tablero, valoresElegidos.Row, 
                        valoresElegidos.Col);
            }
            //        valoresElegidos.Col, out piezasComidas);
            //ActualizarTablero(tablero, valoresElegidos.Row, valoresElegidos.Col);
        }

        /// <summary>
        /// Método para actualizar tablero
        /// </summary>
        /// <param name="jugador">Participante que está realizando
        /// movimiento</param>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        public void ActualizarTablero(Tablero tablero, int row, int col)
        {
            tablero.SetBoard(row, col, color);
            tablero.MostrarTablero();
        }

        /// <summary>
        /// Método donde decidimos la forma de actual de la IA.
        /// En este caso, elgimos de forma aleatoria entre las posiciones
        /// válidas
        /// </summary>
        /// <returns>Posicion elegida</returns>
        public Posicion ElegirPosicion(Tablero tablero)
        {
            Posicion valoresElegidos = new Posicion();
            PonerFichasComidasEnBlanco();
            valoresElegidos = ElegirPosicion_bis(tablero);
            return valoresElegidos;
        }

        /// <summary>
        /// Buscamos posición que genere una mayor ganancia de piezas del
        /// rival
        /// </summary>
        /// <param name="tablero">Tablero del que queremos obtener
        /// distintas opciones de juego</param>
        /// <param name="profundidad">rango de movimientos máximos que
        /// podemos simular en cascada</param>
        /// <returns></returns>
        public Posicion ElegirPosicion_bis(Tablero tablero)
        {
            int piezasComidas;
            int row, col;
            Posicion posicionElegida = new Posicion();
            Tablero tableroCopia = new Tablero();
            tableroCopia.Copiar(tablero);

            foreach (Posicion posicion in posicionesValidas)
            {
                Reglas.CambioDeColor(tableroCopia, color, posicion.Row,
                        posicion.Col, out piezasComidas);
                row = posicion.Row;
                col = posicion.Col;
                
                fichasComidas[row, col] += piezasComidas;
            }
            ImprimirMatriz();
            posicionElegida = ComprobarMejorPosicion();

            return posicionElegida;
        }

        /// <summary>
        /// Método para imprimir matriz
        /// </summary>
        private void ImprimirMatriz()
        {
            for (int row = 1; row < 9; row++)
            {
                for (int col = 1; col < 9; col++)
                    Console.Write("{0} ", fichasComidas[row, col]);
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }


        /// <summary>
        /// Comprobamos la matriz de registro de piezas comidas, y devolvemos
        /// la mejor posición para colocar la pieza
        /// </summary>
        /// <returns>Posición óptima</returns>
        private Posicion ComprobarMejorPosicion()
        {
            Posicion posicion = new Posicion();

            int rowMax = 1, colMax = 1;
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    if (fichasComidas[row, col] > fichasComidas[rowMax, colMax])
                    {
                        rowMax = row;
                        colMax = col;
                    }

            posicion.Row = rowMax;
            posicion.Col = colMax;

            return posicion;
        }

        /// <summary>
        /// Ponemos la matriz que nos dice que jugada es más óptima, con
        /// cual ganamos más piezas al contrario, a 0
        /// </summary>
        private void PonerFichasComidasEnBlanco()
        {
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    this.fichasComidas[row, col] = 0;
        }           
    }
}