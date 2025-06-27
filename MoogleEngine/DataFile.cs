namespace MoogleEngine;

public class DataFile
    {   
        #region Área de declaración de propiedades de clase

        public string FileRoot {get; private set;} // ruta del archivo
        public string FileName {get; private set;} // nombre del archivo
        public string FileContent {get; private set;} // contenido del archivo sin procesar
        public int FileWords {get; private set;} // cantidad de palabras por archivo
        public string[] AllWordsOnFile {get; set;} // array que contiene todas las palabras del archivo
        public Dictionary <string, float> WordFreq {get; set;} // diccionario que contiene el conjunto de palabras de un documento y su TF

        #endregion

        #region Área del constructor de la clase

        public DataFile(string root)
        {   
            FileRoot = root;

            FileName = GetFileName(root); 

            FileContent = GetFileContent(root); 
           
            AllWordsOnFile = Tools.TxtProcesser(FileContent);

            FileWords = AllWordsOnFile.Length; 

            WordFreq = TFCalculator(AllWordsOnFile);
        }
        #endregion

        #region Métodos de clase

        private static string GetFileName(string root) // método que devuelve el nombre del archivo
        {
            string FileName = Path.GetFileName(root); // obtenemos el nombre del archivo con su extension
            FileName = Path.ChangeExtension(FileName, null); // anulamos la extension
            
            return FileName;
        }

        private static string GetFileContent(string root) // método que devuleve el contenido del archivo
        {
            StreamReader reader = new StreamReader(root); // leemos el contenido del archivo
            string FileContent = reader.ReadToEnd();
            reader.Close(); 

            return FileContent;
        }

        private static Dictionary<string, float> TFCalculator(string[] AllWordsOnFile) // método que devuelve un diccionario que tiene como llave cada palabra del documento y value su tf
        {   
            Dictionary<string, float> WordFreq = new Dictionary<string, float>();
            float maxFreq = 0;
            foreach(string word in AllWordsOnFile)
                {  
                    if(!WordFreq.Keys.Contains(word)) // si la palabra no estaba en el documento la cargamos y aparece 1 vez
                    {
                        WordFreq.Add(word, 1);
                    }
                    else // si la palabra ya estaba en el documento aumentamos su valor en 1
                    {
                        WordFreq[word]++;
                        maxFreq = Math.Max(maxFreq , WordFreq[word]);
                    }
                }

                foreach(string key in WordFreq.Keys)
                {
                    WordFreq[key] = WordFreq[key]/maxFreq; // tomamos el TF de un termino en un documento como el numero de apariciones del termino en un documento / numero total de palabras del documento
                }

            return WordFreq;
        }

        public string FragmentWithWords(string[] query, string root, Dictionary <string, float> IDF) // método que devuelve el snippet
        {
            StreamReader reader = new StreamReader(root);
            string content = reader.ReadToEnd().ToLower();
            reader.Close();
            content = Tools.Transform(content);

            string[] words = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int part1 = 0; // contadores para comparar
            int part2 = 0;

            string[] Query = WordsImportant(query, IDF); // extraemos las palabras que tienen relevancia en la query

            while (words.Length > 80) // solo devolveremos un maximo de 80 palabras
            {
                // partimos a la mitad el array
                string[] Part1 = Partition(words, 0, words.Length / 2);
                string[] Part2 = Partition(words, words.Length / 2, words.Length);

                int count = 0;

                for (int i = 0; i < Math.Min(Part1.Length, Part2.Length); i++)
                {
                    count++;
                    if (Query.Contains(Part1[i])) // si la query está contenida en el en la primera mitad
                    { 
                        part1++; // aumentamos en una unidad el valor del comparador
                    }
                    if (Query.Contains(Part2[i])) // si la query está contenida en la segunda mitad
                    {
                        part2++; // aumnetamos en una unidad el valor del comparador
                    }
                }
                if (Part1.Length > Part2.Length)
                {
                    for (int i = count; i < Part1.Length; i++)
                    {
                        if (Query.Contains(Part1[i]))
                        {
                            part1++;
                        }
                    }
                }
                if (Part2.Length > Part1.Length)
                {
                    for (int i = count; i < Part2.Length; i++)
                    {
                        if (Query.Contains(Part2[i]))
                        {
                            part2++;
                        }
                    }
                }
                if (part1 >= part2) // nos quedamos con la que mas resultados tuvo
                    words = Part1;
                else
                    words = Part2;
                part1 = 0;
                part2 = 0;
            }

            string result = "";

            foreach (string a in words) // generamos el texto a devolver
            {
                if (Query.Contains(a))
                    result += "**" + a + "** ";
                else
                    result += a + " ";
            }

            return "........" + result + "........";
        } 

        string[] Partition(string[]words,int stratindex,int endindex)// método que devuelve una partición de un array de palabras
        {
            string[] result = new string[endindex - stratindex];
            int position = 0;
            for(int i = stratindex; i < endindex; i++)
            {
                result[position] = words[i];
                position++;
            }
            return result;
        }
        string[] WordsImportant(string[] AllWordsOnFile, Dictionary <string, float> IDF) // metodo que nos devuelve un array con las palabras que tengan relevancia
        {
            List<string> result = new List<string>();

            foreach(string word in AllWordsOnFile)
            {   
                if(IDF.ContainsKey(word) && IDF[word] != 0 && word.Length > 3) //si la palabra existe en la carpeta de archivos, su relevancia no es cero y es mayor a tres carácteres(nos deshacemos de los artículos)
                result.Add(word);
            }

            return result.ToArray();
        }

        #endregion
    }