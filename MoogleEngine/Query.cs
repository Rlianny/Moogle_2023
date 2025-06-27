namespace MoogleEngine;

    public class Query
    {
        #region Área de declaración de propiedades de la clase

        public string InputQuery {get; set;} // string introducido por el usuario
        public Dictionary <string, float> DataQuery; // diccionario que almacena la relevancia de cada palabra de la query
        public string[] QueryWordsArray {get; private set;} // array que las palabras de la query

        #endregion

        #region Área del constructor de la clase

        public Query(string input)
        {
            InputQuery = input;

            QueryWordsArray = Tools.TxtProcesser(InputQuery); // procesamos la query

            DataQuery = GetQueryRelevance(QueryWordsArray); // obtenemos el diccionario con la relevancia
        
            System.Console.WriteLine($"La Query {input} ha sido cargada");
        }
        #endregion

        #region Métodos de clase
        static private Dictionary <string, float> GetQueryRelevance(string[] QueryWordsArray) // array que devuelve un diccionario que tiene como llave cada palabra de la query, y comon value su relevancia
        {   
            Dictionary <string, float> DataQuery = new Dictionary <string, float>();

            foreach(string word in QueryWordsArray) // por cada palabra en la query
            {
                if(!DataQuery.Keys.Contains(word)) // si el diccionario no contenía la palabra
                DataQuery.Add(word, 1); // la anadimos y le asignamos una frecuencia de 1
 
                else // si la palabra ya se encontraba en el diccionario
                DataQuery[word]++; //aumentamos su frecuencia en 1
            }

            foreach(string key in DataQuery.Keys)
            {
                DataQuery[key] = DataQuery[key]/QueryWordsArray.Length;; // calculamos el TF de la query
                if(DataFolder.IDF.ContainsKey(key))
                {
                    DataQuery[key] = DataQuery[key] * DataFolder.IDF[key];
                } // multiplicamos el TF*IDF para obtener la relevancia de cada palabra de la query
            }

            return DataQuery;
        }
        #endregion
    }

    