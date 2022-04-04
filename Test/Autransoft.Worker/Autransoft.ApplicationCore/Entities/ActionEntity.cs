using Autransoft.Template.EntityFramework.Lib.Entities;
using System;

namespace Autransoft.ApplicationCore.Entities
{
    public class ActionEntity : AutranSoftEntity
    {
        public Guid Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Ticker { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}