using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using GoBroBackend.DataObjects;
using GoBroBackend.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;
using System.Data.Entity.Migrations;
using GoBroBackend.Login;
using GoBroBackend.Migrations;

namespace GoBroBackend
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // enable custom login
            options.LoginProviders.Add(typeof(CustomLoginProvider));

#if DEBUG
            config.SetIsHosted(true);
#endif
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Define a map from the database type to 
                // client type. Used when getting data.

                // CHALLENGE //
                cfg.CreateMap<Challenge, ChallengeDto>()
                .ForMember(challengeDto => challengeDto.GroupId,
                        map => map.MapFrom(challenge => challenge.Group.Id));

                // COMMENT //
                cfg.CreateMap<Comment, CommentDto>()
                    .ForMember(commentDto => commentDto.UserId,
                        map => map.MapFrom(comment => comment.User.Id))
                    .ForMember(commentDto => commentDto.UserUsername,
                        map => map.MapFrom(comment => comment.User.DisplayName))
                    .ForMember(commentDto => commentDto.EntryId,
                        map => map.MapFrom(comment => comment.Entry.Id));

                // ENTRY //
                cfg.CreateMap<Entry, EntryDto>()
                    .ForMember(entryDto => entryDto.UserId,
                        map => map.MapFrom(entry => entry.User.Id))
                    .ForMember(entryDto => entryDto.UserUsername,
                        map => map.MapFrom(entry => entry.User.DisplayName));

                // USER //
                cfg.CreateMap<User, UserDto>()
                    .ForMember(userDto => userDto.GroupId,
                        map => map.MapFrom(user => user.Group.Id));


                // Define a map from the client type to the database
                // type. Used when inserting and updating data.

                // CHALLENGE //
                cfg.CreateMap<ChallengeDto, Challenge>();

                // COMMENT //
                cfg.CreateMap<CommentDto, Comment>();

                // ENTRY //
                cfg.CreateMap<EntryDto, Entry>();

                // USER //
                cfg.CreateMap<UserDto, User>();
            });

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
#if DEBUG
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
#endif

            // Set default and null value handling to "Include" for Json Serializer
            config.Formatters.JsonFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;

            // Migrations
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
        }
    }
}

