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
        [BsonElement("modelmetadataid")]
        public string ModelMetaDataId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("project")]
        public string Project { get; set; }
        [BsonElement("version")]
        public int Version { get; set; }
        [BsonElement("active")]
        public bool Active { get; set; }
        [BsonElement("exec_mode")]
        public string Exec_mode { get; set; }
        [BsonElement("exec_env")]
        public string Exec_env { get; set; }
        [BsonElement("created_by")]
        public string Created_by { get; set; }
        [BsonElement("date_created")]
        public DateTime Date_created { get; set; }
        [BsonElement("last_modified_by")]
        public string Last_modified_by { get; set; }
        [BsonElement("date_modified")]
        public DateTime? Date_modified { get; set; }
        [BsonElement("input")]
        public Input Input { get; set; }
        [BsonElement("output")]
        public Output Output { get; set; }
        [BsonElement("exec_mode_batch")]
        public ExecutionModeBatch Exec_mode_batch { get; set; }
        [BsonElement("exec_mode_realtime")]
        public ExecutionModeRealtime Exec_mode_realtime { get; set; }
        [BsonElement("training")]
        public Training Training { get; set; }
        [BsonElement("parameters")]
        public Dictionary<string, string> Parameters { get; set; }
        [BsonElement("hyperparameters")]
        public Dictionary<string, string> HyperParameters { get; set; }       
        [BsonElement("tags")]
        public Tags Tags { get; set; }
        [BsonElement("isDeleted")]
        public Tags isDeleted { get; set; }
    }

    public class Input
    {
        [BsonElement("data")]
        public Data Data { get; set; }
        [BsonElement("artifact")]
        public Artifact Artifact { get; set; }
    }

    public class Output
    {
        [BsonElement("data")]
        public Data Data { get; set; }
        [BsonElement("artifact")]
        public Artifact Artifact { get; set; }
    }

    public class ExecutionModeBatch
    {
        [BsonElement("frequency")]
        public string Frequency { get; set; }
        [BsonElement("cron_schedule")]
        public string Cron_schedule { get; set; }
        [BsonElement("temporality")]
        public string Temporality { get; set; }
        [BsonElement("notebook_path")]
        public string Notebook_path { get; set; }
        [BsonElement("cluster_details")]
        public ClusterDetails Cluster_details { get; set; }
    }

    public class ClusterDetails
    {
        [BsonElement("workload_profile_id")]
        public string Workload_profile_id { get; set; }
        [BsonElement("context")]
        public Dictionary<string, string> Context { get; set; }
    }

    public class ExecutionModeRealtime
    {
        [BsonElement("context")]
        public Dictionary<string, string> Context { get; set; }
    }

    public class Training
    {
        [BsonElement("experiment")]
        public string Experiment { get; set; }
        [BsonElement("experimentId")]
        public string ExperimentId { get; set; }
        [BsonElement("metrics")]
        public Dictionary<string, string> Metrics { get; set; }
    }

    public class Tags
    {
        [BsonElement("icto")]
        public string Icto { get; set; }
        [BsonElement("environment")]
        public string Environment { get; set; }
        [BsonElement("additionaltags")]
        public Dictionary<string, string> AdditionalTags { get; set; }
    }

    public class Data
    {
        [BsonElement("table_type")]
        public string Table_type { get; set; }
        [BsonElement("table")]
        public string Table { get; set; }
        [BsonElement("context")]
        public Dictionary<string, string> Context { get; set; }
    }

    public class Artifact
    {
        [BsonElement("flavor")]
        public string Flavor { get; set; }
        [BsonElement("path")]
        public string Path { get; set; }
    }
}
