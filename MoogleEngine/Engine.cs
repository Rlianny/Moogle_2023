using System;
using System.IO; // EnumerateFiles, StreamReader
using System.Collections.Generic; // Dictionary

namespace MoogleEngine;

public static class Engine
{   
    #region Area de metodos de la clase

        public static SearchResult Query(string query, DataFolder Content) // metodo que recibe una query y un contenedor de archivos y devuelve el resultado de la busqueda
        {   
            string suggestion = query; // asiganamos a la sugerencia el mismo string que la query 
            Query ToSearch = new Query(query); // creamos con la query un objeto de tipo Query

            Dictionary < string, Dictionary<string, float>> Docs = Content.Relevance;

            List<SearchItem> docs_Scores = new List<SearchItem>(); // creamos una lista para guardar los objetos de tipo SearchItem
            
            foreach(DataFile file in Content.Files) // por cada archivo en el contenedor
            {
                string tittle = file.FileName; // obtenemos su título
                string snippet = file.FragmentWithWords(ToSearch.QueryWordsArray, file.FileRoot, DataFolder.IDF); // obtenemos su snippet
                float score = ScoreCalculator(ToSearch, Docs, file.FileName); // obtenemos su score

                SearchItem item = new SearchItem(tittle, snippet, score); // creamos un objeto de tipo SearchItem

                if(item.Score != null && item.Snippet != null) // si el score o el snippet son distintos de null
                {
                    docs_Scores.Add(item); // anadimos el objeto a la lista 
                }
            }
    	    
            SearchItem[] Items = new SearchItem[20]; // creamos el array para devolver con 20 resultados

            SearchItem[] sortedScores = Sort(docs_Scores.ToArray()); // ordenamos la lista de SearchItem llevado a array 

            for(int i = 0; i < Items.Length; i++)
            {
            Items[i] = sortedScores[i]; // llenamos el array a devolver con los primeros SearchItem del array de SearchItem ordenado
            }
            

            System.Console.WriteLine("Los resultados han sido devueltos");

            foreach(SearchItem item in Items)
            {
                System.Console.WriteLine($"el item {item.Title} tiene un score de {item.Score}");
            }

            return new SearchResult(Items, suggestion);
        }

        private static float ScoreCalculator (Query ToSearch, Dictionary < string, Dictionary<string, float>> Docs, string file) // método que calcula el score
        {
            float dotProduct = 0;
            float dim1 = 0;
            float dim2 = 0;

            foreach(string word in Docs[file].Keys)
                {
                    if(!ToSearch.DataQuery.ContainsKey(word))
                        dotProduct += 0;
                    else
                    {
                        dotProduct += ToSearch.DataQuery[word] * Docs[file][word];  
                        dim2 += (float)Math.Pow(ToSearch.DataQuery[word], 2); //para calcular la norma del vector 2
                    }

                    dim1 += (float)Math.Pow(Docs[file][word], 2); // para calcular la norma del vector 1
                }
            return dotProduct / ((dim1==0 || dim2==0)?1:(float)(Math.Sqrt(dim1) * Math.Sqrt(dim2))); // distancia coseno 
        }

        private static SearchItem[] Sort(SearchItem[] docs_score)// metodo que ordena el array de SearchItem segun sus scores
        {
            for (int i = 0; i < docs_score.Length; i++)
            {
                for (int j = i; j < docs_score.Length; j++)
                {    if (docs_score[j].Score > docs_score[i].Score)
                    {
                        SearchItem temp = docs_score[j];
                        docs_score[j] = docs_score[i];
                        docs_score[i] = temp;
                    }
                }
            }

            return docs_score;
        }  
          
          
           #endregion
}