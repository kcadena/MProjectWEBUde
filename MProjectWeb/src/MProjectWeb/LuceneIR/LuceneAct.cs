using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




using Lucene.Net.Store;
using Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

using Lucene.Net.Util;
using Lucene.Net.Analysis.Snowball;
using System.IO;
using SF.Snowball.Ext;
using Contrib.Regex;
using MProjectWeb.Models.ModelController;



namespace MProjectWeb.LuceneIR
{

    public class LuceneAct
    {
        public int totDocs = 0;
        public int totSear = 0;
        private IndexWriter writer;
        public IndexSearcher searcher;
        private Lucene.Net.Store.Directory directory;
        private Analyzer analyzer;
        private SpanishStemmer sp;

        /*opt => 1:write   ;   2:Read */
        public LuceneAct()
        {
            DBCConfiguracion conf = new DBCConfiguracion();
            //Setup indexer
            directory = FSDirectory.Open(conf.getPathServer() + @"lucene\");
            analyzer = new SnowballAnalyzer(Lucene.Net.Util.Version.LUCENE_30, "Spanish");
            sp = new SpanishStemmer();
            sp.Stem();
        }

        public List<ScoreDoc> search(Dictionary<string, string> dat, string text, string typ, string cars,string usr)
        {
            typ = " ( tipo:" + typ + ") ";
            totSear = 0;
            bool est = true;
            try
            {
                searcher = new IndexSearcher(directory);
            }
            catch { est = false; }

            if (est)
            {
                MultiFieldQueryParser parser = new MultiFieldQueryParser(
                               Lucene.Net.Util.Version.LUCENE_30,
                               new string[] { "titulo", "descripcion", "contenido" },
                               analyzer
                               );
                IndexReader rea = IndexReader.Open(directory, false);
                totDocs = rea.MaxDoc;
                rea.Dispose();
                //Supply conditions
                parser.AllowLeadingWildcard = true;
                //Do the search
                Query query;
                if (totDocs > 0)
                {
                    if (text.Equals(""))
                    {
                        if (usr.Length > 0)
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (publicacion:1 || publicacion:2 || publicacion:3) AND id_usuario_arc:" + dat["usuAct"] + ") OR ( ( publicacion:2 || publicacion:3 ) AND NOT id_usuario_arc:" + dat["usuAct"] + ") OR ( publicacion:1 AND (" + usr + ")) )");
                        else
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (publicacion:1 || publicacion:2 || publicacion:3) AND id_usuario_arc:" + dat["usuAct"] + ") OR ( ( publicacion:2 || publicacion:3 ) AND NOT id_usuario_arc:" + dat["usuAct"] + ") )");
                    }

                    else
                    {
                        string[] car = new string[] { "+", "-", "&&", "||", "!", "(", ")", "{",
                        "}", "[", "]", "^", "\"", "~", "*", "?", ":", "\\", "AND", "OR" };
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text.ElementAt(i) == ' ')
                            {
                                try
                                {
                                    if (text.ElementAt(i + 1) == ' ')
                                    {
                                        text = text.Remove(i, 1);
                                        i--;
                                    }
                                }
                                catch
                                {
                                    text = text.Remove(i--, 1);
                                }

                            }

