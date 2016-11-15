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
            //Setup indexer
            directory = FSDirectory.Open(@"D:\RepositoriosMProject\lucene\");
            analyzer = new SnowballAnalyzer(Lucene.Net.Util.Version.LUCENE_30, "Spanish");
            sp = new SpanishStemmer();
            sp.Stem();
        }

        public List<ScoreDoc> search(Dictionary<string, string> dat, string text, string typ, string cars,string usr)
        {
            //string req = " (  keym: " + dat["keym"] + " AND idUsu:" + dat["idUsu"] + "  ";
            //dat["idUsu"] = "2";
            //System.Windows.MessageBox.Show(dat["idCar"]);
            //string req = " (  idUsu:" + dat["idUsu"]+" ) ";
            typ = " ( type:" + typ + ") ";
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
                               new string[] { "nom", "desc", "cont" },
                               analyzer
                               );
                //QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "<default field>", analyzer);

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
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (vis:1 || vis:2 || vis:3) AND usuOwn:" + dat["usuAct"] + ") OR ( ( vis:2 || vis:3 ) AND NOT usuOwn:" + dat["usuAct"] + ") OR ( vis:1 AND (" + usr + ")) )");
                        else
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (vis:1 || vis:2 || vis:3) AND usuOwn:" + dat["usuAct"] + ") OR ( ( vis:2 || vis:3 ) AND NOT usuOwn:" + dat["usuAct"] + ") )");
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
                        //if (cars.Length > 0)
                        if(usr.Length>0)
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (vis:1 || vis:2 || vis:3) AND usuOwn:" + dat["usuAct"] + ") OR ( ( vis:2 || vis:3 ) AND NOT usuOwn:" + dat["usuAct"] + ") OR ( vis:1 AND (" + usr + ")) )" + " AND  (\"" + text + "\"~)");
                        else
                            query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (vis:1 || vis:2 || vis:3) AND usuOwn:" + dat["usuAct"] + ") OR ( ( vis:2 || vis:3 ) AND NOT usuOwn:" + dat["usuAct"] + ")  )" + " AND  (\"" + text + "\"~)");
                        //query = parser.Parse("( "+cars + " ) AND " +typ + " AND " + "(\"" + text + "\"~)" + " AND (( (vis:1 || vis:2 || vis:3) AND own:2) OR ( ( vis:2 || vis:3 ) AND NOT own:2))");
                        //else
                        //    query = parser.Parse(typ + " AND " + "(\"" + text + "\"~)");

                    }
                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);
                        if (hits.TotalHits == 0)
                        {
                            parser.FuzzyMinSim = (float)0.8;
                            text = text + "~";
                            text = text.Replace(" ", "~ ");

                            //System.Windows.MessageBox.Show(text + "    " + text.Length);

                            if (text.Length > 1)
                            {
                                if(usr.Length>0)
                                    query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (vis:1 || vis:2 || vis:3) AND usuOwn:" + dat["usuAct"] + ") OR ( ( vis:2 || vis:3 ) AND NOT usuOwn:" + dat["usuAct"] + ") OR ( vis:1 AND (" + usr + ")) )" + " AND (" + text + ")");
                                else
                                    query = parser.Parse("( " + cars + " ) AND " + typ + " AND (( (vis:1 || vis:2 || vis:3) AND usuOwn:" + dat["usuAct"] + ") OR ( ( vis:2 || vis:3 ) AND NOT usuOwn:" + dat["usuAct"] + ")  )" + " AND (" + text + ")");
                            }
                                
                            //query = parser.Parse("( "+cars + " ) AND " +typ + " AND " + "(" + text + ")" + " AND " + cars + " AND (( (vis:1 || vis:2 || vis:3) AND own:2) OR ( ( vis:2 || vis:3 ) AND NOT own:2))");
                            // else
                            //     query = parser.Parse(req + " AND " + typ + " AND "+ cars);
                            hits = searcher.Search(query, totDocs);
                        }

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                        //System.Windows.MessageBox.Show(err.ToString());
                    }
                }

            }
            return null;
        }

        public List<ScoreDoc> publicSearch(string text, string typ, string cars)
        {
            //string req = " (  keym: " + dat["keym"] + " AND idUsu:" + dat["idUsu"] + "  ";
            //dat["idUsu"] = "2";
            //System.Windows.MessageBox.Show(dat["idCar"]);
            //string req = " (  idUsu:" + dat["idUsu"]+" ) ";
            typ = " ( type:" + typ + ") ";
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
                               new string[] { "nom", "desc", "cont" },
                               analyzer
                               );
                //QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "<default field>", analyzer);

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
                        query = parser.Parse("( " + cars + " ) AND " + typ + " AND vis:3 ");
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
                        //if (cars.Length > 0)
                        query = parser.Parse("( " + cars + " ) AND " + typ + " AND vis:3 " + " AND  (\"" + text + "\"~)");
                        //query = parser.Parse("( "+cars + " ) AND " +typ + " AND " + "(\"" + text + "\"~)" + " AND (( (vis:1 || vis:2 || vis:3) AND own:2) OR ( ( vis:2 || vis:3 ) AND NOT own:2))");
                        //else
                        //    query = parser.Parse(typ + " AND " + "(\"" + text + "\"~)");

                    }
                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);
                        if (hits.TotalHits == 0)
                        {
                            parser.FuzzyMinSim = (float)0.8;
                            text = text + "~";
                            text = text.Replace(" ", "~ ");

                            //System.Windows.MessageBox.Show(text + "    " + text.Length);

                            if (text.Length > 1)
                                query = parser.Parse("( " + cars + " ) AND " + typ + " AND vis:3 " + " AND (" + text + ")");
                            //query = parser.Parse("( "+cars + " ) AND " +typ + " AND " + "(" + text + ")" + " AND " + cars + " AND (( (vis:1 || vis:2 || vis:3) AND own:2) OR ( ( vis:2 || vis:3 ) AND NOT own:2))");
                            // else
                            //     query = parser.Parse(req + " AND " + typ + " AND "+ cars);
                            hits = searcher.Search(query, totDocs);
                        }

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                        //System.Windows.MessageBox.Show(err.ToString());
                    }
                }

            }
            return null;
        }

        public List<ScoreDoc> publicSearchPosition()
        {
            string typ = " ( type:img ) ";
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
                               new string[] { "location", "longitude" },
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
                    query = parser.Parse(typ + " AND vis:3 AND location:* AND longitude:* ");

                    try
                    {
                        TopDocs hits = searcher.Search(query, totDocs);

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
            typ = " ( type: " + typ + " ) ";
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
                               new string[] { "nom", "desc", "cont" },
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
                        query = parser.Parse(typ + " AND vis:3 ");
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
                        query = parser.Parse(typ + " AND vis:3 " + " AND  (\"" + text + "\"~)");
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
                                query = parser.Parse(typ + " AND vis:3 " + " AND (" + text + ")");
                            hits = searcher.Search(query, totDocs);
                        }

                        List<ScoreDoc> sc = hits.ScoreDocs.ToList<ScoreDoc>();
                        totSear = sc.Count;
                        return sc;
                    }
                    catch (Exception err)
                    {
                        //System.Windows.MessageBox.Show(err.ToString());
                    }
                }

            }
            return null;
        }
        public ScoreDoc searchFile(Dictionary<string, string> inf)
        {
            //string req = " (  keym: " + dat["keym"] + " AND idUsu:" + dat["idUsu"] + "  ";
            //dat["idUsu"] = "2";
            //System.Windows.MessageBox.Show(dat["idCar"]);
            //string req = " (  idUsu:" + dat["idUsu"]+" ) ";
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
                               new string[] { },
                               analyzer
                               );
                //QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "<default field>", analyzer);

                totDocs = 1;
                //Supply conditions
                parser.AllowLeadingWildcard = true;

                //Do the search
                Query query;
                query = parser.Parse(" idCar:" +inf["idCar"]+ " AND keym:" +inf["keym"]+ " AND usuCar:" + inf["usuCar"] + " AND idFile:"+inf["idFile"]);
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

            idFile
            usuCar =>caracterisitca
            usuOwn =>Dueño
            idCar
            keym
            type
            nom
            src
            desc
            cont
            srcGif
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
                doc.Add(new Field("idFile", info["idFile"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                try
                {
                    //datos correspondientes a la ubicacion de la imagen
                    doc.Add(new Field("location", info["location"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("longitude", info["longitude"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                catch { }

                try
                {
                    //usuCar => idUsuario   => usuario de la caracteristica
                    //usuOwn => usuOwn      => Usuario propietario del archivo
                    doc.Add(new Field("usuCar", info["usuCar"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("usuOwn", info["usuOwn"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                catch (Exception err) { }

                doc.Add(new Field("idCar", info["idCar"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("keym", info["keym"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                doc.Add(new Field("vis", info["vis"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("srcServ", info["srcServ"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("nom2", info["nom2"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                doc.Add(new Field("type", info["type"], Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("nom", info["nom"], Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("src", info["src"], Field.Store.YES, Field.Index.NOT_ANALYZED));

                try
                {
                    if (info["desc"].Length != 0)
                        doc.Add(new Field("desc", info["desc"], Field.Store.YES, Field.Index.ANALYZED));
                }
                catch { }

                try
                {
                    if (info["cont"].Length != 0)
                        doc.Add(new Field("cont", info["cont"], Field.Store.YES, Field.Index.ANALYZED));
                }
                catch { }
                if (info["type"].Equals("vid"))
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
                //MultiFieldQueryParser parser = new MultiFieldQueryParser(
                //              Lucene.Net.Util.Version.LUCENE_30,
                //              new string[] { "idCar", "idFile", "idUsu", "keym" },
                //              analyzer
                //              );
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "<default field>", analyzer);
                Query query;

                query = parser.Parse(
                    "idCar:" + info["idCar"] +
                    " AND idFile:" + info["idFile"] +
                    " AND usuCar:" + info["usuCar"]
                    //+" AND keym:M1"
                    );
                writer.DeleteDocuments(query);
                writer.Optimize();
                writer.Dispose();
                return true;
            }
            catch (Exception err)
            {
                //System.Windows.MessageBox.Show("Error lcnDel:" + err.ToString());
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
                //System.Windows.MessageBox.Show("Error lcnUpdate:" + err.ToString());
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


/*

"dependencies": {
        "Lucene.Net": "3.0.3",
        "Lucene.Net.Contrib": "3.0.3"
    }
*/
