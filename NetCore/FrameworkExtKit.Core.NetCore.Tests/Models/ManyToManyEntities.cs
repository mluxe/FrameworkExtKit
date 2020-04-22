using FrameworkExtKit.Core.Data.Entity.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkExtKit.Core.NetCore.Tests.Models
{
    public class ManyToManyEntityA {
        public int Id { get; set; }
        public String Name { get; set; }

        public ManyToManyEntityA() {
            ManyToManyEntityABMappings = new List<ManyToManyEntityABMapping>();
            ManyToManyEntityBs = new JoinCollectionFacade<ManyToManyEntityB, ManyToManyEntityA, ManyToManyEntityABMapping>(this, ManyToManyEntityABMappings);
        }
        [NotMapped]
        public ICollection<ManyToManyEntityB> ManyToManyEntityBs { get; set; }
        private ICollection<ManyToManyEntityABMapping> ManyToManyEntityABMappings { get; set; }
    }
    public class ManyToManyEntityB {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public ManyToManyEntityB() {
            ManyToManyEntityABMappings = new List<ManyToManyEntityABMapping>();
            ManyToManyEntityAs = new JoinCollectionFacade<ManyToManyEntityA, ManyToManyEntityB, ManyToManyEntityABMapping>(this, ManyToManyEntityABMappings);
        }
        [NotMapped]
        public ICollection<ManyToManyEntityA> ManyToManyEntityAs { get; set; }
        private ICollection<ManyToManyEntityABMapping> ManyToManyEntityABMappings { get; set; }
    }

    public class ManyToManyEntityABMapping : IJoinEntity<ManyToManyEntityA>, IJoinEntity<ManyToManyEntityB> {
        public int ManyToManyEntityAId { get; set; }
        public ManyToManyEntityA EntityA { get; set; }
        public int ManyToManyEntityBId { get; set; }
        public ManyToManyEntityB EntityB { get; set; }

        ManyToManyEntityA IJoinEntity<ManyToManyEntityA>.Navigation {
            get => EntityA;
            set => EntityA = value;
        }

        ManyToManyEntityB IJoinEntity<ManyToManyEntityB>.Navigation {
            get => EntityB;
            set => EntityB = value;
        }
    }
}
