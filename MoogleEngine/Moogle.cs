﻿namespace MoogleEngine;
    
public static class Moogle
{
    static DataFolder Content;
    public static bool Initied = false;
    public static void Init()
    {
        if(!Initied)
        {
            Initied = true;

            Content = new DataFolder("../Content");
        }
    }
    public static SearchResult Query(string query) 
    {   
        Init();
        
        if(!string.IsNullOrEmpty(query) && Content.FilesRoot.Length != 0)
        {   
            SearchResult result = Engine.Query(query,Content);
            return result;
        }

        SearchItem ToChange = new SearchItem("Error", "Realice una nueva búsqueda", (float)0.05);
        SearchItem[] Change = new SearchItem[] {ToChange};
        return new SearchResult(Change, query);
    }
}
