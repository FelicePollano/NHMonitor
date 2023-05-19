
using Moq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;
using NHMonitor.Probe;
using NHMonitor.Receiver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.Test
{
    [TestFixture]
    public class NHibernateFixtures
    {
        int syncDelay = 100;
        ISession session;
        [SetUp]
        public void SetUp() 
        {
            var cfg = new Configuration();
            cfg.Interceptor = new Interceptor("NH");
            cfg.DataBaseIntegration(d =>
            {
                d.Dialect<SQLiteDialect>();
                d.Driver<SQLite20Driver>();
                d.ConnectionString = "Data Source=:memory:;Version=3;New=True";
            });
            var map = new ModelMapper();
            map.AddMappings(new Type[] { typeof(ChildMap), typeof(MyEntityMap) }); ;
            var hbm = map.CompileMappingForAllExplicitlyAddedEntities();
            hbm.autoimport = false;
            cfg.AddMapping(hbm);
            SchemaExport se = new SchemaExport(cfg);
            
            session = cfg.BuildSessionFactory().OpenSession();
            se.Execute(true, true, false,session.Connection,null);
        }
        [Test]
        public async Task check_simple_query_intercepted()
        {
            var consumerMock = new Mock<IConsumer>();
            Listener receiver = new Listener(consumerMock.Object);
            receiver.StartServer();
            await Task.Delay(syncDelay); 
            var all = session.Query<MyEntity>().Where(u=>u.Integer>0).ToList();
            consumerMock.Verify(k => k.Query(It.IsAny<DateTime>(), It.IsRegex(".*select.*"),
               It.IsAny<IEnumerable<KeyValuePair<string, string>>>()), Times.Once);
            await receiver.StopServer();
        }
        [TearDown]
        public void TearDown()
        {
            session.Close();
        }
        public class ChildMap : ClassMapping<Child>
        {
            public ChildMap()
            {
                Id(x => x.Id, u => { u.Generator(Generators.Identity); u.Column("id"); });
                Property(x => x.Name);
            }
        }
        public class MyEntityMap : ClassMapping<MyEntity>
        {
            public MyEntityMap()
            {
                Id(x => x.Id, u => { u.Generator(Generators.Identity); u.Column("id"); });
                Property(x => x.Integer);
                Property(x => x.S1);
                Bag(u => u.Children,f=>f.Key(k=> { k.Column("parentId"); }),m=>m.OneToMany(m=>m.Class(typeof(Child))));
            }
        }
        
    }
    public class Child
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
    public class MyEntity
    {
        public virtual int Id { get; set; }
        public virtual int Integer { get; set; }
        public virtual string S1 { get; set; }
        public virtual IEnumerable<Child> Children { get; set; }
    }
}
