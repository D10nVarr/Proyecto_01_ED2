using System;

namespace SistemaBiblioteca
{
    public class Libro
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CopiasDisponibles { get; set; }
        public int VecesPrestado { get; set; }

        public override string ToString()
        {
            return $"[{Codigo}] {Titulo} | {Autor} | Existencias: {CopiasDisponibles} | Préstamos: {VecesPrestado}";
        }
    }

    public class NodoHeap
    {
        public int Valor { get; set; } 
        public int CodigoLibro { get; set; }

        public NodoHeap(int valor, int codigoLibro)
        {
            Valor = valor;
            CodigoLibro = codigoLibro;
        }
    }
    public class MiLista<T>
    {
        private T[] items;
        public int Cantidad { get; private set; }

        public MiLista(int capacidadInicial = 4)
        {
            items = new T[capacidadInicial];
            Cantidad = 0;
        }

        public T this[int indice]
        {
            get { return items[indice]; }
            set { items[indice] = value; }
        }

        public void Agregar(T item)
        {
            if (Cantidad == items.Length) Redimensionar();
            items[Cantidad] = item;
            Cantidad++;
        }

        public void Insertar(int indice, T item)
        {
            if (Cantidad == items.Length) Redimensionar();
            for (int i = Cantidad; i > indice; i--) items[i] = items[i - 1];
            items[indice] = item;
            Cantidad++;
        }

        public void EliminarEn(int indice)
        {
            for (int i = indice; i < Cantidad - 1; i++) items[i] = items[i + 1];
            Cantidad--;
            items[Cantidad] = default!; 
        }

        public void AgregarRango(MiLista<T> otro)
        {
            for (int i = 0; i < otro.Cantidad; i++) Agregar(otro[i]);
        }

        public MiLista<T> ObtenerRango(int indice, int cantidad)
        {
            MiLista<T> resultado = new MiLista<T>(cantidad > 4 ? cantidad : 4);
            for (int i = 0; i < cantidad; i++) resultado.Agregar(items[indice + i]);
            return resultado;
        }

        public int IndiceDe(T item)
        {
            for (int i = 0; i < Cantidad; i++)
            {
                if (object.Equals(items[i], item)) return i;
            }
            return -1;
        }

        public void Limpiar()
        {
            Cantidad = 0;
            items = new T[4];
        }

        private void Redimensionar()
        {
            T[] nuevoArreglo = new T[items.Length * 2];
            for (int i = 0; i < Cantidad; i++) nuevoArreglo[i] = items[i];
            items = nuevoArreglo;
        }

        public override string ToString()
        {
            string resultado = "";
            for (int i = 0; i < Cantidad; i++)
            {
                resultado += items[i]!.ToString();
                if (i < Cantidad - 1) resultado += ", ";
            }
            return resultado;
        }
    }

    public class MinHeap
    {
        private MiLista<NodoHeap> heap = new MiLista<NodoHeap>();

        public void Insertar(int valor, int codigo)
        {
            heap.Agregar(new NodoHeap(valor, codigo));
            HeapifyUp(heap.Cantidad - 1);
        }

        public void Actualizar(int codigo, int nuevoValor)
        {
            int index = -1;
            for (int i = 0; i < heap.Cantidad; i++)
            {
                if (heap[i].CodigoLibro == codigo) { index = i; break; }
            }
            if (index == -1) return;

            int valorViejo = heap[index].Valor;
            heap[index].Valor = nuevoValor;

            if (nuevoValor < valorViejo) HeapifyUp(index);
            else HeapifyDown(index);
        }

        public void EliminarPorCodigo(int codigo)
        {
            int index = -1;
            for (int i = 0; i < heap.Cantidad; i++)
            {
                if (heap[i].CodigoLibro == codigo) { index = i; break; }
            }
            if (index == -1) return;

            if (index == heap.Cantidad - 1)
            {
                heap.EliminarEn(index);
                return;
            }

            NodoHeap ultimo = heap[heap.Cantidad - 1];
            heap.EliminarEn(heap.Cantidad - 1);
            
            int valorViejo = heap[index].Valor;
            heap[index] = ultimo;

            if (heap[index].Valor < valorViejo) HeapifyUp(index);
            else HeapifyDown(index);
        }

        private void HeapifyUp(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;
                if (heap[indice].Valor < heap[padre].Valor)
                {
                    NodoHeap temp = heap[indice];
                    heap[indice] = heap[padre];
                    heap[padre] = temp;
                    indice = padre;
                }
                else break;
            }
        }

        private void HeapifyDown(int indice)
        {
            int cantidad = heap.Cantidad;
            while (true)
            {
                int menor = indice;
                int hijoIz = 2 * indice + 1;
                int hijoDer = 2 * indice + 2;

                if (hijoIz < cantidad && heap[hijoIz].Valor < heap[menor].Valor) menor = hijoIz;
                if (hijoDer < cantidad && heap[hijoDer].Valor < heap[menor].Valor) menor = hijoDer;

                if (menor == indice) break;

                NodoHeap temp = heap[indice];
                heap[indice] = heap[menor];
                heap[menor] = temp;
                indice = menor;
            }
        }

        public NodoHeap? ObtenerMinimo()
        {
            if (heap.Cantidad == 0) return null;
            return heap[0];
        }
    }

    public class MaxHeap
    {
        private MiLista<NodoHeap> heap = new MiLista<NodoHeap>();

        public void Insertar(int valor, int codigo)
        {
            heap.Agregar(new NodoHeap(valor, codigo));
            HeapifyUp(heap.Cantidad - 1);
        }

        public void Actualizar(int codigo, int nuevoValor)
        {
            int index = -1;
            for (int i = 0; i < heap.Cantidad; i++)
            {
                if (heap[i].CodigoLibro == codigo) { index = i; break; }
            }
            if (index == -1) return;

            int valorViejo = heap[index].Valor;
            heap[index].Valor = nuevoValor;

            if (nuevoValor > valorViejo) HeapifyUp(index);
            else HeapifyDown(index);
        }

        public void EliminarPorCodigo(int codigo)
        {
            int index = -1;
            for (int i = 0; i < heap.Cantidad; i++)
            {
                if (heap[i].CodigoLibro == codigo) { index = i; break; }
            }
            if (index == -1) return;

            if (index == heap.Cantidad - 1)
            {
                heap.EliminarEn(index);
                return;
            }

            NodoHeap ultimo = heap[heap.Cantidad - 1];
            heap.EliminarEn(heap.Cantidad - 1);
            
            int valorViejo = heap[index].Valor;
            heap[index] = ultimo;

            if (heap[index].Valor > valorViejo) HeapifyUp(index);
            else HeapifyDown(index);
        }

        private void HeapifyUp(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;
                if (heap[indice].Valor > heap[padre].Valor)
                {
                    NodoHeap temp = heap[indice];
                    heap[indice] = heap[padre];
                    heap[padre] = temp;
                    indice = padre;
                }
                else break;
            }
        }

        private void HeapifyDown(int indice)
        {
            int cantidad = heap.Cantidad;
            while (true)
            {
                int mayor = indice;
                int hijoIz = 2 * indice + 1;
                int hijoDer = 2 * indice + 2;

                if (hijoIz < cantidad && heap[hijoIz].Valor > heap[mayor].Valor) mayor = hijoIz;
                if (hijoDer < cantidad && heap[hijoDer].Valor > heap[mayor].Valor) mayor = hijoDer;

                if (mayor == indice) break;

                NodoHeap temp = heap[indice];
                heap[indice] = heap[mayor];
                heap[mayor] = temp;
                indice = mayor;
            }
        }

        public NodoHeap? ObtenerMaximo()
        {
            if (heap.Cantidad == 0) return null;
            return heap[0];
        }

        public NodoHeap? ExtraerMaximo()
        {
            if (heap.Cantidad == 0) return null;
            
            NodoHeap maximo = heap[0];
            
            if (heap.Cantidad == 1)
            {
                heap.EliminarEn(0);
                return maximo;
            }

            NodoHeap ultimo = heap[heap.Cantidad - 1];
            heap.EliminarEn(heap.Cantidad - 1);
            
            heap[0] = ultimo;
            HeapifyDown(0);
            
            return maximo;
        }
    }

    public class NodoBPlus
    {
        public bool Hoja { get; set; }
        public MiLista<int> Claves { get; set; }
        public MiLista<Libro> Valores { get; set; } 
        public MiLista<NodoBPlus> Hijos { get; set; }
        public NodoBPlus? Siguiente { get; set; }
        public NodoBPlus? Padre { get; set; }

        public NodoBPlus(bool hoja = true)
        {
            Hoja = hoja;
            Claves = new MiLista<int>();
            Valores = new MiLista<Libro>();
            Hijos = new MiLista<NodoBPlus>();
            Siguiente = null;
            Padre = null;
        }
    }

    public class ArbolBPlus
    {
        public int Orden { get; private set; }
        public int MaxClaves { get; private set; }
        public int MinClavesHoja { get; private set; }
        public int MinHijosInterno { get; private set; }
        public NodoBPlus Raiz { get; private set; }

        public ArbolBPlus(int orden = 4)
        {
            Orden = orden;
            MaxClaves = orden - 1;
            MinClavesHoja = (int)Math.Ceiling((orden - 1) / 2.0);
            MinHijosInterno = (int)Math.Ceiling(orden / 2.0);
            Raiz = new NodoBPlus(hoja: true);
        }

        private int BisectLeft(MiLista<int> lista, int valor)
        {
            int lo = 0, hi = lista.Cantidad;
            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                if (lista[mid] < valor) lo = mid + 1;
                else hi = mid;
            }
            return lo;
        }

        private int BisectRight(MiLista<int> lista, int valor)
        {
            int lo = 0, hi = lista.Cantidad;
            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                if (valor < lista[mid]) hi = mid;
                else lo = mid + 1;
            }
            return lo;
        }

        private NodoBPlus BuscarHoja(int clave)
        {
            NodoBPlus nodo = Raiz;
            while (!nodo.Hoja)
            {
                int pos = BisectRight(nodo.Claves, clave);
                nodo = nodo.Hijos[pos];
            }
            return nodo;
        }

        public Libro? Buscar(int clave)
        {
            NodoBPlus hoja = BuscarHoja(clave);
            int pos = BisectLeft(hoja.Claves, clave);
            if (pos < hoja.Claves.Cantidad && hoja.Claves[pos] == clave)
            {
                return hoja.Valores[pos];
            }
            return null;
        }

        public void Insertar(Libro libro)
        {
            if (Buscar(libro.Codigo) != null) return; 

            NodoBPlus hoja = BuscarHoja(libro.Codigo);
            int pos = BisectLeft(hoja.Claves, libro.Codigo);
            
            hoja.Claves.Insertar(pos, libro.Codigo);
            hoja.Valores.Insertar(pos, libro); 

            if (hoja.Claves.Cantidad > MaxClaves) DividirHoja(hoja);
            RecalcularGuias(Raiz);
        }

        private void DividirHoja(NodoBPlus hoja)
        {
            int punto = (hoja.Claves.Cantidad + 1) / 2;
            NodoBPlus nuevaHoja = new NodoBPlus(hoja: true);
            nuevaHoja.Padre = hoja.Padre;

            nuevaHoja.Claves = hoja.Claves.ObtenerRango(punto, hoja.Claves.Cantidad - punto);
            nuevaHoja.Valores = hoja.Valores.ObtenerRango(punto, hoja.Valores.Cantidad - punto);
            
            hoja.Claves = hoja.Claves.ObtenerRango(0, punto);
            hoja.Valores = hoja.Valores.ObtenerRango(0, punto);

            nuevaHoja.Siguiente = hoja.Siguiente;
            hoja.Siguiente = nuevaHoja;

            InsertarEnPadre(hoja, nuevaHoja.Claves[0], nuevaHoja);
        }

        private void InsertarEnPadre(NodoBPlus izq, int claveGuia, NodoBPlus der)
        {
            if (izq == Raiz)
            {
                NodoBPlus nuevaRaiz = new NodoBPlus(hoja: false);
                nuevaRaiz.Claves.Agregar(claveGuia);
                nuevaRaiz.Hijos.Agregar(izq);
                nuevaRaiz.Hijos.Agregar(der);
                izq.Padre = nuevaRaiz;
                der.Padre = nuevaRaiz;
                Raiz = nuevaRaiz;
                return;
            }

            NodoBPlus? padre = izq.Padre;
            if (padre == null) return;

            int pos = padre.Hijos.IndiceDe(izq);
            padre.Claves.Insertar(pos, claveGuia);
            padre.Hijos.Insertar(pos + 1, der);
            der.Padre = padre;

            if (padre.Claves.Cantidad > MaxClaves) DividirInterno(padre);
        }

        private void DividirInterno(NodoBPlus nodo)
        {
            int centro = nodo.Claves.Cantidad / 2;
            int claveSube = nodo.Claves[centro];

            NodoBPlus nuevoInt = new NodoBPlus(hoja: false);
            nuevoInt.Padre = nodo.Padre;

            nuevoInt.Claves = nodo.Claves.ObtenerRango(centro + 1, nodo.Claves.Cantidad - (centro + 1));
            nuevoInt.Hijos = nodo.Hijos.ObtenerRango(centro + 1, nodo.Hijos.Cantidad - (centro + 1));

            for (int i = 0; i < nuevoInt.Hijos.Cantidad; i++) nuevoInt.Hijos[i].Padre = nuevoInt;

            nodo.Claves = nodo.Claves.ObtenerRango(0, centro);
            nodo.Hijos = nodo.Hijos.ObtenerRango(0, centro + 1);

            InsertarEnPadre(nodo, claveSube, nuevoInt);
        }

        public bool Eliminar(int clave)
        {
            NodoBPlus hoja = BuscarHoja(clave);
            int posicion = BisectLeft(hoja.Claves, clave);

            if (posicion >= hoja.Claves.Cantidad || hoja.Claves[posicion] != clave) return false;

            hoja.Claves.EliminarEn(posicion);
            hoja.Valores.EliminarEn(posicion); 

            if (hoja == Raiz) return true;

            if (hoja.Claves.Cantidad < MinClavesHoja) RepararHoja(hoja);

            RecalcularGuias(Raiz);
            return true;
        }

        private void RepararHoja(NodoBPlus hoja)
        {
            NodoBPlus? padre = hoja.Padre;
            if (padre == null) return;
            int posicion = padre.Hijos.IndiceDe(hoja);

            NodoBPlus? hermanoIzq = posicion > 0 ? padre.Hijos[posicion - 1] : null;
            NodoBPlus? hermanoDer = posicion + 1 < padre.Hijos.Cantidad ? padre.Hijos[posicion + 1] : null;

            if (hermanoIzq != null && hermanoIzq.Claves.Cantidad > MinClavesHoja)
            {
                int clavePrestada = hermanoIzq.Claves[hermanoIzq.Claves.Cantidad - 1];
                Libro valorPrestado = hermanoIzq.Valores[hermanoIzq.Valores.Cantidad - 1];
                
                hermanoIzq.Claves.EliminarEn(hermanoIzq.Claves.Cantidad - 1);
                hermanoIzq.Valores.EliminarEn(hermanoIzq.Valores.Cantidad - 1);
                
                hoja.Claves.Insertar(0, clavePrestada);
                hoja.Valores.Insertar(0, valorPrestado);
                return;
            }

            if (hermanoDer != null && hermanoDer.Claves.Cantidad > MinClavesHoja)
            {
                int clavePrestada = hermanoDer.Claves[0];
                Libro valorPrestado = hermanoDer.Valores[0];
                
                hermanoDer.Claves.EliminarEn(0);
                hermanoDer.Valores.EliminarEn(0);
                
                hoja.Claves.Agregar(clavePrestada);
                hoja.Valores.Agregar(valorPrestado);
                return;
            }

            if (hermanoIzq != null)
            {
                hermanoIzq.Claves.AgregarRango(hoja.Claves);
                hermanoIzq.Valores.AgregarRango(hoja.Valores); 
                hermanoIzq.Siguiente = hoja.Siguiente;
                padre.Hijos.EliminarEn(posicion);
                padre.Claves.EliminarEn(posicion - 1);
                RepararInterno(padre);
            }
            else if (hermanoDer != null)
            {
                hoja.Claves.AgregarRango(hermanoDer.Claves);
                hoja.Valores.AgregarRango(hermanoDer.Valores);
                hoja.Siguiente = hermanoDer.Siguiente;
                padre.Hijos.EliminarEn(posicion + 1);
                padre.Claves.EliminarEn(posicion);
                RepararInterno(padre);
            }
        }

        private void RepararInterno(NodoBPlus nodo)
        {
            if (nodo == Raiz)
            {
                if (nodo.Claves.Cantidad == 0)
                {
                    Raiz = nodo.Hijos[0];
                    Raiz.Padre = null;
                }
                return;
            }

            if (nodo.Hijos.Cantidad >= MinHijosInterno) return;

            NodoBPlus? padre = nodo.Padre;
            if (padre == null) return;
            
            int posicion = padre.Hijos.IndiceDe(nodo);

            NodoBPlus? izq = posicion > 0 ? padre.Hijos[posicion - 1] : null;
            NodoBPlus? der = posicion + 1 < padre.Hijos.Cantidad ? padre.Hijos[posicion + 1] : null;

            if (izq != null && izq.Hijos.Cantidad > MinHijosInterno)
            {
                NodoBPlus hijoMovido = izq.Hijos[izq.Hijos.Cantidad - 1];
                izq.Hijos.EliminarEn(izq.Hijos.Cantidad - 1);
                hijoMovido.Padre = nodo;

                int nuevaGuia = izq.Claves[izq.Claves.Cantidad - 1];
                izq.Claves.EliminarEn(izq.Claves.Cantidad - 1);

                nodo.Hijos.Insertar(0, hijoMovido);
                nodo.Claves.Insertar(0, padre.Claves[posicion - 1]);
                padre.Claves[posicion - 1] = nuevaGuia;
                return;
            }

            if (der != null && der.Hijos.Cantidad > MinHijosInterno)
            {
                NodoBPlus hijoMovido = der.Hijos[0];
                der.Hijos.EliminarEn(0);
                hijoMovido.Padre = nodo;

                nodo.Hijos.Agregar(hijoMovido);
                nodo.Claves.Agregar(padre.Claves[posicion]);

                int nuevaGuia = der.Claves[0];
                der.Claves.EliminarEn(0);
                padre.Claves[posicion] = nuevaGuia;
                return;
            }

            if (izq != null)
            {
                izq.Claves.Agregar(padre.Claves[posicion - 1]);
                padre.Claves.EliminarEn(posicion - 1);
                izq.Claves.AgregarRango(nodo.Claves);

                for (int i = 0; i < nodo.Hijos.Cantidad; i++) nodo.Hijos[i].Padre = izq;

                izq.Hijos.AgregarRango(nodo.Hijos);
                padre.Hijos.EliminarEn(posicion);
                RepararInterno(padre);
            }
            else if (der != null)
            {
                nodo.Claves.Agregar(padre.Claves[posicion]);
                padre.Claves.EliminarEn(posicion);
                nodo.Claves.AgregarRango(der.Claves);

                for (int i = 0; i < der.Hijos.Cantidad; i++) der.Hijos[i].Padre = nodo;

                nodo.Hijos.AgregarRango(der.Hijos);
                padre.Hijos.EliminarEn(posicion + 1);
                RepararInterno(padre);
            }
        }

        private void RecalcularGuias(NodoBPlus nodo)
        {
            if (nodo.Hoja) return;
            for (int i = 0; i < nodo.Hijos.Cantidad; i++) RecalcularGuias(nodo.Hijos[i]);

            nodo.Claves.Limpiar();
            for (int i = 1; i < nodo.Hijos.Cantidad; i++)
            {
                NodoBPlus actual = nodo.Hijos[i];
                while (!actual.Hoja) actual = actual.Hijos[0];
                nodo.Claves.Agregar(actual.Claves[0]);
            }
        }

        public MiLista<Libro> ObtenerTodosLosLibros()
        {
            NodoBPlus? nodo = Raiz;
            while (!nodo.Hoja) nodo = nodo.Hijos[0];

            MiLista<Libro> catalogo = new MiLista<Libro>();
            while (nodo != null)
            {
                catalogo.AgregarRango(nodo.Valores);
                nodo = nodo.Siguiente;
            }
            return catalogo;
        }
        
        public void MostrarEstructura()
        {
            Console.WriteLine("\n[Árbol B+]");
            MostrarInterno(Raiz, 0);
        }

        private void MostrarInterno(NodoBPlus nodo, int nivel)
        {
            string sangria = new string(' ', nivel * 4);
            string tipo = nodo.Hoja ? "Hoja" : "Interno";
            Console.WriteLine($"{sangria}{tipo}: [{nodo.Claves.ToString()}]");

            if (!nodo.Hoja)
            {
                for (int i = 0; i < nodo.Hijos.Cantidad; i++)
                {
                    MostrarInterno(nodo.Hijos[i], nivel + 1);
                }
            }
        }
    }

    public class GestorBiblioteca
    {
        private ArbolBPlus catalogo = new ArbolBPlus();
        private MaxHeap heapPrestamos = new MaxHeap();
        private MinHeap heapExistencias = new MinHeap();

        public bool ExisteLibro(int codigo)
        {
            return catalogo.Buscar(codigo) != null;
        }

        public void RegistrarLibro(int codigo, string titulo, string autor, string categoria, int copias)
        {
            Libro? libroExistente = catalogo.Buscar(codigo);

            if (libroExistente != null)
            {
                libroExistente.CopiasDisponibles += copias;
                heapExistencias.Actualizar(codigo, libroExistente.CopiasDisponibles);
                Console.WriteLine($"\nEl libro '{libroExistente.Titulo}' ya estaba registrado. Se sumaron {copias} copias (Total actual: {libroExistente.CopiasDisponibles}).");
            }
            else
            {
                Libro nuevoLibro = new Libro
                {
                    Codigo = codigo,
                    Titulo = titulo,
                    Autor = autor,
                    Categoria = categoria,
                    CopiasDisponibles = copias,
                    VecesPrestado = 0
                };

                catalogo.Insertar(nuevoLibro);
                heapExistencias.Insertar(copias, codigo);
                heapPrestamos.Insertar(0, codigo);
                
                Console.WriteLine($"\nLibro: '{titulo}' registrado correctamente");
            }
        }

        public void BuscarLibro(int codigo)
        {
            Libro? libro = catalogo.Buscar(codigo);
            if (libro != null) Console.WriteLine($"\nEncontrado: {libro}");
            else Console.WriteLine($"\nLibro con código {codigo} no encontrado.");
        }

        public void EliminarLibro(int codigo)
        {
            Libro? libro = catalogo.Buscar(codigo);
            if (libro == null)
            {
                Console.WriteLine($"\nEl libro con código {codigo} no existe.");
                return;
            }

            string tituloTemp = libro.Titulo;
            
            catalogo.Eliminar(codigo);
            heapExistencias.EliminarPorCodigo(codigo);
            heapPrestamos.EliminarPorCodigo(codigo);

            Console.WriteLine($"\nEl libro '{tituloTemp}' ha sido eliminado de la biblioteca.");
        }

        public void MostrarCatalogoOrdenado()
        {
            MiLista<Libro> lista = catalogo.ObtenerTodosLosLibros();
            
            if (lista.Cantidad == 0)
            {
                Console.WriteLine("\nEl catálogo está vacío.");
                return;
            }

            Console.WriteLine("\nCATÁLOGO DE LA BIBLIOTECA");
            for (int i = 0; i < lista.Cantidad; i++) 
            {
                Console.WriteLine(lista[i]);
            }
            Console.WriteLine("----------------------------------------------------");
        }

        public void MostrarArbolBPlus()
        {
            catalogo.MostrarEstructura();
        }

        public void PrestarLibro(int codigo)
        {
            Libro? libro = catalogo.Buscar(codigo);
            if (libro != null)
            {
                if (libro.CopiasDisponibles > 0)
                {
                    libro.CopiasDisponibles--;
                    libro.VecesPrestado++;
                    
                    heapExistencias.Actualizar(codigo, libro.CopiasDisponibles);
                    heapPrestamos.Actualizar(codigo, libro.VecesPrestado);
                    
                    Console.WriteLine($"\nSe prestó '{libro.Titulo}', quedan {libro.CopiasDisponibles} copias.");
                }
                else Console.WriteLine($"\nNo hay copias disponibles de '{libro.Titulo}'.");
            }
            else Console.WriteLine($"\nLibro con código {codigo} no encontrado.");
        }

        public void DevolverLibro(int codigo)
        {
            Libro? libro = catalogo.Buscar(codigo);
            if (libro != null)
            {
                libro.CopiasDisponibles++;
                heapExistencias.Actualizar(codigo, libro.CopiasDisponibles);
                Console.WriteLine($"\nSe devolvió '{libro.Titulo}'. Nuevas copias disponibles {libro.CopiasDisponibles} copias.");
            }
            else Console.WriteLine($"\nLibro con código {codigo} no encontrado.");
        }

        public void MostrarMasPrestado()
        {
            Console.WriteLine("\nTOP 3 LIBROS MÁS POPULARES: ");
            
            MiLista<NodoHeap> extraidos = new MiLista<NodoHeap>();
            int contador = 0;

            for (int i = 0; i < 3; i++)
            {
                NodoHeap? nodo = heapPrestamos.ExtraerMaximo();
                
                if (nodo != null && nodo.Valor > 0)
                {
                    Libro? libro = catalogo.Buscar(nodo.CodigoLibro);
                    if (libro != null)
                    {
                        contador++;
                        Console.WriteLine($"  {contador}. {libro.Titulo} (Prestado {libro.VecesPrestado} veces)");
                    }
                    extraidos.Agregar(nodo); 
                }
                else
                {
                    if (nodo != null) extraidos.Agregar(nodo);
                    break;
                }
            }

            if (contador == 0)
            {
                Console.WriteLine("Aún no hay préstamos registrados en el sistema.");
            }

            for (int i = 0; i < extraidos.Cantidad; i++)
            {
                heapPrestamos.Insertar(extraidos[i].Valor, extraidos[i].CodigoLibro);
            }
        }

        public void MostrarMenosExistencias()
        {
            NodoHeap? nodo = heapExistencias.ObtenerMinimo();
            if (nodo != null)
            {
                Libro? libro = catalogo.Buscar(nodo.CodigoLibro);
                if (libro != null) Console.WriteLine($"\nEl libro con menos stocks es: {libro.Titulo} (Quedan solo {libro.CopiasDisponibles} copias)");
            }
            else Console.WriteLine("\nNo hay libros en el sistema.");
        }
    }

    class Program
    {
        static void Main()
        {
            GestorBiblioteca biblioteca = new GestorBiblioteca();
            bool ejecutando = true;

            while (ejecutando)
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine("          BIBLIOTECA AURA         ");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("1. Registrar o agregar existencias");
                Console.WriteLine("2. Buscar libro");
                Console.WriteLine("3. Eliminar libro de la librería");
                Console.WriteLine("4. Mostrar biblioteca completa");
                Console.WriteLine("5. Mostrar estructura del árbol");
                Console.WriteLine("6. Registrar préstamo");
                Console.WriteLine("7. Registrar devolución");
                Console.WriteLine("8. Ver libros más populares");
                Console.WriteLine("9. Ver libro con menor disponibilidad");
                Console.WriteLine("10. Salir");
                Console.WriteLine("----------------------------------");
                Console.Write("\nSeleccione una opción: ");
                
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int opcion))
                {
                    Console.WriteLine("\nPor favor ingrese un número válido.");
                    continue;
                }

                int codigo;
                switch (opcion)
                {
                    case 1:
                        codigo = LeerEntero("\nIngrese código numérico del libro: ");
                        
                        if (biblioteca.ExisteLibro(codigo))
                        {
                            Console.WriteLine("Este libro ya existe dentro de la libreria.");
                            int copias = LeerEntero("Cantidad de copias a sumar: ");
                            biblioteca.RegistrarLibro(codigo, "", "", "", copias);
                        }
                        else
                        {
                            Console.Write("Título: "); string titulo = Console.ReadLine() ?? "";
                            Console.Write("Autor: "); string autor = Console.ReadLine() ?? "";
                            Console.Write("Categoría: "); string categoria = Console.ReadLine() ?? "";
                            int copias = LeerEntero("Cantidad de copias a ingresar: ");
                            biblioteca.RegistrarLibro(codigo, titulo, autor, categoria, copias);
                        }
                        break;
                    case 2:
                        codigo = LeerEntero("Ingrese código del libro a buscar: ");
                        biblioteca.BuscarLibro(codigo);
                        break;
                    case 3:
                        codigo = LeerEntero("Ingrese código del libro a eliminar: ");
                        biblioteca.EliminarLibro(codigo);
                        break;
                    case 4:
                        biblioteca.MostrarCatalogoOrdenado();
                        break;
                    case 5:
                        biblioteca.MostrarArbolBPlus();
                        break;
                    case 6:
                        codigo = LeerEntero("Ingrese código del libro a prestar: ");
                        biblioteca.PrestarLibro(codigo);
                        break;
                    case 7:
                        codigo = LeerEntero("Ingrese código del libro a devolver: ");
                        biblioteca.DevolverLibro(codigo);
                        break;
                    case 8:
                        biblioteca.MostrarMasPrestado();
                        break;
                    case 9:
                        biblioteca.MostrarMenosExistencias();
                        break;
                    case 10:
                        ejecutando = false;
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static int LeerEntero(string mensaje)
        {
            int numero;
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out numero))
                {
                    return numero;
                }
                Console.WriteLine("Debe ingresar un valor numérico entero.");
            }
        }
    }
}