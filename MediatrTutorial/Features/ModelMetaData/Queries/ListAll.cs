using AutoMapper;
using MediatR;
using MediatrTutorial.Data;
using MediatrTutorial.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatrTutorial.Features.ModelMetaData.Queries
{
    public class ListAll
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<ModelMetaData> Models { get; set; }
        }

        public class ModelMetaData
        {
            public string Name { get; set; }
            public string Project { get; set; }
            public bool Active { get; set; }
            public int Version { get; set; }
            public string Exec_mode { get; set; }
            public string Exec_env { get; set; }
            public DateTime Date_created { get; set; }
            public DateTime? Date_modified { get; set; }
            public Input Input { get; set; }
            public Output Output { get; set; }
            public ExecutionModeBatch Exec_mode_batch { get; set; }
            public ExecutionModeRealtime Exec_mode_realtime { get; set; }
            public Training Training { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
            public Dictionary<string, string> HyperParameters { get; set; }
            public Tags Tags { get; set; }
        }

        public class Input
        {
            public Data Data { get; set; }
            public Artifact Artifact { get; set; }
        }

        public class Output
        {
            public Data Data { get; set; }
            public Artifact Artifact { get; set; }
        }

        public class ExecutionModeBatch
        {
            public string Frequency { get; set; }
            public string Cron_schedule { get; set; }
            public string Temporality { get; set; }
            public string Notebook_path { get; set; }
            public ClusterDetails Cluster_details { get; set; }
        }

        public class ClusterDetails
        {
            public Dictionary<string, string> Context { get; set; }
        }

        public class ExecutionModeRealtime
        {
            public Dictionary<string, string> Context { get; set; }
        }

        public class Training
        {
            public string Experiment { get; set; }
            public string ExperimentId { get; set; }
            public Dictionary<string, string> Metrics { get; set; }
        }

        public class Tags
        {
            public string Icto { get; set; }
            public string Environment { get; set; }
            public Dictionary<string, string> AdditionalTags { get; set; }
        }

        public class Data
        {
            public string Table_type { get; set; }
            public string Table { get; set; }
            public Dictionary<string, string> Context { get; set; }
        }

        public class Artifact
        {
            public string Flavor { get; set; }
            public string Path { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<BaseModelMetaData, ModelMetaData>();
                CreateMap<MediatrTutorial.Domain.Input, Input>();
                CreateMap<MediatrTutorial.Domain.Output, Output>();
                CreateMap<MediatrTutorial.Domain.ExecutionModeBatch, ExecutionModeBatch>();
                CreateMap<MediatrTutorial.Domain.ExecutionModeRealtime, ExecutionModeRealtime>();
                CreateMap<MediatrTutorial.Domain.Training, Training>();
                CreateMap<MediatrTutorial.Domain.ClusterDetails, ClusterDetails>();
                CreateMap<MediatrTutorial.Domain.Tags, Tags>();
                CreateMap<MediatrTutorial.Domain.Data, Data>();
                CreateMap<MediatrTutorial.Domain.Artifact, Artifact>();
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IMapper mapper;
            private readonly IMongoDbContext mongoDbContext;
            public Handler(IMapper mapper, IMongoDbContext mongoDbContext)
            {
                this.mapper = mapper;
                this.mongoDbContext = mongoDbContext;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var rawModels = await this.mongoDbContext.ListAll();
                var models = mapper.Map<List<ModelMetaData>>(rawModels);
                return new Result { Models = models };
            }
        }
    }
}
