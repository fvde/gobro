namespace GoBroBackend.Migrations
{
    using DataObjects;
    using Microsoft.WindowsAzure.Mobile.Service;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GoBroBackend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GoBroBackend.Models.MobileServiceContext context)
        {
            //  This method will be called after migrating to the latest version.

            var challenge = new Challenge()
            {
                Content = "Be the wrecking ball",
                Id = "default",
                IsUserGenerated = true,
                Entries = new List<Entry>()
            };

            var group = new Group()
            {
                Id = "default",
                DisplayName = "IT Community"
            };

            var user = new User()
            {
                DisplayName = "Max Mustermann",
                Email = "Max@mustermann.de",
                Id = "default",
                Group = group
            };

            AddOrUpdatePreservingCreatedAt(
                context.Users,
                user
            );

            group.CurrentChallenge = challenge;

            AddOrUpdatePreservingCreatedAt(
                context.Groups,
                group
            );

            var entry = new Entry
            {
                Challenge = challenge,
                ContainerName = "entries",
                Content = "Hello World!",
                Id = "default",
                ImageUri = "http://www.watchyourweb.de/assets/watchyourweb/mdb/2/trollgesicht.jpg",
                ResourceName = "default",
                User = user,
                Votes = 0
            };

            AddOrUpdatePreservingCreatedAt(
                context.Entries,
                entry
            );

            challenge.Entries.Add(entry);

            AddOrUpdatePreservingCreatedAt(
                context.Challenges,
                challenge
            );
        }

        /// <summary>
        /// Adds or updates an item in a DbSet. If the item is already present in the database the existing items CreatedAt value is preserved.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="item"></param>
        private void AddOrUpdatePreservingCreatedAt<T>(DbSet<T> set, T item) where T : EntityData
        {
            var existing = set.Where(i => i.Id == item.Id).FirstOrDefault();
            if (existing != null)
            {
                item.CreatedAt = existing.CreatedAt;
            }
            set.AddOrUpdate(i => i.Id, item);
        }
    }
}
