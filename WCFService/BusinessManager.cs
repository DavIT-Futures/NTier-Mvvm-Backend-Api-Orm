using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TestApp.Entity;
using TestApp.Server.Service;

namespace TestApp.WCFService
{
    public class BusinessManager
    {
        private Dictionary<Type, Type> mapEntitiesInterfaces = new Dictionary<Type, Type>();

        private static BusinessManager instance = null;
        public static BusinessManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new BusinessManager();
                return instance;
            }
        }

        private BusinessManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            Type serviceBaseType = typeof(EntityService<EntityObject>);
            Type entityBaseType = typeof(EntityObject);

            Assembly assembly = typeof(EntityService<>).Assembly;
            Type[] types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.BaseType != null && type.BaseType.Name == serviceBaseType.Name)
                {
                    if (type.BaseType.GenericTypeArguments.Count() > 0 &&
                        type.BaseType.GenericTypeArguments[0].BaseType != null &&
                        type.BaseType.GenericTypeArguments[0].BaseType.FullName == entityBaseType.FullName)
                    {
                        mapEntitiesInterfaces[type.BaseType.GenericTypeArguments[0]] = type;
                    }
                }
            }
        }

        public EntityObject GetById(string typeName, int id)
        {
            return GetEntity(typeName, nameof(GetById), new object[] { id });
        }

        public List<EntityObject> GetByTemplate(string typeName, EntityObject template)
        {
            return GetList(typeName, nameof(GetByTemplate), new object[] { template });
        }

        public List<EntityObject> GetAll(string typeName)
        {
            return GetList(typeName, nameof(GetAll), new object[] { });
        }

        private EntityObject GetEntity(string typeName, string methodName, object[] methodParams)
        {
            Type entityType = typeof(EntityObject).Assembly.GetType(typeName);
            Type serviceType = mapEntitiesInterfaces[entityType];
            var service = serviceType.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            MethodInfo mi = serviceType.GetMethod(methodName);
            EntityObject result = mi.Invoke(service, methodParams) as EntityObject;
            return result;
        }

        private List<EntityObject> GetList(string typeName, string methodName, object[] methodParams)
        {
            Type entityType = typeof(EntityObject).Assembly.GetType(typeName);
            Type serviceType = mapEntitiesInterfaces[entityType];
            var service = serviceType.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            MethodInfo mi = serviceType.GetMethod(methodName);
            IList list = mi.Invoke(service, methodParams) as IList;
            List<EntityObject> result = new List<EntityObject>();
            foreach (EntityObject item in list)
            {
                result.Add(item);
            }
            return result;
        }
    }
}