using System;
using System.Collections.Generic;
using TestApp.Entity;

namespace TestApp.WCFService
{
    public class EntityWCFService : IEntityWCFService
    {
        public EntityObject GetById(string typeName, int id)
        {
            return BusinessManager.Instance.GetById(typeName, id);
        }

        public List<EntityObject> GetByTemplate(string typeName, EntityObject template)
        {
            return BusinessManager.Instance.GetByTemplate(typeName, template);
        }

        public List<EntityObject> GetAll(string typeName)
        {
            return BusinessManager.Instance.GetAll(typeName);
        }
    }
}
