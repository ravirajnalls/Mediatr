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
            public string ProjectId { get; set; }
            public string Name { get; set; }
            public int Version { get; set; }
            public string Etag { get; set; }
            public string Frequency { get; set; }
            public string Created { get; set; }
            public string ModifiedOn { get; set; }
            public string Owner { get; set; }
            public string Tags { get; set; }
            public string NotebookRef { get; set; }
            public string ExecutionId { get; set; }
            public Model Model { get; set; }
            public Study Study { get; set; }
            public Entity Entity { get; set; }
            public Parameters Parameters { get; set; }
            public Inputs Inputs { get; set; }
            public Hyperparameters Hyperparameters { get; set; }
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
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Model, MediatrTutorial.Domain.Model>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Study, MediatrTutorial.Domain.Study>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Entity, MediatrTutorial.Domain.Entity>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Parameters, MediatrTutorial.Domain.Parameters>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Inputs, MediatrTutorial.Domain.Inputs>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Metrics, MediatrTutorial.Domain.Metrics>();
                CreateMap<MediatrTutorial.Features.ModelMetaData.Commands.Create.Hyperparameters, MediatrTutorial.Domain.Hyperparameters>();
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
                data.ExperimentId = Guid.NewGuid().ToString();
                await mongoDbContext.Create(data);
                return data.ProjectId;
            }
        }
    }
}
