using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatrTutorial.Domain
{
    public class BaseModelMetaData 
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("experimentId")]
        public string ExperimentId { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string Etag { get; set; }
        public string Frequency { get; set; }
        public string Created { get; set; }
        public string ModifiedOn { get; set; }
        public string Owner { get; set; }
        [BsonElement("tags")]
        public string Tags { get; set; }
        public string NotebookRef { get; set; }
        public string ExecutionId { get; set; }
        public Model Model { get; set; }
        public Study Study { get; set; }
        public Entity Entity { get; set; }
        public Parameters Parameters { get; set; }
        public Inputs Inputs { get; set; }
        // public Hyperparameters Hyperparameters { get; set; }
        public Metrics Metrics { get; set; }
        public string Results_Table { get; set; }
        public string Study_Completed { get; set; }
        public string Temporality { get; set; }
    }

    public class Model
    {
        public string ModelType { get; set; }
    }

    public class Study
    {
        public string Study_Id { get; set; }
        public int Study_Version { get; set; }
        public string Study_Name { get; set; }
        public string Study_Description { get; set; }
        public string Study_Notebook { get; set; }
        public string Study_Status { get; set; }
        public string Study_Initiated { get; set; }
    }

    public class Entity
    {
        public string Entity_Type { get; set; }
        public string Entity_Universe { get; set; }
    }

    public class Parameters
    {
        public string Entities { get; set; }
        public string Tickers { get; set; }
        public string Chain_ids { get; set; }
        public string Percentile { get; set; }
        public string Outlier_Detection_Methodology { get; set; }
        public string Distance_Feature_Vector { get; set; }
        public string Month_Over_Month_Feature_Vector { get; set; }
    }

    public class Inputs
    {
        public string Input_Table { get; set; }
        public string Date_Column { get; set; }
        public string Value_Column { get; set; }
        public string Ts_Id_Column { get; set; }
    }

    public class Hyperparameters
    {
    }

    public class Metrics
    {
        public string Sandbytes_Practice { get; set; }
        public string Plan_Id { get; set; }
        public string Plan_Revision { get; set; }
        public string Plan_Description { get; set; }
    }
}
