﻿using DAL_CV_Fiches.Models.Graph;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_CV_Fiches.Repositories.Graph
{
    public class InstituitionGraphRepository : GraphRepositoy<Instituition>
    {
        public InstituitionGraphRepository()
        {
        }

        public InstituitionGraphRepository(DocumentClient documentClient, DocumentCollection documentCollection) : base(documentClient, documentCollection)
        {
        }

        public InstituitionGraphRepository(string Database, string Graph) : base(Database, Graph)
        {
        }

        public InstituitionGraphRepository(string Endpoint, string Key, string Database, string Graph) : base(Endpoint, Key, Database, Graph)
        {
        }
    }
}
