using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using GoBroBackend.Models;
using System.Web.Http.Controllers;
using GoBroBackend.DataObjects;
using System.Threading.Tasks;
using AutoMapper;

namespace GoBroBackend.Controllers
{
    [AuthorizeLevel(Microsoft.WindowsAzure.Mobile.Service.Security.AuthorizationLevel.Anonymous)]
    public class CommentController : ApiController
    {
        public ApiServices Services { get; set; }

        private MobileServiceContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
        }

        // POST api/Comment
        [HttpPost]
        public async Task<CommentDto> PostComment(string entryid, string userid, string content)
        {
            // Create new instance of db object
            Comment comment = new Comment();
            comment.CreatedAt = DateTimeOffset.Now;
            comment.Id = Guid.NewGuid().ToString();
            comment.User = context.Users.FirstOrDefault(j => (j.Id == userid));
            comment.Entry = context.Entries.FirstOrDefault(j => (j.Id == entryid));
            comment.Content = content;
            context.Comments.Add(comment);

            // increase number of comments
            // TODO is this atomic?
            var entry = context.Entries.FirstOrDefault(i => i.Id == entryid);
            if (entry != null)
            {
                entry.NumberOfComments += 1;
            }

            // store
            await context.SaveChangesAsync();

            //convert back to dto before sending back
            var commentDto = Mapper.Map<Comment, CommentDto>(comment);
            return commentDto;
        }

        [HttpGet]
        public async Task<IEnumerable<CommentDto>> GetComments(string entryid)
        {
            var results = context.Comments.Where(item => item.Entry.Id == entryid).ToList();
            // TODO PAGINATION
            var dtos = results.Select(c => Mapper.Map<Comment, CommentDto>(c));
            return dtos;
        }
    }
}
