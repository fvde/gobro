using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;

namespace GoBroBackend.Utilities
{
    public class SimpleMappedEntityDomainManager<TData, TModel>
    : MappedEntityDomainManager<TData, TModel>
    where TData : class, ITableData
    where TModel : EntityData
    {
        public SimpleMappedEntityDomainManager(DbContext context,
            HttpRequestMessage request, ApiServices services)
            : base(context, request, services)
        {

        }

        public override SingleResult<TData> Lookup(string id)
        {
            return this.LookupEntity(item => item.Id == id);
        }

        public override Task<TData> UpdateAsync(string id, Delta<TData> patch)
        {
            return this.UpdateEntityAsync(patch, id);
        }

        public override Task<bool> DeleteAsync(string id)
        {
            return this.DeleteItemAsync(id);
        }
    }
}