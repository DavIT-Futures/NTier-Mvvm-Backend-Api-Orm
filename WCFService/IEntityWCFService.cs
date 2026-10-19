using System;
using System.Collections.Generic;
using CoreWCF;
using TestApp.Entity;

namespace TestApp.WCFService
{
    [ServiceContract]
    public interface IEntityWCFService
    {
        [OperationContract]
        EntityObject GetById(string typeName, int id);

        [OperationContract]
        List<EntityObject> GetByTemplate(string typeName, EntityObject template);

        [OperationContract]
        List<EntityObject> GetAll(string typeName);
    }
}
