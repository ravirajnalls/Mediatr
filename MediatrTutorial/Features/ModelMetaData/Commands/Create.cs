using AutoMapper;
using FluentValidation;
using MediatR;
using MediatrTutorial.Data;
using MediatrTutorial.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatrTutorial.Features.ModelMetaData.Commands
{
    public class Create
    {
        public class ModelMetaDataCommand : IRequest<string>
        {
            public Guid? ModelMetaDataId { get; set; }
            public string Name { get; set; }
            public string Project { get; set; }
            public bool Active { get; set; }
            public string Exec_mode { get; set; }
            public string Exec_env { get; set; }
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
            public string Workload_profile_id { get; set; }
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

        public class Validator : AbstractValidator<ModelMetaDataCommand>
        {
            public Validator()
            {
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ModelMetaDataCommand, BaseModelMetaData>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Input, MediatrTutorial.Domain.Input>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Output, MediatrTutorial.Domain.Output>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.ExecutionModeBatch, MediatrTutorial.Domain.ExecutionModeBatch>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.ExecutionModeRealtime, MediatrTutorial.Domain.ExecutionModeRealtime>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Training, MediatrTutorial.Domain.Training>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.ClusterDetails, MediatrTutorial.Domain.ClusterDetails>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Tags, MediatrTutorial.Domain.Tags>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Data, MediatrTutorial.Domain.Data>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Artifact, MediatrTutorial.Domain.Artifact>();
            }
        }

        public class Handler : IRequestHandler<ModelMetaDataCommand, string>
        {
            private readonly IMapper mapper;
            private readonly IMongoDbContext mongoDbContext;
            public Handler(IMapper mapper, IMongoDbContext mongoDbContext)
            {
                this.mapper = mapper;
                this.mongoDbContext = mongoDbContext;
            }

            public async Task<string> Handle(ModelMetaDataCommand request, CancellationToken cancellationToken)
            {
                var data = mapper.Map<BaseModelMetaData>(request);
                return request.ModelMetaDataId.HasValue ? await CreateNewModelMetadataVersion(request, data) : await CreateNewModelMetadata(request, data);
            }

            private async Task<string> CreateNewModelMetadata(ModelMetaDataCommand request, BaseModelMetaData data)
            {
                data.ModelMetaDataId = Guid.NewGuid();
                data.Version = 1;
                SetDefaultValues(data);
                await mongoDbContext.Create(data);
                return data.Id.ToString();
            }

            private async Task<string> CreateNewModelMetadataVersion(ModelMetaDataCommand request, BaseModelMetaData data)
            {
                var allModelMetadataVersions = (await this.mongoDbContext.FindAllVersionsByModelMetaDataId(request.ModelMetaDataId.Value));
                if (data.Active)
                {
                    var activeModel = allModelMetadataVersions.Where(m => m.Active == true).FirstOrDefault();
                    activeModel.Active = false;
                    await this.mongoDbContext.Update(activeModel);
                }
                SetVersion(request, data, allModelMetadataVersions);
                SetDefaultValues(data);
                await mongoDbContext.Create(data);
                return data.Id.ToString();
            }

            private void SetVersion(ModelMetaDataCommand request, BaseModelMetaData data, List<BaseModelMetaData> allModelMetadataVersions)
            {
                var latestModel = allModelMetadataVersions.OrderByDescending(m => m.Version).FirstOrDefault();
                data.Version = latestModel.Version++;
            }

            private void SetDefaultValues(BaseModelMetaData data)
            {
                data.Created_by = "testuser"; // TODO: update user
                data.Date_created = DateTime.UtcNow;
                data.Date_modified = null;
            }
        }
    }
}