                            foreach (var x in car)
                            {
                                if (text.ElementAt(i) == x.ElementAt(0))
                                {
                                    char s = '\\';
                                    text = text.Insert(i, s.ToString());
                                    i++;
                                }
                            }
                        }
                        parser.FuzzyMinSim = (float)0.9;
                        if(usr.Length>0)
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (publicacion:1 || publicacion:2 || publicacion:3) AND id_usuario_arc:" + dat["usuAct"] + ") OR ( ( publicacion:2 || publicacion:3 ) AND NOT id_usuario_arc:" + dat["usuAct"] + ") OR ( publicacion:1 AND (" + usr + ")) )" + " AND  (\"" + text + "\"~)");
                        else
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (publicacion:1 || publicacion:2 || publicacion:3) AND id_usuario_arc:" + dat["usuAct"] + ") OR ( ( publicacion:2 || publicacion:3 ) AND NOT id_usuario_arc:" + dat["usuAct"] + ")  )" + " AND  (\"" + text + "\"~)");

                    }
                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);
                        if (hits.TotalHits == 0)
                        {
                            parser.FuzzyMinSim = (float)0.8;
                            text = text + "~";
                            text = text.Replace(" ", "~ ");
                            if (text.Length > 1)
                            {
                                if(usr.Length>0)
                                    query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (publicacion:1 || publicacion:2 || publicacion:3) AND id_usuario_arc:" + dat["usuAct"] + ") OR ( ( publicacion:2 || publicacion:3 ) AND NOT id_usuario_arc:" + dat["usuAct"] + ") OR ( publicacion:1 AND (" + usr + ")) )" + " AND (" + text + ")");
                                else
                                    query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (publicacion:1 || publicacion:2 || publicacion:3) AND id_usuario_arc:" + dat["usuAct"] + ") OR ( ( publicacion:2 || publicacion:3 ) AND NOT id_usuario_arc:" + dat["usuAct"] + ")  )" + " AND (" + text + ")");
                            }
                            hits = searcher.Search(query, totDocs);
                        }

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                    }
                }

            }
            return null;
        }

        public List<ScoreDoc> publicSearch(string text, string typ, string cars)
        {
            typ = " ( tipo:" + typ + ") ";
            totSear = 0;
            bool est = true;
            //Setup searcher
            try
            {
                searcher = new IndexSearcher(directory);
            }
            catch { est = false; }

            if (est)
            {
                MultiFieldQueryParser parser = new MultiFieldQueryParser(
                               Lucene.Net.Util.Version.LUCENE_30,
                               new string[] { "titulo", "descripcion", "contenido" },
                               analyzer
                               );

                IndexReader rea = IndexReader.Open(directory, false);
                totDocs = rea.MaxDoc;
                rea.Dispose();
                //Supply conditions
                parser.AllowLeadingWildcard = true;

                //Do the search
                Query query;
                if (totDocs > 0)
                {
                    if (text.Equals(""))
                    {
                        query = parser.Parse("( " + cars + " ) AND " + typ + " AND publicacion:3 ");
                    }

                    else
                    {
                        string[] car = new string[] { "+", "-", "&&", "||", "!", "(", ")", "{",
                        "}", "[", "]", "^", "\"", "~", "*", "?", ":", "\\", "AND", "OR" };
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text.ElementAt(i) == ' ')
                            {
                                try
                                {
                                    if (text.ElementAt(i + 1) == ' ')
                                    {
                                        text = text.Remove(i, 1);
                                        i--;
                                    }
                                }
                                catch
                                {
                                    text = text.Remove(i--, 1);
                                }

                            }

                            foreach (var x in car)
                            {
                                if (text.ElementAt(i) == x.ElementAt(0))
                                {
                                    char s = '\\';
                                    text = text.Insert(i, s.ToString());
                                    i++;
                                }
                            }
                        }
                        parser.FuzzyMinSim = (float)0.9;
                        query = parser.Parse("( " + cars + " ) AND " + typ + " AND publicacion:3 " + " AND  (\"" + text + "\"~)");
                    }
                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);
                        if (hits.TotalHits == 0)
                        {
                            parser.FuzzyMinSim = (float)0.8;
                            text = text + "~";
                            text = text.Replace(" ", "~ ");

                            if (text.Length > 1)
                                query = parser.Parse("( " + cars + " ) AND " + typ + " AND publicacion:3 " + " AND (" + text + ")");
                            hits = searcher.Search(query, totDocs);
                        }

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                    }
                }

            }
            return null;
        }

        public List<ScoreDoc> publicSearchPosition()
        {
            string typ = " ( tipo:img ) ";
            totSear = 0;
            bool est = true;
            //Setup searcher
            try
            {
                searcher = new IndexSearcher(directory);
            }
            catch { est = false; }

            if (est)
            {
                MultiFieldQueryParser parser = new MultiFieldQueryParser(
                               Lucene.Net.Util.Version.LUCENE_30,
                               new string[] { "localizacion", "longitud" },
                               analyzer
                               );
                IndexReader rea = IndexReader.Open(directory, false);
                totDocs = rea.MaxDoc;
                rea.Dispose();
                //Supply conditions
                parser.AllowLeadingWildcard = true;

                //Do the search
                Query query;
                if (totDocs > 0)
                {
                    query = parser.Parse(typ + " AND publicacion:3 AND localizacion:* AND longitud:* ");

                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);
                        //p//
                        Lucene.Net.Documents.Document doc = searcher.Doc(hits.ScoreDocs.ElementAt(0).Doc);

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                    }
                }

            }
            return null;
        }
        public List<ScoreDoc> publicFileSearch(string text, string typ)
        {
            typ = " ( tipo: " + typ + " ) ";
            totSear = 0;
            bool est = true;
            //Setup searcher
            try
            {
                searcher = new IndexSearcher(directory);
            }
            catch { est = false; }

            if (est)
            {
                MultiFieldQueryParser parser = new MultiFieldQueryParser(
                               Lucene.Net.Util.Version.LUCENE_30,
                               new string[] { "titulo", "descripcion", "contenido" },
                               analyzer
                               );
                IndexReader rea = IndexReader.Open(directory, false);
                totDocs = rea.MaxDoc;
                rea.Dispose();
                //Supply conditions
                parser.AllowLeadingWildcard = true;

                //Do the search
                Query query;
                if (totDocs > 0)
                {
                    if (text.Equals(""))
                    {
                        query = parser.Parse(typ );
                        //query = parser.Parse(typ + " AND publicacion:3 ");
                    }

                    else
                    {
                        string[] car = new string[] { "+", "-", "&&", "||", "!", "(", ")", "{",
                        "}", "[", "]", "^", "\"", "~", "*", "?", ":", "\\", "AND", "OR" };
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text.ElementAt(i) == ' ')
                            {
                                try
                                {
                                    if (text.ElementAt(i + 1) == ' ')
                                    {
                                        text = text.Remove(i, 1);
                                        i--;
                                    }
                                }
                                catch
                                {
                                    text = text.Remove(i--, 1);
                                }

                            }

                            foreach (var x in car)
                            {
                                if (text.ElementAt(i) == x.ElementAt(0))
                                {
                                    char s = '\\';
                                    text = text.Insert(i, s.ToString());
                                    i++;
                                }
                            }
                        }
                        parser.FuzzyMinSim = (float)0.9;
                        query = parser.Parse(typ + " AND publicacion:3 " + " AND  (\"" + text + "\"~)");
                    }
                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);
                        //p//
                        Lucene.Net.Documents.Document doc = searcher.Doc(hits.ScoreDocs.ElementAt(0).Doc);

                        if (hits.TotalHits == 0)
                        {
                            parser.FuzzyMinSim = (float)0.8;
                            text = text + "~";
                            text = text.Replace(" ", "~ ");

                            if (text.Length > 1)
                                query = parser.Parse(typ + " AND publicacion:3 " + " AND (" + text + ")");
                            hits = searcher.Search(query, totDocs);
                        }

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                    }
                }

            }
            return null;
        }

        public ScoreDoc searchFile(Dictionary<string, string> inf)
        {
            totSear = 0;
            bool est = true;
            try
            {
                searcher = new IndexSearcher(directory);
            }
            catch { est = false; }

            if (est)
            {
                MultiFieldQueryParser parser = new MultiFieldQueryParser(
                               Lucene.Net.Util.Version.LUCENE_30,
                               new string[] { },
                               analyzer
                               );
                totDocs = 1;
                //Supply conditions
                parser.AllowLeadingWildcard = true;
                //Do the search
                Query query;
                query = parser.Parse(" id_caracteristica:" + inf["id_caracteristica"]+ " AND keym:" +inf["keym_car"]+ " AND id_cusuario_car:" + inf["id_cusuario_car"] + " AND id_archivo:"+inf["id_archivo"]);
                TopDocs hits = searcher.Search(query, totDocs);
                ScoreDoc sc = hits.ScoreDocs.First();
                return sc;
            }
            return null;
        }
        public bool luceneAdd(Dictionary<string, string> info)
        {
            /*
             op => opciones para  1=>adicionar   2=>Actualizar

             keym_arc            
             id_archivo          
             id_usuario_arc      

             keym_car            
             id_caracteristica   
             id_usuario_car      

             nombre_archivo      

             titulo          
             subtitulo       
             descripcion     
             contenido       

             fecha_creacion              
             fecha_ultima_modificacion   

             publicacion 
             tipo        

             localizacion    
             longitud        
             srcGifServ      
             srcServ             
             srcGif          
             src             
             */



            try
            {
                try { writer.Dispose(); } catch { }
                try
                {
                    writer = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                catch
                {
                    writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                //Add documents to the index
                Document doc = new Document();

                doc.Add(new Field("keym_arc", info["keym_arc"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("id_archivo", info["id_archivo"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("id_usuario_arc", info["id_usuario_arc"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                doc.Add(new Field("keym_car", info["keym_car"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("id_caracteristica", info["id_caracteristica"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("id_usuario_car", info["id_usuario_car"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                try
                {
                    //datos correspondientes a la ubicacion de la imagen
                    doc.Add(new Field("localizacion", info["localizacion"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("longitud", info["longitud"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                catch { }

                doc.Add(new Field("nombre_archivo", info["nomobre_archivo"], Field.Store.YES, Field.Index.ANALYZED));

                doc.Add(new Field("titulo", info["titulo"], Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("subtitulo", info["subtitulo"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                try
                {
                    if (info["descripcion"].Length != 0)
                        doc.Add(new Field("descripcion", info["descripcion"], Field.Store.YES, Field.Index.ANALYZED));
                }
                catch { }
                try
                {
                    if (info["contenido"].Length != 0)
                        doc.Add(new Field("contenido", info["contenido"], Field.Store.YES, Field.Index.ANALYZED));
                }
                catch { }

                doc.Add(new Field("fecha_creacion", info["fecha_creacion"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("fecha_ultima_modificacion", info["fecha_ultima_modificacion"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                doc.Add(new Field("tipo", info["tipo"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("publicacion", info["publicacion"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                doc.Add(new Field("srcServ", info["srcServ"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("src", info["src"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                if (info["tipo"].Equals("vid"))
                {
                    try
                    {
                        doc.Add(new Field("srcGif", info["srcGif"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                        doc.Add(new Field("srcGifServ", info["srcGifServ"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    catch { }

                }


                writer.AddDocument(doc);

                writer.Optimize();
                writer.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool luceneDel(Dictionary<string, string> info)
        {
            try
            {
                 try { writer.Dispose(); } catch { }
                writer = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
                
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "<default field>", analyzer);
                Query query;

                query = parser.Parse(
                    "id_archivo:" + info["id_archivo"] +
                    " AND keym_arc:" + info["keym_arc"] +
                    " AND id_usuario_arc:" + info["id_usuario_arc"]
                    );
                writer.DeleteDocuments(query);
                writer.Optimize();
                writer.Dispose();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        public bool luceneUpdate(Dictionary<string, string> info)
        {
            try
            {
                bool stAdd = false;
                bool stDel = luceneDel(info);
                if (stDel)
                    return luceneAdd(info);
                else
                    return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        public void closeLucene()
        {
            //Close the writer
            //writer.Flush();
            //searcher.Dispose();
        }
    }
}

